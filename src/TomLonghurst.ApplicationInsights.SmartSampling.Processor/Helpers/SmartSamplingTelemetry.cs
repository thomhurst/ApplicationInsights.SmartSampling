using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using TomLonghurst.ApplicationInsights.SmartSampling.Processor.Extensions;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Processor.Helpers;

public static class SmartSamplingTelemetry
{
    public static TTelemetry NonSampledTelemetry<TTelemetry>(TTelemetry telemetry) where TTelemetry : ITelemetry, ISupportSampling
    {
        telemetry.DoNotSample();
        return telemetry;
    }

    public static TTelemetry CustomSamplingTelemetry<TTelemetry>(TTelemetry telemetry, double samplingPercentage) where TTelemetry : ITelemetry, ISupportSampling
    {
        telemetry.SetSamplingPercentage(samplingPercentage);
        return telemetry;
    }
}