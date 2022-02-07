namespace TomLonghurst.ApplicationInsights.SmartSampling.Options;

public class SmartSamplingOptions
{
    public DoNotSampleEntireJourneyRules DoNotSampleEntireJourneyRules { get; } = new();
    
    public DoNotSampleIndividualTelemetryRules DoNotSampleIndividualTelemetryRules { get; } = new();

    public TimeSpan SendTelemetryNotLinkedToRequestsAfter { get; set; } = TimeSpan.FromMinutes(2);
}