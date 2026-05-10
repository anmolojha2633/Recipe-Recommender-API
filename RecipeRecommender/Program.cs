using RecipeRecommender.Business.Services;
using RecipeRecommender.Data.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddHttpClient<SpoonacularService>();
builder.Services.AddHttpClient<AiService>();

// Redis 
builder.Services.AddSingleton<IConnectionMultiplexer?>(sp =>
{
    try
    {
        var redisConnection = ConnectionMultiplexer.Connect(
            builder.Configuration["Redis:Connection"]
        );

        Console.WriteLine("Redis connected ✅");
        return redisConnection;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Redis NOT available ❌: {ex.Message}");
        return null;
    }
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");


// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");



app.MapGet("/", () => "Recipe API is running 🚀");

app.Run();