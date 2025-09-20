using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace BingPulseHack.Controllers;

public class BingApiSettings
{
    public string AppId { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}

public class SearchRequest
{
    public string Query { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly BingApiSettings _settings;
    private readonly ILogger<SearchController> _logger;

    public SearchController(
        IHttpClientFactory httpClientFactory,
        IOptions<BingApiSettings> settings,
        ILogger<SearchController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var bingRequest = new
            {
                query = request.Query,
                Answers = new
                {
                    webPages = new { maxItems = 20 },
                    news = new { },
                    QuestionsAndAnswers = new { maxItems = 20 },
                    travel = new { },
                    places = new { }
                }
            };

            var requestUrl = $"{_settings.Endpoint}?appid={_settings.AppId}&cc=IN&setlang=en";
            _logger.LogInformation($"Sending request to Bing API: {requestUrl}");
            _logger.LogInformation($"Request body: {JsonSerializer.Serialize(bingRequest)}");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(bingRequest),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            // Add required headers
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "BingPulseHack/1.0");

            var response = await client.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Response Status: {response.StatusCode}");
            _logger.LogInformation($"Response Content: {content}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Bing API error: {content}");
                return StatusCode((int)response.StatusCode, new { error = "Error from Bing API", details = content });
            }

            try
            {
                // Try to parse the response to ensure it's valid JSON
                var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(content);
                return Content(content, "application/json");
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Invalid JSON response from Bing API: {ex.Message}");
                return BadRequest(new { error = "Invalid response from Bing API" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing search request");
            return StatusCode(500, new { error = "An error occurred while processing your request" });
        }
    }
}