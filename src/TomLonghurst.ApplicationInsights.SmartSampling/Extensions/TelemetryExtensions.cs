using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

public static class TelemetryExtensions
{
    public static void DoNotSample(this ISupportAdvancedSampling telemetry)
    {
        telemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;
    }
    
    public static void DoNotSample(this ISupportSampling telemetry)
    {
        telemetry.SamplingPercentage = 0;
    }

    public static void SetSamplingPercentage(this ISupportSampling telemetry, double percentage)
    {
        telemetry.SamplingPercentage = percentage;
    }
}