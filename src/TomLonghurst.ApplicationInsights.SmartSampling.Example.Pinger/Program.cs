// See https://aka.ms/new-console-template for more information

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

var tasks = new List<Task>();
for (var i = 0; i < 1000; i++)
{
    tasks.AddRange(urls.Select(httpClient.GetAsync));
}

await Task.WhenAll(tasks);

Console.WriteLine("Complete! Check Application Insights!");