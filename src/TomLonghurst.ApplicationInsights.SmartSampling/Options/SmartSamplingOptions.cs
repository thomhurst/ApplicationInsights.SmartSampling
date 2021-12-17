using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class SmartSamplingOptions
{
    public List<Func<ITelemetry, bool>> AnyTelemetryTypeDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<Func<DependencyTelemetry, bool>> DependencyDoNotSampleEntireJourneyRules { get; } = new();

    public List<Func<RequestTelemetry, bool>> RequestDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<Func<EventTelemetry, bool>> CustomEventDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<Func<TraceTelemetry, bool>> TraceDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<Func<ExceptionTelemetry, bool>> ExceptionDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<Func<PageViewPerformanceTelemetry, bool>> PageViewPerformanceDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<Func<PageViewTelemetry, bool>> PageViewDoNotSampleEntireJourneyRules { get; } = new();
    
    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}