using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RecipeRecommender.Business.Services
{
    public class AiService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AiService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GetRecipeStepsAsync(string recipeName)
        {
            if (string.IsNullOrWhiteSpace(recipeName))
                return "Invalid recipe name.";

            var apiKey = _config["OpenRouter:ApiKey"];  
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:4200");
                _httpClient.DefaultRequestHeaders.Add("X-Title", "Recipe App");

                var requestBody = new
                {
                    model = "openrouter/free",
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = $"Explain how to cook {recipeName} in 50 words."
                        }
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    content
                );

                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"AI error ({(int)response.StatusCode})";
                }

                var json = JsonDocument.Parse(responseString);

                var result = json
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return result ?? "No response";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}