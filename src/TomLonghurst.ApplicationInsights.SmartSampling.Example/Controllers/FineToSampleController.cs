using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Example.Controllers;

public class FineToSampleController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public FineToSampleController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet(Name = "FineToSample")]
    public IActionResult Get()
    {
        _telemetryClient.TrackEvent($"My {GetType().Name} Event");
        _telemetryClient.TrackTrace($"My {GetType().Name} Trace");
        _telemetryClient.TrackException(new Exception($"My {GetType().Name} Exception"));
        return Ok();
    }
}