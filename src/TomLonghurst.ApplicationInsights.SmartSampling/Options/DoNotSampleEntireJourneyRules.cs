using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class DoNotSampleEntireJourneyRules
{
    public List<JourneyDoNotSampleRule<ITelemetry>> GenericTelemetryRules { get; } = new();
    
    public List<JourneyDoNotSampleRule<DependencyTelemetry>> Dependencies { get; } = new();

    public List<JourneyDoNotSampleRule<RequestTelemetry>> Requests { get; } = new();
      
    public List<JourneyDoNotSampleRule<EventTelemetry>> Events { get; } = new();
      
    public List<JourneyDoNotSampleRule<TraceTelemetry>> Traces { get; } = new();
      
    public List<JourneyDoNotSampleRule<ExceptionTelemetry>> Exceptions { get; } = new();
    
    public List<JourneyDoNotSampleRule<PageViewPerformanceTelemetry>> PageViewPerformance { get; } = new();
    
    public List<JourneyDoNotSampleRule<PageViewTelemetry>> PageViews { get; } = new();
}