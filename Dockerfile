# Use SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["inmind_session5_DDD.API/inmind_session5_DDD.API.csproj", "inmind_session5_DDD.API/"]
COPY ["inmind_session5_DDD.Application/inmind_session5_DDD.Application.csproj", "inmind_session5_DDD.Application/"]
COPY ["inmind_session5_DDD.Common/inmind_session5_DDD.Common.csproj", "inmind_session5_DDD.Common/"]
COPY ["inmind_session5_DDD.Domain/inmind_session5_DDD.Domain.csproj", "inmind_session5_DDD.Domain/"]
COPY ["inmind_session5_DDD.Persistence/inmind_session5_DDD.Persistence.csproj", "inmind_session5_DDD.Persistence/"]
COPY ["inmind_session5_DDD.Infrastructure/inmind_session5_DDD.Infrastructure.csproj", "inmind_session5_DDD.Infrastructure/"]
RUN dotnet restore "inmind_session5_DDD.API/inmind_session5_DDD.API.csproj"

# Copy entire project
COPY . .
WORKDIR "/src/inmind_session5_DDD.API"
RUN dotnet publish "inmind_session5_DDD.API.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5120
ENTRYPOINT ["dotnet", "inmind_session5_DDD.API.dll"]
