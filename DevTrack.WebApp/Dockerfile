FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevTrack.WebApp/DevTrack.WebApp.csproj", "DevTrack.WebApp/"]
COPY ["DevTrack.Domain/DevTrack.Domain.csproj", "DevTrack.Domain/"]
COPY ["DevTrack.Database/DevTrack.Database.csproj", "DevTrack.Database/"]
COPY ["DevTrack.Shared/DevTrack.Shared.csproj", "DevTrack.Shared/"]
RUN dotnet restore "./DevTrack.WebApp/DevTrack.WebApp.csproj"
COPY . .
WORKDIR "/src/DevTrack.WebApp"
RUN dotnet build "./DevTrack.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevTrack.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevTrack.WebApp.dll"]
