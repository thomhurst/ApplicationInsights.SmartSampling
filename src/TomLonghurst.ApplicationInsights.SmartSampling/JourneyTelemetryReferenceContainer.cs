using System.Collections.Concurrent;
using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

internal static class JourneyTelemetryReferenceContainer
{
    internal static readonly ConcurrentDictionary<ITelemetry, byte> DoNotSampleJourneyTelemetries = new();
}