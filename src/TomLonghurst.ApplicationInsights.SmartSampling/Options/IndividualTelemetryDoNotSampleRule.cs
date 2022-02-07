using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class IndividualTelemetryDoNotSampleRule<TTelemetry> where TTelemetry : ITelemetry
{
    public Func<TTelemetry, bool> ConditionToNotSampleJourney { get; }

    public static implicit operator Func<TTelemetry, bool> (IndividualTelemetryDoNotSampleRule<TTelemetry> journeyDoNotSampleRule) => journeyDoNotSampleRule.ConditionToNotSampleJourney;
    
    private IndividualTelemetryDoNotSampleRule(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        ConditionToNotSampleJourney = conditionToNotSampleJourney;
    }

    public static IndividualTelemetryDoNotSampleRule<TTelemetry> DoNotSampleTelemetryIf(Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new IndividualTelemetryDoNotSampleRule<TTelemetry>(conditionToNotSampleJourney);
    }
    
    public static IndividualTelemetryDoNotSampleRule<TTelemetry> DoNotSampleTelemetryIf(string description, Func<TTelemetry, bool> conditionToNotSampleJourney)
    {
        return new IndividualTelemetryDoNotSampleRule<TTelemetry>(conditionToNotSampleJourney);
    }
}