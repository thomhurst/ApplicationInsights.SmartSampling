using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using TomLonghurst.ApplicationInsights.SmartSampling.Wrappers;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

public static class TelemetryExtensions
{
    public static TSamplingTelemetry DoNotSample<TSamplingTelemetry>(this TSamplingTelemetry telemetry) where TSamplingTelemetry : ISupportSampling, ITelemetry
    {
        telemetry.SamplingPercentage = 0;
        return telemetry;
    }

    public static TSamplingTelemetry SetSamplingPercentage<TSamplingTelemetry>(this TSamplingTelemetry telemetry, double percentage) where TSamplingTelemetry : ISupportSampling, ITelemetry
    {
        telemetry.SamplingPercentage = percentage;
        return telemetry;
    }
    
    public static DoNotSampleJourneyTelemetry ToDoNotSampleJourneyTelemetry<TSamplingTelemetry>(this TSamplingTelemetry telemetry) where TSamplingTelemetry : ITelemetry
    {
        return new DoNotSampleJourneyTelemetry(telemetry);
    }
}