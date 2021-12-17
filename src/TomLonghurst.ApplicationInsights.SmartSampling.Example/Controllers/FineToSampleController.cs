using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Example.Controllers;

public class FineToSampleController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public FineToSampleController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet("FineToSample")]
    public IActionResult Get()
    {
        _telemetryClient.TrackEvent($"My {GetType().Name} Event");
        
        _telemetryClient.TrackEvent(new EventTelemetry($"My {GetType().Name} Event that I have over-ridden to never sample").DoNotSample());
        _telemetryClient.TrackEvent(new EventTelemetry($"My {GetType().Name} Event that I have over-ridden to sample 90% of the time").SetSamplingPercentage(90));

        _telemetryClient.TrackTrace($"My {GetType().Name} Trace");
        _telemetryClient.TrackException(new Exception($"My {GetType().Name} Exception"));
        return Ok();
    }
}