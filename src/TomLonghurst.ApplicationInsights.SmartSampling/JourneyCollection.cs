using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

internal class JourneyCollection
{
    public bool RequestFinalized { get; private set; }
    
    public bool ShouldSample { get; private set; } = true;
    
    private readonly SmartSamplingOptions _smartSamplingOptions;

    private List<ITelemetry> _telemetries = new();

    public JourneyCollection(SmartSamplingOptions smartSamplingOptions)
    {
        _smartSamplingOptions = smartSamplingOptions;
    }
    
    public void AddTelemetry(ITelemetry telemetry)
    {
        _telemetries.Add(telemetry);

        CheckSampleRules(telemetry);

        RequestFinalized = telemetry is RequestTelemetry;
    }

    public void Send(ITelemetryProcessor telemetryProcessor)
    {
        var telemetries = Interlocked.Exchange(ref _telemetries, new List<ITelemetry>());
        Parallel.ForEach(telemetries, telemetryProcessor.Process);
    }

    private void CheckSampleRules(ITelemetry telemetry)
    {
        if (!ShouldSample)
        {
            return;
        }
        
        switch (telemetry)
        {
            case DependencyTelemetry dependencyTelemetry:
                SetFlagBasedOnRules(dependencyTelemetry, _smartSamplingOptions.DependencyNonSamplingRules);
                break;
            case EventTelemetry eventTelemetry:
                SetFlagBasedOnRules(eventTelemetry, _smartSamplingOptions.CustomEventNonSamplingRules);
                break;
            case ExceptionTelemetry exceptionTelemetry:
                SetFlagBasedOnRules(exceptionTelemetry, _smartSamplingOptions.ExceptionNonSamplingRules);
                break;
            case PageViewPerformanceTelemetry pageViewPerformanceTelemetry:
                SetFlagBasedOnRules(pageViewPerformanceTelemetry, _smartSamplingOptions.PageViewPerformanceNonSamplingRules);
                break;
            case PageViewTelemetry pageViewTelemetry:
                SetFlagBasedOnRules(pageViewTelemetry, _smartSamplingOptions.PageViewNonSamplingRules);
                break;
            case RequestTelemetry requestTelemetry:
                SetFlagBasedOnRules(requestTelemetry, _smartSamplingOptions.RequestNonSamplingRules);
                break;
            case TraceTelemetry traceTelemetry:
                SetFlagBasedOnRules(traceTelemetry, _smartSamplingOptions.TraceNonSamplingRules);
                break;
        }
    }

    private void SetFlagBasedOnRules<TTelemetry>(TTelemetry telemetry, IReadOnlyList<Func<TTelemetry, bool>> rulesForTelemetryType) where TTelemetry : ITelemetry 
    {
        if (rulesForTelemetryType.Any(rule => rule(telemetry)))
        {
            ShouldSample = false;
        }
    }
}