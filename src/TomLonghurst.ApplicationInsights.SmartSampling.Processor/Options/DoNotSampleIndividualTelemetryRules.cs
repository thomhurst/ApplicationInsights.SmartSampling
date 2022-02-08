using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Options;

public class DoNotSampleIndividualTelemetryRules
{
    public List<DoNotSampleIndividualTelemetryRule<ITelemetry>> GenericTelemetryRules { get; } = new();
    
    public List<DoNotSampleIndividualTelemetryRule<DependencyTelemetry>> Dependencies { get; } = new();

    public List<DoNotSampleIndividualTelemetryRule<RequestTelemetry>> Requests { get; } = new();
      
    public List<DoNotSampleIndividualTelemetryRule<EventTelemetry>> Events { get; } = new();
      
    public List<DoNotSampleIndividualTelemetryRule<TraceTelemetry>> Traces { get; } = new();
      
    public List<DoNotSampleIndividualTelemetryRule<ExceptionTelemetry>> Exceptions { get; } = new();
    
    public List<DoNotSampleIndividualTelemetryRule<PageViewPerformanceTelemetry>> PageViewPerformance { get; } = new();
    
    public List<DoNotSampleIndividualTelemetryRule<PageViewTelemetry>> PageViews { get; } = new();
}