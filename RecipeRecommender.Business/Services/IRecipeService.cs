using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeRecommender.Business.Models;

namespace RecipeRecommender.Business.Services
{
    public interface IRecipeService
    {
        Task <List<RecipeResponseDTO>> GetRecommendedRecipesAsync(RecipeRequestDTO request);
    }
}
