using System.Runtime.CompilerServices;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

public class SmartSamplingTelemetryProcessor : AdaptiveSamplingTelemetryProcessor, ITelemetryProcessor, IDisposable
{
    private readonly InternalSmartSamplingOptions _smartSamplingOptions;
    private readonly ITelemetryProcessor _skipSamplingTelemetryProcessor;
    private readonly MemoryCache _telemetryMemoryCache;

    public SmartSamplingTelemetryProcessor(IOptions<SmartSamplingOptions> smartSamplingOptions,
        SamplingPercentageEstimatorSettings percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : this(smartSamplingOptions.Value.MapToInternalModel(),
        percentageEstimatorSettings, skipSamplingTelemetryProcessor)
    {
    }
    
    public SmartSamplingTelemetryProcessor(IOptions<SmartSamplingOptions> smartSamplingOptions,
        IOptions<SamplingPercentageEstimatorSettings> percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : this(smartSamplingOptions.Value.MapToInternalModel(),
        percentageEstimatorSettings.Value, skipSamplingTelemetryProcessor)
    {
    }
    
    [ActivatorUtilitiesConstructor]
    public SmartSamplingTelemetryProcessor(SmartSamplingOptions smartSamplingOptions,
        SamplingPercentageEstimatorSettings percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : this(smartSamplingOptions.MapToInternalModel(),
        percentageEstimatorSettings, skipSamplingTelemetryProcessor)
    {
    }
    
    internal SmartSamplingTelemetryProcessor(InternalSmartSamplingOptions smartSamplingOptions, 
        SamplingPercentageEstimatorSettings percentageEstimatorSettings,
        ITelemetryProcessor skipSamplingTelemetryProcessor) : base(percentageEstimatorSettings, default, skipSamplingTelemetryProcessor)
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

        if (HasIndividualDoNotSampleRule(item))
        {
            _skipSamplingTelemetryProcessor.Process(item);
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

    private bool HasIndividualDoNotSampleRule(ITelemetry item)
    {
        if (_smartSamplingOptions.AnyTelemetryTypeDoNotSampleIndividualTelemetryRules.Any(x => x(item)))
        {
            return true;
        }

        return item switch
        {
            DependencyTelemetry dependencyTelemetry => _smartSamplingOptions.DependencyDoNotSampleIndividualTelemetryRules.Any(x => x(dependencyTelemetry)),
            EventTelemetry eventTelemetry => _smartSamplingOptions.CustomEventDoNotSampleIndividualTelemetryRules.Any(x => x(eventTelemetry)),
            ExceptionTelemetry exceptionTelemetry => _smartSamplingOptions.ExceptionDoNotSampleIndividualTelemetryRules.Any(x => x(exceptionTelemetry)),
            PageViewPerformanceTelemetry pageViewPerformanceTelemetry => _smartSamplingOptions.PageViewPerformanceDoNotSampleIndividualTelemetryRules.Any(x => x(pageViewPerformanceTelemetry)),
            PageViewTelemetry pageViewTelemetry => _smartSamplingOptions.PageViewDoNotSampleIndividualTelemetryRules.Any(x => x(pageViewTelemetry)),
            RequestTelemetry requestTelemetry => _smartSamplingOptions.RequestDoNotSampleIndividualTelemetryRules.Any(x => x(requestTelemetry)),
            TraceTelemetry traceTelemetry => _smartSamplingOptions.TraceDoNotSampleIndividualTelemetryRules.Any(x => x(traceTelemetry)),
            _ => false
        };
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