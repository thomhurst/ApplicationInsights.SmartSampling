using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public struct JourneyDoNotSampleRule<TTelemetry> where TTelemetry : ITelemetry
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (JourneyDoNotSampleRule<TTelemetry> journeyDoNotSampleRule) => journeyDoNotSampleRule.ConditionToNotSampleJourney;
    
    private JourneyDoNotSampleRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static JourneyDoNotSampleRule<TTelemetry> DoNotSampleJourneyIf(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new JourneyDoNotSampleRule<TTelemetry>(conditionToNotSampleJourney);
    }
}