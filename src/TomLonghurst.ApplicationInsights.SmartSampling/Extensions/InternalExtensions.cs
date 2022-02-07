using System.Collections.Immutable;
using Microsoft.ApplicationInsights.Channel;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

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
            SendTelemetryNotLinkedToRequestsAfter = smartSamplingOptions.SendTelemetryNotLinkedToRequestsAfter
        };
    }

    private static ImmutableArray<Func<TTelemetry, bool>> UnwrapToImmutableArray<TTelemetry>(this IEnumerable<JourneyDoNotSampleRule<TTelemetry>> journeyDoNotSampleRule) where TTelemetry : ITelemetry
    {
        return journeyDoNotSampleRule.Select(x => x.ConditionToNotSampleJourney).ToImmutableArray();
    }
}