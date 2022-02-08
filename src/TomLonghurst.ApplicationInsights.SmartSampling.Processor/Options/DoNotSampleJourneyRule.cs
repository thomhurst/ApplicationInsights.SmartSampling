using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Options;

public class DoNotSampleJourneyRule<TTelemetry> where TTelemetry : ITelemetry
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (DoNotSampleJourneyRule<TTelemetry> doNotSampleJourneyRule) => doNotSampleJourneyRule.ConditionToNotSampleJourney;
    
    private DoNotSampleJourneyRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static DoNotSampleJourneyRule<TTelemetry> DoNotSampleJourneyIf(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new DoNotSampleJourneyRule<TTelemetry>(conditionToNotSampleJourney);
    }
    
    public static DoNotSampleJourneyRule<TTelemetry> DoNotSampleJourneyIf(string description, Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new DoNotSampleJourneyRule<TTelemetry>(conditionToNotSampleJourney);
    }
}