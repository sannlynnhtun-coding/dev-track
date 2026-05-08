FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore

# Build Api
RUN dotnet publish "DevTrack.Api/DevTrack.Api.csproj" -c Release -o /app/api

# Build WebApp
RUN dotnet publish "DevTrack.WebApp/DevTrack.WebApp.csproj" -c Release -o /app/webapp

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/api ./api
COPY --from=build /app/webapp ./webapp

# Create a startup script to run both
RUN echo '#!/bin/bash\n\
# Start the API in the background on localhost:5000\n\
ASPNETCORE_URLS=http://localhost:5000 dotnet /app/api/DevTrack.Api.dll & \n\
API_PID=$!\n\
\n\
# Start the WebApp in the foreground on the port provided by Render\n\
ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} dotnet /app/webapp/DevTrack.WebApp.dll\n\
\n\
# Cleanup\n\
kill $API_PID' > /app/run.sh

RUN chmod +x /app/run.sh

# Default env vars for the combined service
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DatabaseProvider=InMemory
ENV ApiSettings__BaseUrl=http://localhost:5000

EXPOSE 8080
ENTRYPOINT ["/app/run.sh"]
