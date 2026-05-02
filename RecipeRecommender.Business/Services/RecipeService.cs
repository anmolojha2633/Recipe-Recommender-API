using RecipeRecommender.Business.Models;
using RecipeRecommender.Data.Entities;
using RecipeRecommender.Data.Repository;

namespace RecipeRecommender.Business.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repo;
        private readonly SpoonacularService _spoonacularService;
        private readonly RedisCacheService _redisCacheService;

        public RecipeService(IRecipeRepository recipeRepository, SpoonacularService spoonacularService,RedisCacheService redisCacheService)
        {
            _repo = recipeRepository;
            _spoonacularService = spoonacularService;
            _redisCacheService = redisCacheService;
        }

        public async Task<List<RecipeResponseDTO>> GetRecommendedRecipesAsync(RecipeRequestDTO request)
        {
            var cacheKey = $"recipes:{string.Join(",", request.Ingredients).ToLower()}";  // ⭐ moved to top

            var cachedResult = await _redisCacheService.GetAsync<List<RecipeResponseDTO>>(cacheKey); // ⭐ moved before DB call

            if (cachedResult != null)
            {
                Console.WriteLine("CACHE HIT ");
                return cachedResult;
            }

            var dbRecipes = await _repo.GetRecipesAsync();
            var apiRecipes = await _spoonacularService.GetRecipeFromApi(request.Ingredients);

            var userIngredients = NormalizeList(request.Ingredients);

            var allResults = new List<(RecipeResponseDTO Recipe, int Score)>();

            

                foreach (var r in dbRecipes)
            {
                var ingredientsList = NormalizeList(
                    r.Ingredients.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

                int matchCount = ingredientsList.Count(i => userIngredients.Contains(i));

                if (matchCount == 0)
                    continue;

                var missing = ingredientsList
                    .Where(i => !userIngredients.Contains(i))
                    .ToList();

                int score = CalculateScore(matchCount, missing.Count, r.Protein);

                allResults.Add((
                    new RecipeResponseDTO
                    {
                        Name = r.Name,
                        Protein = r.Protein,
                        MissingIngredients = missing
                    },
                    score
                ));
            }

            
            foreach (var r in apiRecipes)
            {
                var missing = NormalizeList(r.MissingIngredients ?? new List<string>());

                int matchCount = userIngredients.Count - missing.Count;

                if (matchCount <= 0)
                    continue;

                int score = CalculateScore(matchCount, missing.Count, 0);

                allResults.Add((
                    new RecipeResponseDTO
                    {
                        Name = r.Name,
                        Protein = r.Protein,
                        MissingIngredients = missing
                    },
                    score
                ));
            }
           

            var results =allResults
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select(x => x.Recipe)
                .ToList();


            await _redisCacheService.SetAsync(cacheKey, results);

            return results;
        }

        
        private List<string> NormalizeList(IEnumerable<string> list)
        {
            return list
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLower())
                .ToList();
        }

        
        private int CalculateScore(int matchCount, int missingCount, int protein)
        {
            return (matchCount * 100) - (missingCount * 10) + (protein * 2);
        }
    }
}