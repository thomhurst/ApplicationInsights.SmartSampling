// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

var httpClient = new HttpClient();

var endpoints = new List<string>
{
    "DoNotSample",
    "DoesNotSampleBecauseOfCustomTelemetryType",
    "FineToSample"
};

var urls = endpoints.Select(x => new UriBuilder("https://localhost:7237")
{
    Path = x
}.Uri).ToList();

var stopWatch = Stopwatch.StartNew();

var start = DateTime.UtcNow;

while (DateTime.UtcNow - start < TimeSpan.FromSeconds(30))
{
    var tasks = new List<Task>();
    for (var i = 0; i < 1000; i++)
    {
        tasks.AddRange(urls.Select(httpClient.GetAsync));
    }

    await Task.WhenAll(tasks);
}

Console.WriteLine($"Complete after {stopWatch.Elapsed.TotalSeconds} seconds! Check Application Insights!");