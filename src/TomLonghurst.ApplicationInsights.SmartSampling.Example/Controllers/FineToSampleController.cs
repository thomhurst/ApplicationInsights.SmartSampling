using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;
using TomLonghurst.ApplicationInsights.SmartSampling.Processor.Extensions;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Example.Controllers;

public class FineToSampleController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public FineToSampleController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet("FineToSample")]
    public IActionResult FineToSample()
    {
        _telemetryClient.TrackEvent($"My {MethodBase.GetCurrentMethod().Name} Event");
        
        _telemetryClient.TrackEvent(new EventTelemetry($"My {MethodBase.GetCurrentMethod().Name} Event that I have over-ridden to never sample").DoNotSample());
        _telemetryClient.TrackEvent(new EventTelemetry($"My {MethodBase.GetCurrentMethod().Name} Event that I have over-ridden to sample 20% of the time").SetSamplingPercentage(20));

        _telemetryClient.TrackTrace($"My {MethodBase.GetCurrentMethod().Name} Trace");
        return Ok();
    }
}