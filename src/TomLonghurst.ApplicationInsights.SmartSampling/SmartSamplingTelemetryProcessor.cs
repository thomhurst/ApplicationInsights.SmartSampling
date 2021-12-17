using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.Caching.Memory;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

public class SmartSamplingTelemetryProcessor : AdaptiveSamplingTelemetryProcessor, ITelemetryProcessor, IDisposable
{
    private readonly SmartSamplingOptions _smartSamplingOptions;
    private readonly ITelemetryProcessor _skipSamplingTelemetryProcessor;
    private readonly MemoryCache _telemetryMemoryCache;

    public SmartSamplingTelemetryProcessor(SmartSamplingOptions smartSamplingOptions, 
        SamplingPercentageEstimatorSettings percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : base(percentageEstimatorSettings, (second, percentage, samplingPercentage, changed, settings) => {}, skipSamplingTelemetryProcessor)
    {
        _smartSamplingOptions = smartSamplingOptions;
        _skipSamplingTelemetryProcessor = skipSamplingTelemetryProcessor;
        _telemetryMemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public new void Process(ITelemetry item)
    {
        var operationId = item?.Context?.Operation?.Id;
        
        if (item is MetricTelemetry || string.IsNullOrEmpty(operationId))
        {
            // Metrics should always be sampled!
            // No operation ID? Then we can't tie it to a journey
            base.Process(item);
            return;
        }

        var journeyCollection = GetFromCacheOrCreate(operationId);
        
        journeyCollection.AddTelemetry(item);

        if (!journeyCollection.RequestFinalized)
        {
            return;
        }

        _telemetryMemoryCache.Remove(operationId);
        
        Send(journeyCollection);
    }

    protected override void Dispose(bool disposing)
    {
        Parallel.ForEach(_telemetryMemoryCache.GetKeys<string>(), key => Send(_telemetryMemoryCache.Get<JourneyCollection>(key)));

        _telemetryMemoryCache.Dispose();
        
        base.Dispose(disposing);
    }

    private void Send(JourneyCollection journeyCollection)
    {
        if (journeyCollection.ShouldSample)
        {
            Parallel.ForEach(journeyCollection.Telemetries, base.Process);
        }
        else
        {
            Parallel.ForEach(journeyCollection.Telemetries, _skipSamplingTelemetryProcessor.Process);
        }
    }

    private JourneyCollection GetFromCacheOrCreate(string? operationId)
    {
        return _telemetryMemoryCache.GetOrCreate(operationId, entry =>
        {
            entry.SlidingExpiration = _smartSamplingOptions.SendTelemetryNotLinkedToRequestsAfter;
            var collection = new JourneyCollection(_smartSamplingOptions);
            entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
            {
                EvictionCallback = (key, value, reason, state) =>
                {
                    if (reason == EvictionReason.Expired)
                    {
                        Send(collection);
                    }
                }
            });
            return collection;
        });
    }
}