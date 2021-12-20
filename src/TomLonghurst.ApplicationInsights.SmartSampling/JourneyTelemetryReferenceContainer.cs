using System.Runtime.CompilerServices;
using Microsoft.ApplicationInsights.Channel;

namespace TomLonghurst.ApplicationInsights.SmartSampling;

internal static class JourneyTelemetryReferenceContainer
{
    internal static readonly ConditionalWeakTable<ITelemetry, string> DoNotSampleJourneyTelemetries = new();
}