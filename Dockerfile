# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

# ✅ Correct project path
RUN dotnet restore "RecipeRecommender.API/RecipeRecommender.API.csproj"
RUN dotnet publish "RecipeRecommender.API/RecipeRecommender.API.csproj" -c Release -o /app/out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080

# ✅ Correct DLL name
ENTRYPOINT ["dotnet", "RecipeRecommender.API.dll"]