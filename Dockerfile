FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "RecipeRecommender/RecipeRecommender.csproj"
RUN dotnet publish "RecipeRecommender/RecipeRecommender.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "RecipeRecommender.dll"]