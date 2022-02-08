using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Options;

public class DoNotSampleEntireJourneyRules
{
    public List<DoNotSampleJourneyRule<ITelemetry>> GenericTelemetryRules { get; } = new();
    
    public List<DoNotSampleJourneyRule<DependencyTelemetry>> Dependencies { get; } = new();

    public List<DoNotSampleJourneyRule<RequestTelemetry>> Requests { get; } = new();
      
    public List<DoNotSampleJourneyRule<EventTelemetry>> Events { get; } = new();
      
    public List<DoNotSampleJourneyRule<TraceTelemetry>> Traces { get; } = new();
      
    public List<DoNotSampleJourneyRule<ExceptionTelemetry>> Exceptions { get; } = new();
    
    public List<DoNotSampleJourneyRule<PageViewPerformanceTelemetry>> PageViewPerformance { get; } = new();
    
    public List<DoNotSampleJourneyRule<PageViewTelemetry>> PageViews { get; } = new();
}