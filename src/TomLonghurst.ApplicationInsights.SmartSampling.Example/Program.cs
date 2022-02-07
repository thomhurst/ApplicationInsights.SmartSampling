using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using TomLonghurst.ApplicationInsights.SmartSampling.Extensions;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsWithSmartSampling(new SmartSamplingOptions
{
    DoNotSampleEntireJourneyRules =
    {
        Requests =
        {
            // Keep the whole journey telemetry if: We hit a certain endpoint, or the request is slow, or we hit an internal server error
            JourneyDoNotSampleRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Url.AbsolutePath.Contains("DoNotSample")),
            JourneyDoNotSampleRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Duration > TimeSpan.FromSeconds(5)),
            JourneyDoNotSampleRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.ResponseCode == HttpStatusCode.InternalServerError.ToString() || telemetry.ResponseCode == "500"),
        },
        Exceptions =
        {
            // If any exception happens, keep the whole journey for analysis
            JourneyDoNotSampleRule<ExceptionTelemetry>.DoNotSampleJourneyIf(_ => true)
        },
        Events =
        {
            // If we log a specific event, we want to be able to investigate this journey. E.g. a potential hacking attempt?
            JourneyDoNotSampleRule<EventTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Name == "SomeImportantEvent")
        }   
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.StartAsync();

await app.WaitForShutdownAsync();

var telemetryClient = app.Services.GetRequiredService<TelemetryClient>();

telemetryClient.TrackEvent(new EventTelemetry("Customer Logged In").DoNotSample());
telemetryClient.TrackEvent(new EventTelemetry("Hacking Attempt").DoNotSampleJourney());

await telemetryClient.FlushAsync(CancellationToken.None);