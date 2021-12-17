using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public struct JourneyNonSamplingRule<TTelemetry> where TTelemetry : ITelemetry, ISupportSampling
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (JourneyNonSamplingRule<TTelemetry> journeyNonSamplingRule) => journeyNonSamplingRule.ConditionToNotSampleJourney;
    
    private JourneyNonSamplingRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static JourneyNonSamplingRule<TTelemetry> Of(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new JourneyNonSamplingRule<TTelemetry>(conditionToNotSampleJourney);
    }
}