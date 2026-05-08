FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevTrack.Api/DevTrack.Api.csproj", "DevTrack.Api/"]
COPY ["DevTrack.Domain/DevTrack.Domain.csproj", "DevTrack.Domain/"]
COPY ["DevTrack.Database/DevTrack.Database.csproj", "DevTrack.Database/"]
COPY ["DevTrack.Shared/DevTrack.Shared.csproj", "DevTrack.Shared/"]
RUN dotnet restore "./DevTrack.Api/DevTrack.Api.csproj"
COPY . .
WORKDIR "/src/DevTrack.Api"
RUN dotnet build "./DevTrack.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevTrack.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevTrack.Api.dll"]
