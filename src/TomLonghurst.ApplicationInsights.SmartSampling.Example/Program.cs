using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
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
    RequestDoNotSampleEntireJourneyRules =
    {
        // Keep the whole journey telemetry if: We hit a certain endpoint, or the request is slow, or we hit an internal server error
        JourneyRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Url.AbsolutePath.Contains("DoNotSample")),
        JourneyRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Duration > TimeSpan.FromSeconds(5)),
        JourneyRule<RequestTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.ResponseCode == HttpStatusCode.InternalServerError.ToString() || telemetry.ResponseCode == "500"),
    },
    ExceptionDoNotSampleEntireJourneyRules =
    {
        // If any exception happens, keep the whole journey for analysis
        JourneyRule<ExceptionTelemetry>.DoNotSampleJourneyIf(_ => true)
    },
    CustomEventDoNotSampleEntireJourneyRules =
    {
        // If we log a specific event, we want to be able to investigate this journey. E.g. a potential hacking attempt?
        JourneyRule<EventTelemetry>.DoNotSampleJourneyIf(telemetry => telemetry.Name == "SomeImportantEvent")
    }
}, null,
    new SamplingPercentageEstimatorSettings { InitialSamplingPercentage = 30 });


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

await app.Services.GetRequiredService<TelemetryClient>().FlushAsync(CancellationToken.None);