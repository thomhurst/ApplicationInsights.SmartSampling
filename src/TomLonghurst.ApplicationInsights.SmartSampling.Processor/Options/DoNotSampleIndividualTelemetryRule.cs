using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Options;

public class DoNotSampleIndividualTelemetryRule<TTelemetry> where TTelemetry : ITelemetry
{
    public Func<TTelemetry, bool> ConditionToNotSampleTelemetry { get; }

    public static implicit operator Func<TTelemetry, bool> (DoNotSampleIndividualTelemetryRule<TTelemetry> journeyRule) => journeyRule.ConditionToNotSampleTelemetry;
    
    private DoNotSampleIndividualTelemetryRule(Func<TTelemetry, bool> conditionToNotSampleTelemetry)
    {
        ConditionToNotSampleTelemetry = conditionToNotSampleTelemetry;
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