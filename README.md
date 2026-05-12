An AI-powered Recipe Recommendation Backend built using ASP.NET Core Web API, integrated with:

🧠 AI-based recipe suggestions
🍲 Spoonacular API integration
⚡ Redis caching for faster responses
🐘 PostgreSQL database support
☁️ Docker + Render deployment ready



🚀 Live Demo
Backend API
https://recipe-backend-bcj1.onrender.com
Frontend UI
https://recipe-ui-7vnh.onrender.com

it might fail as we need to migrate the DB data from Local To Cloud , which we have not done yet .



🛠️ Tech Stack
Technology	Usage
ASP.NET Core 8	Backend API
C#	Programming Language
PostgreSQL	Database
Redis	Caching
Spoonacular API	Recipe Data
OpenRouter AI	AI Suggestions
Docker	Containerization
Angular FrontEnd UI

Project Structure :

RecipeRecommender/
│
├── RecipeRecommender/              # API Project
├── RecipeRecommender.Business/     # Business Logic Layer
├── RecipeRecommender.Data/         # Repository/Data Layer
├── Dockerfile
├── RecipeRecommender.sln




✨ Features
🔍 Search recipes using ingredients
🤖 AI-generated recipe suggestions
⚡ Redis caching for optimized API performance
🐳 Dockerized deployment
☁️ Render cloud deployment
📦 Layered architecture implementation
🌐 RESTful APIs
🔒 Environment-based configuration


🐳 Docker Setup
Build Docker Image
docker build -t recipe-api .
Run Container
docker run -p 8080:8080 recipe-api


Request Body
{
  "ingredients": "eggs, tomato, onion"
}
Response
[
  {
    "title": "Tomato Egg Omelette",
    "instructions": "..."
  }
]
