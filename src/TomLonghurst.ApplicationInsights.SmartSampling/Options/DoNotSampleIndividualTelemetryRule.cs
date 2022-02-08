using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class DoNotSampleIndividualTelemetryRule<TTelemetry> where TTelemetry : ITelemetry
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (DoNotSampleIndividualTelemetryRule<TTelemetry> journeyRule) => journeyRule.ConditionToNotSampleJourney;
    
    private DoNotSampleIndividualTelemetryRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static DoNotSampleIndividualTelemetryRule<TTelemetry> DoNotSampleTelemetryIf(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new DoNotSampleIndividualTelemetryRule<TTelemetry>(conditionToNotSampleJourney);
    }
    
    public static DoNotSampleIndividualTelemetryRule<TTelemetry> DoNotSampleTelemetryIf(string description, Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new DoNotSampleIndividualTelemetryRule<TTelemetry>(conditionToNotSampleJourney);
    }
}