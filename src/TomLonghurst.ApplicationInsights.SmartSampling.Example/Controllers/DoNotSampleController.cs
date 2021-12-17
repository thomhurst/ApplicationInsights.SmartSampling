using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Example.Controllers;

public class DoNotSampleController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;

    public DoNotSampleController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [HttpGet("DoNotSample")]
    public IActionResult DoNotSample()
    {
        _telemetryClient.TrackEvent($"My {MethodBase.GetCurrentMethod().Name} Event");
        _telemetryClient.TrackTrace($"My {MethodBase.GetCurrentMethod().Name} Trace");
        _telemetryClient.TrackException(new Exception($"My {MethodBase.GetCurrentMethod().Name} Exception"));
        return Ok();
    }
    
    [HttpGet("DoesNotSampleBecauseOfCustomTelemetryType")]
    public IActionResult DoesNotSampleBecauseOfCustomTelemetryType()
    {
        _telemetryClient.TrackEvent($"My {MethodBase.GetCurrentMethod().Name} Event that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item");
        _telemetryClient.TrackTrace($"My {MethodBase.GetCurrentMethod().Name} Trace that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item");
        _telemetryClient.TrackException(new Exception($"My {MethodBase.GetCurrentMethod().Name} Exception that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item"));
        
        _telemetryClient.TrackEvent(new EventTelemetry($"My {MethodBase.GetCurrentMethod().Name} Event that I have called {nameof(TelemetryExtensions.DoNotSampleJourney)} on").DoNotSampleJourney());

        return Ok();
    }
}