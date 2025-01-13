using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Serialization;
using Movies.Client.Helpers;
using Movies.Client.Models;

namespace Movies.Client.Services;

public class CRUDSamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public CRUDSamples(IHttpClientFactory httpClientFactory, JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ?? throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));

    }
    public async Task RunAsync()
    {
        //await GetResourceAsync();
        await GetResourceThroughHttpRequestMessageAsync();
    }

    public async Task GetResourceAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml",0.9));
        
        var response = await httpClient.GetAsync("api/movies");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();

        List<Movie>? movies = new();

        if (response.Content.Headers.ContentType?.MediaType == "application/json") { 
            movies = JsonSerializer.Deserialize<List<Movie>>(content, _jsonSerializerOptionsWrapper.options);
        
        } else if (response.Content.Headers.ContentType?.MediaType == "application/xml")
        {
            var serializer = new XmlSerializer(typeof(List<Movie>));
            movies = serializer.Deserialize(new StringReader(content)) as List<Movie>;

        }

        Console.WriteLine(movies);
    }

    public async Task GetResourceThroughHttpRequestMessageAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/movies");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        IEnumerable<Movie>? movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(
            content, _jsonSerializerOptionsWrapper.options);

        Console.WriteLine(movies);
    }
}
