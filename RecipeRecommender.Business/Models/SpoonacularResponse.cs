using System.Text.Json.Serialization;

public class SpoonacularResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("missedIngredients")]
    public List<Ingredient>? MissedIngredients { get; set; }

    public class Ingredient
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}