#!/usr/bin/env bash
set -euo pipefail

## Setup for the DB
docker compose up -d postgres

echo "Waiting for Postgres..."
until docker compose exec -T postgres pg_isready -U postgres -d chirp >/dev/null 2>&1; do
  sleep 2
done
## After DB is setup Run app
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=chirp;Username=postgres;Password=postgres"
dotnet run --project src/Chirp.Web

## if you want to work form a "clean" database use 
#docker compose down -v 