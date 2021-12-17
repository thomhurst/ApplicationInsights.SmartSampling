using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public struct JourneyRule<TTelemetry> where TTelemetry : ITelemetry, ISupportSampling
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (JourneyRule<TTelemetry> journeyRule) => journeyRule.ConditionToNotSampleJourney;
    
    private JourneyRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static JourneyRule<TTelemetry> DoNotSampleJourneyIf(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new JourneyRule<TTelemetry>(conditionToNotSampleJourney);
    }
}