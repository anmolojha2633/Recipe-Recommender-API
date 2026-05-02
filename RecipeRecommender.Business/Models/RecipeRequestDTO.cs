using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRecommender.Business.Models
{
    public class RecipeRequestDTO
    {
        public List<string> Ingredients { get; set; } = new();
        public string Goal { get; set; } = string.Empty;
    }
}
