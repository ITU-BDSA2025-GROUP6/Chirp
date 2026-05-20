FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore Chirp.sln

RUN dotnet publish src/Chirp.Web/Chirp.Web.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Patch OS packages Trivy flags in the Debian base (libgnutls30:
# CVE-2026-33845, CVE-2026-42010). Microsoft's base tag hasn't picked up
# deb12u7 yet, so pull the security fix explicitly.
RUN apt-get update \
    && apt-get install -y --no-install-recommends --only-upgrade libgnutls30 \
    && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appuser && useradd -r -g appuser appuser

COPY --from=build /app/publish .
RUN mkdir -p data && chown -R appuser:appuser /app

USER appuser

ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001

ENTRYPOINT ["dotnet", "Chirp.Web.dll"]