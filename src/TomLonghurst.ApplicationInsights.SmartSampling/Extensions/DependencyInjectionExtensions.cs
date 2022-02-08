using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.Extensions.DependencyInjection;
using TomLonghurst.ApplicationInsights.SmartSampling.Options;

namespace TomLonghurst.ApplicationInsights.SmartSampling.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationInsightsWithSmartSampling(this IServiceCollection serviceCollection,
        SmartSamplingOptions smartSamplingOptions)
    {
        return AddApplicationInsightsWithSmartSampling(serviceCollection, smartSamplingOptions, null, null);
    }
    
    public static IServiceCollection AddApplicationInsightsWithSmartSampling(this IServiceCollection serviceCollection,
        SmartSamplingOptions smartSamplingOptions,
        SamplingPercentageEstimatorSettings? samplingPercentageEstimatorSettings)
    {
        return AddApplicationInsightsWithSmartSampling(serviceCollection, smartSamplingOptions, null, samplingPercentageEstimatorSettings);
    }

    public static IServiceCollection AddApplicationInsightsWithSmartSampling(this IServiceCollection serviceCollection,
        SmartSamplingOptions smartSamplingOptions,
        ApplicationInsightsServiceOptions? applicationInsightOptions)
    {
        return AddApplicationInsightsWithSmartSampling(serviceCollection, smartSamplingOptions, applicationInsightOptions, null);
    }

    public static IServiceCollection AddApplicationInsightsWithSmartSampling(this IServiceCollection serviceCollection,
        SmartSamplingOptions smartSamplingOptions,
        ApplicationInsightsServiceOptions? applicationInsightOptions,
        SamplingPercentageEstimatorSettings? samplingPercentageEstimatorSettings)
    {
        var nonNullApplicationInsightOptions = applicationInsightOptions ?? new ApplicationInsightsServiceOptions();
        nonNullApplicationInsightOptions.EnableAdaptiveSampling = false;
        
        serviceCollection.AddApplicationInsightsTelemetry(nonNullApplicationInsightOptions);
        
        serviceCollection.AddSingleton(samplingPercentageEstimatorSettings ?? new SamplingPercentageEstimatorSettings());
        serviceCollection.AddSingleton(smartSamplingOptions);
        
        serviceCollection.AddApplicationInsightsTelemetryProcessor<SmartSamplingTelemetryProcessor>();

        return serviceCollection;
    }
}