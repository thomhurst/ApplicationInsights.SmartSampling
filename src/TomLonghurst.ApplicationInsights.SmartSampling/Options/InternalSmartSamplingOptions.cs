using System.Collections.Immutable;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

internal class InternalSmartSamplingOptions
{
    #region Journey Rules

    public ImmutableArray<Func<ITelemetry, bool>> AnyTelemetryTypeDoNotSampleEntireJourneyRules { get; set; }
    
    public ImmutableArray<Func<DependencyTelemetry, bool>> DependencyDoNotSampleEntireJourneyRules { get; set; }

    public ImmutableArray<Func<RequestTelemetry, bool>> RequestDoNotSampleEntireJourneyRules { get; set; }
      
    public ImmutableArray<Func<EventTelemetry, bool>> CustomEventDoNotSampleEntireJourneyRules { get; set; }
      
    public ImmutableArray<Func<TraceTelemetry, bool>> TraceDoNotSampleEntireJourneyRules { get; set; }
      
    public ImmutableArray<Func<ExceptionTelemetry, bool>> ExceptionDoNotSampleEntireJourneyRules { get; set; }
    
    public ImmutableArray<Func<PageViewPerformanceTelemetry, bool>> PageViewPerformanceDoNotSampleEntireJourneyRules { get; set; }
    
    public ImmutableArray<Func<PageViewTelemetry, bool>> PageViewDoNotSampleEntireJourneyRules { get; set; }

    #endregion

    #region Individual Telemetry Rules

    public ImmutableArray<Func<ITelemetry, bool>> AnyTelemetryTypeDoNotSampleIndividualTelemetryRules { get; set; }
    
    public ImmutableArray<Func<DependencyTelemetry, bool>> DependencyDoNotSampleIndividualTelemetryRules { get; set; }

    public ImmutableArray<Func<RequestTelemetry, bool>> RequestDoNotSampleIndividualTelemetryRules { get; set; }
      
    public ImmutableArray<Func<EventTelemetry, bool>> CustomEventDoNotSampleIndividualTelemetryRules { get; set; }
      
    public ImmutableArray<Func<TraceTelemetry, bool>> TraceDoNotSampleIndividualTelemetryRules { get; set; }
      
    public ImmutableArray<Func<ExceptionTelemetry, bool>> ExceptionDoNotSampleIndividualTelemetryRules { get; set; }
    
    public ImmutableArray<Func<PageViewPerformanceTelemetry, bool>> PageViewPerformanceDoNotSampleIndividualTelemetryRules { get; set; }
    
    public ImmutableArray<Func<PageViewTelemetry, bool>> PageViewDoNotSampleIndividualTelemetryRules { get; set; }

    #endregion
    
    
    
    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}