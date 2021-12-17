using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class SmartSamplingOptions
{
    public List<Func<DependencyTelemetry, bool>> DependencyNonSamplingRules { get; } = new();

    public List<Func<RequestTelemetry, bool>> RequestNonSamplingRules { get; } = new();
      
    public List<Func<EventTelemetry, bool>> CustomEventNonSamplingRules { get; } = new();
      
    public List<Func<TraceTelemetry, bool>> TraceNonSamplingRules { get; } = new();
      
    public List<Func<ExceptionTelemetry, bool>> ExceptionNonSamplingRules { get; } = new();
    
    public List<Func<PageViewPerformanceTelemetry, bool>> PageViewPerformanceNonSamplingRules { get; } = new();
    
    public List<Func<PageViewTelemetry, bool>> PageViewNonSamplingRules { get; } = new();
    
    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}