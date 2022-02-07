using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class DoNotSampleIndividualTelemetryRules
{
    public List<IndividualTelemetryDoNotSampleRule<ITelemetry>> GenericTelemetryRules { get; } = new();
    
    public List<IndividualTelemetryDoNotSampleRule<DependencyTelemetry>> Dependencies { get; } = new();

    public List<IndividualTelemetryDoNotSampleRule<RequestTelemetry>> Requests { get; } = new();
      
    public List<IndividualTelemetryDoNotSampleRule<EventTelemetry>> Events { get; } = new();
      
    public List<IndividualTelemetryDoNotSampleRule<TraceTelemetry>> Traces { get; } = new();
      
    public List<IndividualTelemetryDoNotSampleRule<ExceptionTelemetry>> Exceptions { get; } = new();
    
    public List<IndividualTelemetryDoNotSampleRule<PageViewPerformanceTelemetry>> PageViewPerformance { get; } = new();
    
    public List<IndividualTelemetryDoNotSampleRule<PageViewTelemetry>> PageViews { get; } = new();
}