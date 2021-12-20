using System.Collections.Immutable;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

internal class InternalSmartSamplingOptions
{
    public ImmutableArray<Func<ITelemetry, bool>> AnyTelemetryTypeDoNotSampleEntireJourneyRules { get; set; } = new();
    
    public ImmutableArray<Func<DependencyTelemetry, bool>> DependencyDoNotSampleEntireJourneyRules { get; set; } = new();

    public ImmutableArray<Func<RequestTelemetry, bool>> RequestDoNotSampleEntireJourneyRules { get; set; } = new();
      
    public ImmutableArray<Func<EventTelemetry, bool>> CustomEventDoNotSampleEntireJourneyRules { get; set; } = new();
      
    public ImmutableArray<Func<TraceTelemetry, bool>> TraceDoNotSampleEntireJourneyRules { get; set; } = new();
      
    public ImmutableArray<Func<ExceptionTelemetry, bool>> ExceptionDoNotSampleEntireJourneyRules { get; set; } = new();
    
    public ImmutableArray<Func<PageViewPerformanceTelemetry, bool>> PageViewPerformanceDoNotSampleEntireJourneyRules { get; set; } = new();
    
    public ImmutableArray<Func<PageViewTelemetry, bool>> PageViewDoNotSampleEntireJourneyRules { get; set; } = new();
    
    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}