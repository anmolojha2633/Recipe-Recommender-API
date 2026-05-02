using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRecommender.Business.Models
{
    public class RecipeResponseDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Protein { get; set; }
        public List<string> MissingIngredients { get; set; } = new();
    }
}
