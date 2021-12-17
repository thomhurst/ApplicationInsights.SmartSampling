using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Helpers;

public class SmartSampling<TTelemetry> where TTelemetry : ITelemetry, ISupportSampling
{
    public static TTelemetry NonSampledTelemetry(TTelemetry telemetry)
    {
        telemetry.DoNotSample();
        return telemetry;
    }

    public static TTelemetry CustomSamplingTelemetry(TTelemetry telemetry, double samplingPercentage)
    {
        telemetry.SetSamplingPercentage(samplingPercentage);
        return telemetry;
    }
}