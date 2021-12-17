using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Wrappers;

public class DoNotSampleJourneyTelemetry : ITelemetry
{
    public ITelemetry InnerTelemetry { get; }

    public DoNotSampleJourneyTelemetry(ITelemetry innerTelemetry)
    {
        InnerTelemetry = innerTelemetry;
    }

    public void Sanitize()
    {
        InnerTelemetry.Sanitize();
    }

    public ITelemetry DeepClone()
    {
        return InnerTelemetry.DeepClone();
    }

    public void SerializeData(ISerializationWriter serializationWriter)
    {
        InnerTelemetry.SerializeData(serializationWriter);
    }

    public DateTimeOffset Timestamp
    {
        get => InnerTelemetry.Timestamp;
        set => InnerTelemetry.Timestamp = value;
    }

    public TelemetryContext Context => InnerTelemetry.Context;

    public IExtension Extension
    {
        get => InnerTelemetry.Extension;
        set => InnerTelemetry.Extension = value;
    }

    public string Sequence
    {
        get => InnerTelemetry.Sequence;
        set => InnerTelemetry.Sequence = value;
    }
}