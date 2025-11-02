# ----------------------------
# STAGE 1: Build and publish
# ----------------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    COPY *.csproj ./
    
    # Restore dependencies
    RUN dotnet restore ./UrlShortener.csproj
    
    # Copy the rest of the source
    COPY . .
    
    # Build (Release)
    RUN dotnet build ./UrlShortener.csproj -c Release -o /app/build --no-restore
    
    # Publish (Release)
    RUN dotnet publish ./UrlShortener.csproj -c Release -o /app/publish --no-restore
    
    # ----------------------------
    # STAGE 2: Runtime
    # ----------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    
    # Optional for local dev; Railway routes to $PORT automatically
    EXPOSE 8080
    
    # Copy published output
    COPY --from=build /app/publish .
    
    # Bind Kestrel to the container port. Use Railway's PORT if provided.
    ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
    ENV DOTNET_EnableDiagnostics=0
    
    ENTRYPOINT ["dotnet", "UrlShortener.dll"]