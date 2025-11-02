# ----------------------------
# STAGE 1: Build and publish
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY UrlShortener/*.csproj ./UrlShortener/

RUN dotnet restore ./UrlShortener/UrlShortener.csproj

# Copy the rest of the source code
COPY UrlShortener/ ./UrlShortener/

# Build (Release)
RUN dotnet build ./UrlShortener/UrlShortener.csproj -c Release -o /app/build --no-restore

# Publish (Release)
RUN dotnet publish ./UrlShortener/UrlShortener.csproj -c Release -o /app/publish --no-restore

# ----------------------------
# STAGE 2: Runtime
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV DOTNET_EnableDiagnostics=0

EXPOSE 8080

# Copy published output
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UrlShortener.dll"]