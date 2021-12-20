﻿using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

public static class TelemetryExtensions
{
    public static TSamplingTelemetry SetSamplingPercentage<TSamplingTelemetry>(this TSamplingTelemetry telemetry, double percentage) where TSamplingTelemetry : ISupportSampling, ITelemetry
    {
        telemetry.SamplingPercentage = percentage;
        return telemetry;
    }

    public static TSamplingTelemetry DoNotSample<TSamplingTelemetry>(this TSamplingTelemetry telemetry) where TSamplingTelemetry : ISupportSampling, ITelemetry
    {
        telemetry.SamplingPercentage = 0;
        return telemetry;
    }
    
    public static TSamplingTelemetry DoNotSampleIf<TSamplingTelemetry>(this TSamplingTelemetry telemetry, Func<TSamplingTelemetry, bool> condition) where TSamplingTelemetry : ISupportSampling, ITelemetry
    {
        if (condition(telemetry))
        {
            telemetry.DoNotSample();
        }
        
        return telemetry;
    }

    public static TSamplingTelemetry DoNotSampleJourney<TSamplingTelemetry>(this TSamplingTelemetry telemetry) where TSamplingTelemetry : ITelemetry
    {
        JourneyTelemetryReferenceContainer.DoNotSampleJourneyTelemetries.Add(telemetry, default);
        return telemetry;
    }
    
    public static TSamplingTelemetry DoNotSampleJourneyIf<TSamplingTelemetry>(this TSamplingTelemetry telemetry, Func<TSamplingTelemetry, bool> condition) where TSamplingTelemetry : ITelemetry
    {
        if (condition(telemetry))
        {
            telemetry.DoNotSampleJourney();
        }
        
        return telemetry;
    }
}