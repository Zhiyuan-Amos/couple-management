# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Shared ./Shared
COPY Api/Couple.Api.csproj ./Api/Couple.Api.csproj
RUN dotnet restore Api

# Copy everything else and build
COPY Api ./Api
RUN dotnet publish Api -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Couple.Api.dll"]
