# ApplicationInsights.SmartSampling
Fine tune your sampling rules

[![nuget](https://img.shields.io/nuget/v/TomLonghurst.ApplicationInsights.SmartSampling.svg)](https://www.nuget.org/packages/TomLonghurst.ApplicationInsights.SmartSampling/)
[![CodeFactor](https://www.codefactor.io/repository/github/thomhurst/applicationinsights.smartsampling/badge)](https://www.codefactor.io/repository/github/thomhurst/applicationinsights.smartsampling)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/e7c443453ee34bf2bda3ca7b370c19a0)](https://www.codacy.com/gh/thomhurst/ApplicationInsights.SmartSampling/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=thomhurst/ApplicationInsights.SmartSampling&amp;utm_campaign=Badge_Grade)

## Installation
Install via Nuget
`Install-Package TomLonghurst.ApplicationInsights.SmartSampling`

In your `Startup` class, replace `IServiceCollection.AddApplicationInsights();` with `IServiceCollection.AddApplicationInsightsWithSmartSampling(SmartSamplingOptions)`

## Behaviour
This library will sample telemetry by default. You then provide it rules to exclude certain telemetry from sampling.
You can exclude:
- Specific individual telemetry items
- Entire journeys based on a certain telemetry item being raised during that journey

These rules are provided in the `SmartSamplingOptions` object which is passed into the `AddApplicationInsightsWithSmartSampling` call in startup.

There are two sections:
- `DoNotSampleEntireJourneyRules` - If a rule is hit, any telemetry in that whole journey will not be sampled.
- `DoNotSampleIndividualTelemetryRules` - If a rule is hit, just that telemetry will not be sampled. Other telemetry in that journey may still be sampled.

Each of these sections can have rules configured for specific telemetry types. This includes:
- `Dependencies`
- `Requests`
- `Events`
- `Traces`
- `Exceptions`
- `PageViewPerformance`
- `PageViews`
- `GenericTelemetryRules` - This is a catch all. It can contain any of the other telemetry types.

The rules are a `Func<TTelemetry, bool>` - So you just provide it a condition, and if the return is `true`, your rule is met.

Here is an example of a startup with some rules configured:

```csharp
builder.Services.AddApplicationInsightsWithSmartSampling(new SmartSamplingOptions
{
    DoNotSampleEntireJourneyRules =
    {
        Requests =
        {
            // Here any telemetry during this entire journey (which shares the same operation ID) would not be sampled if the Request took longer than 5 seconds, or if the Response was an Internal Server Error. This means we can inspect all telemetry for diagnosing problems, but any healthy requests will still be sampled and not consume lots of data, since we don't need to diagnose any problems for those.
            DoNotSampleJourneyRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Duration > TimeSpan.FromSeconds(5)),
            DoNotSampleJourneyRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.ResponseCode == HttpStatusCode.InternalServerError.ToString() || telemetry.ResponseCode == "500"),
        },
        Exceptions =
        {
            // If any exception happens, keep the whole journey for analysis
            DoNotSampleJourneyRule<ExceptionTelemetry>.DoNotSampleJourneyIf(telemetry => true)
        },
        Events =
        {
            // If we log a specific event, we might want to be able to investigate this journey. E.g. a potential hacking attempt?
            DoNotSampleJourneyRule<EventTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Name == "SomeImportantEvent")
        }   
    },
    DoNotSampleIndividualTelemetryRules =
    {
        Events =
        {
            // This means that only this specific Telemetry item won't be sampled. Other telemetry items during this request may still be sampled. If you wanted to not sample the journey, you should move this rule up into the `DoNotSampleEntireJourneyRules` section
            DoNotSampleIndividualTelemetryRule<EventTelemetry>.DoNotSampleTelemetryIf(telemetry => telemetry.Name == "LoginAttempt")
        }
    }
});
```
