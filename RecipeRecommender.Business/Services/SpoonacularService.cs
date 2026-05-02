using Microsoft.Extensions.Configuration;
using RecipeRecommender.Business.Models;
using System.Text.Json;

namespace RecipeRecommender.Business.Services
{
    public class SpoonacularService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SpoonacularService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Spoonacular:ApiKey"];
        }

        public async Task<List<RecipeResponseDTO>> GetRecipeFromApi(List<string> ingredients)
        {
            var ingredientQuery = string.Join(",", ingredients);

            var url = $"https://api.spoonacular.com/recipes/findByIngredients?ingredients={ingredientQuery}&number=5&apiKey={_apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<RecipeResponseDTO>();
            }

            var json = await response.Content.ReadAsStringAsync();

            var apiRecipes = JsonSerializer.Deserialize<List<SpoonacularResponse>>(json);

            // 🔥 SAFETY CHECK
            if (apiRecipes == null)
            {
                return new List<RecipeResponseDTO>();
            }
            return apiRecipes.Select(r => new RecipeResponseDTO
            {
                Name = r.Title ?? "Unknown",
                Protein = 999,

                MissingIngredients = r.MissedIngredients != null
                    ? r.MissedIngredients
                        .Where(i => i != null)
                        .Select(i => i.Name ?? "")
                        .ToList()
                    : new List<string>()

            }).ToList();
        }
    }
}

   