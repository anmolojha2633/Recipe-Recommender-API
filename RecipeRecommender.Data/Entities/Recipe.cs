using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeRecommender.Data.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ingredients { get; set; } 

        public int Protein { get; set; }
        public int Calories { get; set; }
        public int CookTime { get; set; }

    }
}
