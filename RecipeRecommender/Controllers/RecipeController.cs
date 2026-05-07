using Microsoft.AspNetCore.Mvc;
using RecipeRecommender.Business.Services;
using RecipeRecommender.Business.Models;

[ApiController]
[Route("api/recipes")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _service;

    public RecipeController(IRecipeService service)
    {
        _service = service;
    }

    [HttpPost("recommend")]
    public async Task<IActionResult> Recommend([FromBody] RecipeRequestDTO request)
    {
        var result = await _service.GetRecommendedRecipesAsync(request);
        return Ok(result);
    }
    [HttpPost("ai-steps")]
    public async Task<IActionResult> GetAiSteps(
    [FromBody] string recipeName,
    [FromServices] AiService aiService)
    {
        var result = await aiService.GetRecipeStepsAsync(recipeName);
        return Ok(result);
    }
}