using System.Runtime.CompilerServices;
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

#if DEBUG
    private static AdaptiveSamplingPercentageEvaluatedCallback _callback =
        (second, percentage, samplingPercentage, changed, settings) =>
        {
            Console.WriteLine($"Second {second} | Percentage {percentage} | SamplingPercentage {samplingPercentage} | Changed {changed} | Settings {settings}");
        };
#else
    private static AdaptiveSamplingPercentageEvaluatedCallback _callback = (second, percentage, samplingPercentage, changed, settings) => {};
#endif
    
    public SmartSamplingTelemetryProcessor(SmartSamplingOptions smartSamplingOptions, 
        SamplingPercentageEstimatorSettings percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : base(percentageEstimatorSettings, _callback, skipSamplingTelemetryProcessor)
    {
        _smartSamplingOptions = smartSamplingOptions;
        _skipSamplingTelemetryProcessor = skipSamplingTelemetryProcessor;
        _telemetryMemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public new void Process(ITelemetry item)
    {
        var operationId = item.Context?.Operation?.Id;
        
        if (string.IsNullOrEmpty(operationId) || item is MetricTelemetry)
        {
            // Metrics should always be sampled!
            // No operation ID? Then we can't tie it to a journey
            
            if (JourneyTelemetryReferenceContainer.DoNotSampleJourneyTelemetries.TryRemove(item, out _))
            {
                _skipSamplingTelemetryProcessor.Process(item);
                return;
            }
            
            base.Process(item);
            return;
        }

        var journeyCollection = GetFromCacheOrCreate(operationId!);
        
        journeyCollection.AddTelemetry(item);

        if (!journeyCollection.JourneyFinished)
        {
            return;
        }

        _telemetryMemoryCache.Remove(operationId);
        
        Send(journeyCollection);
    }

    private void Send(JourneyCollection journeyCollection)
    {
        var processor = GetProcessor(journeyCollection);
        Parallel.ForEach(journeyCollection.Telemetries, processor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Action<ITelemetry> GetProcessor(JourneyCollection journeyCollection)
    {
        return journeyCollection.ShouldSample
            ? telemetry => base.Process(telemetry)
            : telemetry => _skipSamplingTelemetryProcessor.Process(telemetry);
    }

    private JourneyCollection GetFromCacheOrCreate(string operationId)
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

    public new void Dispose()
    {
        foreach (var key in _telemetryMemoryCache.GetKeys<string>())
        {
            Send(_telemetryMemoryCache.Get<JourneyCollection>(key));
        }

        _telemetryMemoryCache.Dispose();
        
        base.Dispose();
    }
}