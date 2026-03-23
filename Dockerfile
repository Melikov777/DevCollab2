# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY ["DevCollab.slnx", "./"]
COPY ["src/DevCollab.API/DevCollab.API.csproj", "src/DevCollab.API/"]
COPY ["src/DevCollab.Application/DevCollab.Application.csproj", "src/DevCollab.Application/"]
COPY ["src/DevCollab.Domain/DevCollab.Domain.csproj", "src/DevCollab.Domain/"]
COPY ["src/DevCollab.Infrastructure/DevCollab.Infrastructure.csproj", "src/DevCollab.Infrastructure/"]
COPY ["src/DevCollab.Persistence/DevCollab.Persistence.csproj", "src/DevCollab.Persistence/"]

# Restore dependencies
RUN dotnet restore "src/DevCollab.API/DevCollab.API.csproj"

# Copy all the source code
COPY . .
WORKDIR "/src/src/DevCollab.API"

# Build the application
RUN dotnet build "DevCollab.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DevCollab.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevCollab.API.dll"]
