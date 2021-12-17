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
    public IActionResult Get()
    {
        _telemetryClient.TrackEvent($"My {GetType().Name} Event");
        _telemetryClient.TrackTrace($"My {GetType().Name} Trace");
        _telemetryClient.TrackException(new Exception($"My {GetType().Name} Exception"));
        return Ok();
    }
    
    [HttpGet("DoesNotSampleBecauseOfCustomTelemetryType")]
    public IActionResult GetWithDoNotSampleJourneyTelemetryWrapper()
    {
        _telemetryClient.TrackEvent($"My {GetType().Name} Event that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item");
        _telemetryClient.TrackTrace($"My {GetType().Name} Trace that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item");
        _telemetryClient.TrackException(new Exception($"My {GetType().Name} Exception that is in the same context of a {nameof(TelemetryExtensions.DoNotSampleJourney)} telemetry item"));
        
        _telemetryClient.TrackEvent(new EventTelemetry($"My {GetType().Name} Event that I have called {nameof(TelemetryExtensions.DoNotSampleJourney)} on").DoNotSampleJourney());

        return Ok();
    }
}