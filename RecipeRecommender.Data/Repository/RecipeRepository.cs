using RecipeRecommender.Data.Entities;
using RecipeRecommender.Data.Repository;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;

public class RecipeRepository : IRecipeRepository
{
    private readonly IConfiguration _config;

    public RecipeRepository(IConfiguration config)
    {
        _config = config;
    }
    public async Task<List<Recipe>> GetRecipesAsync()
    {
        using IDbConnection db = new Npgsql.NpgsqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var sql = "SELECT * from Recipes";

        var results = await db.QueryAsync<Recipe>(sql);

        // JUST RETURN AS-IS (no split here)
        return results.ToList();
    }
}