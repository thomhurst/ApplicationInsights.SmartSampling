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
            AnyTelemetryTypeDoNotSampleEntireJourneyRules = smartSamplingOptions.AnyTelemetryTypeDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            RequestDoNotSampleEntireJourneyRules = smartSamplingOptions.RequestDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            DependencyDoNotSampleEntireJourneyRules = smartSamplingOptions.DependencyDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            ExceptionDoNotSampleEntireJourneyRules = smartSamplingOptions.ExceptionDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            TraceDoNotSampleEntireJourneyRules = smartSamplingOptions.TraceDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            CustomEventDoNotSampleEntireJourneyRules = smartSamplingOptions.CustomEventDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            PageViewDoNotSampleEntireJourneyRules = smartSamplingOptions.PageViewDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            PageViewPerformanceDoNotSampleEntireJourneyRules = smartSamplingOptions.PageViewPerformanceDoNotSampleEntireJourneyRules.UnwrapToImmutableArray(),
            SendTelemetryNotLinkedToRequestsAfter = smartSamplingOptions.SendTelemetryNotLinkedToRequestsAfter
        };
    }

    private static ImmutableArray<Func<TTelemetry, bool>> UnwrapToImmutableArray<TTelemetry>(this IEnumerable<JourneyDoNotSampleRule<TTelemetry>> journeyDoNotSampleRule) where TTelemetry : ITelemetry
    {
        return journeyDoNotSampleRule.Select(x => x.ConditionToNotSampleJourney).ToImmutableArray();
    }
}