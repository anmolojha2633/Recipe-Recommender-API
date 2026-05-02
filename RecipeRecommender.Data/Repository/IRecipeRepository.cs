using RecipeRecommender.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRecommender.Data.Repository
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetRecipesAsync();
    }
}
