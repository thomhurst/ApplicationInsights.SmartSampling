using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

internal static class JourneyTelemetryReferenceContainer
{
    internal static readonly HashSet<ITelemetry> DoNotSampleJourneyTelemetries = new();
}