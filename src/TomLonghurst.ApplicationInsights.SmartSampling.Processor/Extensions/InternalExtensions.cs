using System.Collections.Immutable;
using Microsoft.ApplicationInsights.Channel;
using TomLonghurst.ApplicationInsights.SmartSampling.Processor.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Extensions;

internal static class InternalExtensions
{
    internal static InternalSmartSamplingOptions MapToInternalModel(this SmartSamplingOptions smartSamplingOptions)
    {
        return new InternalSmartSamplingOptions
        {
            AnyTelemetryTypeDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.GenericTelemetryRules.UnwrapToImmutableArray(),
            RequestDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.Requests.UnwrapToImmutableArray(),
            DependencyDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.Dependencies.UnwrapToImmutableArray(),
            ExceptionDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.Exceptions.UnwrapToImmutableArray(),
            TraceDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.Traces.UnwrapToImmutableArray(),
            CustomEventDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.Events.UnwrapToImmutableArray(),
            PageViewDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.PageViews.UnwrapToImmutableArray(),
            PageViewPerformanceDoNotSampleEntireJourneyRules = smartSamplingOptions.DoNotSampleEntireJourneyRules.PageViewPerformance.UnwrapToImmutableArray(),
            
            AnyTelemetryTypeDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.GenericTelemetryRules.UnwrapToImmutableArray(),
            RequestDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.Requests.UnwrapToImmutableArray(),
            DependencyDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.Dependencies.UnwrapToImmutableArray(),
            ExceptionDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.Exceptions.UnwrapToImmutableArray(),
            TraceDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.Traces.UnwrapToImmutableArray(),
            CustomEventDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.Events.UnwrapToImmutableArray(),
            PageViewDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.PageViews.UnwrapToImmutableArray(),
            PageViewPerformanceDoNotSampleIndividualTelemetryRules = smartSamplingOptions.DoNotSampleIndividualTelemetryRules.PageViewPerformance.UnwrapToImmutableArray(),
            
            SendTelemetryNotLinkedToRequestsAfter = smartSamplingOptions.SendTelemetryNotLinkedToRequestsAfter
        };
    }

    private static ImmutableArray<Func<TTelemetry, bool>> UnwrapToImmutableArray<TTelemetry>(this IEnumerable<DoNotSampleJourneyRule<TTelemetry>> doNotSampleJourneyRules) where TTelemetry : ITelemetry
    {
        return doNotSampleJourneyRules.Select(x => x.ConditionToNotSampleJourney).ToImmutableArray();
    }
    
    private static ImmutableArray<Func<TTelemetry, bool>> UnwrapToImmutableArray<TTelemetry>(this IEnumerable<DoNotSampleIndividualTelemetryRule<TTelemetry>> doNotSampleIndividualTelemetryRules) where TTelemetry : ITelemetry
    {
        return doNotSampleIndividualTelemetryRules.Select(x => x.ConditionToNotSampleTelemetry).ToImmutableArray();
    }
}