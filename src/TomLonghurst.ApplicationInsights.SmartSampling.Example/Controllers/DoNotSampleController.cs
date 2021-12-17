using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Example.Controllers;

public class DoNotSampleController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public DoNotSampleController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet(Name = "DoNotSample")]
    public IActionResult Get()
    {
        _telemetryClient.TrackEvent($"My {GetType().Name} Event");
        _telemetryClient.TrackTrace($"My {GetType().Name} Trace");
        _telemetryClient.TrackException(new Exception($"My {GetType().Name} Exception"));
        return Ok();
    }
}