using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class SmartSamplingOptions
{
    public List<JourneyDoNotSampleRule<ITelemetry>> AnyTelemetryTypeDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<JourneyDoNotSampleRule<DependencyTelemetry>> DependencyDoNotSampleEntireJourneyRules { get; } = new();

    public List<JourneyDoNotSampleRule<RequestTelemetry>> RequestDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<JourneyDoNotSampleRule<EventTelemetry>> CustomEventDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<JourneyDoNotSampleRule<TraceTelemetry>> TraceDoNotSampleEntireJourneyRules { get; } = new();
      
    public List<JourneyDoNotSampleRule<ExceptionTelemetry>> ExceptionDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<JourneyDoNotSampleRule<PageViewPerformanceTelemetry>> PageViewPerformanceDoNotSampleEntireJourneyRules { get; } = new();
    
    public List<JourneyDoNotSampleRule<PageViewTelemetry>> PageViewDoNotSampleEntireJourneyRules { get; } = new();
    
    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}