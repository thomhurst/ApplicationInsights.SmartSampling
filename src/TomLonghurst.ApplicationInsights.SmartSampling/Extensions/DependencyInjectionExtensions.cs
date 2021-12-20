using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.Extensions.DependencyInjection;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationInsightsWithSmartSampling(this IServiceCollection serviceCollection,
        SmartSamplingOptions smartSamplingOptions,
        ApplicationInsightsServiceOptions? applicationInsightOptions = null,
        SamplingPercentageEstimatorSettings? samplingPercentageEstimatorSettings = null)
    {
        var nonNullApplicationInsightOptions = applicationInsightOptions ?? new ApplicationInsightsServiceOptions();
        nonNullApplicationInsightOptions.EnableAdaptiveSampling = false;
        
        serviceCollection.AddApplicationInsightsTelemetry(nonNullApplicationInsightOptions);
        
        serviceCollection.AddSingleton(samplingPercentageEstimatorSettings ?? new SamplingPercentageEstimatorSettings());
        serviceCollection.AddSingleton(smartSamplingOptions.MapToInternalModel());
        
        serviceCollection.AddApplicationInsightsTelemetryProcessor<SmartSamplingTelemetryProcessor>();

        return serviceCollection;
    }
}