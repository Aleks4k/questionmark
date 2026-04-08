# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore
COPY questionmark.sln .
COPY questionmark.Api/questionmark.Api.csproj questionmark.Api/
COPY questionmark.Application/questionmark.Application.csproj questionmark.Application/
COPY questionmark.Domain/questionmark.Domain.csproj questionmark.Domain/
COPY questionmark.Infrastructure/questionmark.Infrastructure.csproj questionmark.Infrastructure/
COPY questionmark.Tests/questionmark.Tests.csproj questionmark.Tests/

RUN dotnet restore

# Copy everything and build
COPY . .
WORKDIR /src/questionmark.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "questionmark.Api.dll"]