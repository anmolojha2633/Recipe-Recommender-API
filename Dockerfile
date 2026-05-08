FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

# ✅ Correct path (THIS is your fix)
RUN dotnet restore "RecipeRecommender/RecipeRecommender.csproj"
RUN dotnet publish "RecipeRecommender/RecipeRecommender.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080

# ✅ Correct DLL
ENTRYPOINT ["dotnet", "RecipeRecommender.dll"]