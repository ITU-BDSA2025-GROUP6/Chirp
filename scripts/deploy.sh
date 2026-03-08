#!/usr/bin/env bash
set -euo pipefail

APP_DIR="${APP_DIR:-/opt/itu-minitwit}"
COMPOSE_FILE="${COMPOSE_FILE:-$APP_DIR/docker-compose.yml}"

log() { echo "[$(date -IS)] $*"; }
die() { echo "[$(date -is)] ERROR $*" >&2; exit 1;}

log "Starting deploy..."
log "APP_DIR=$APP_DIR"
log "TAG=${TAG:-<not set>} (defaults to TAG in .env or :latest)"

cd "$APP_DIR" || die "Cannot cd to $APP_DIR"
test -f "$COMPOSE_FILE" || die "Missing $COMPOSE_FILE"

# Preflight: docker + compose plugin must exist
if ! command -v docker >/dev/null 2>&1; then
  echo "ERROR: docker not installed"
  exit 1
fi 
if ! docker compose version >/dev/null 2>&1; then
  echo "ERROR: docker compose plugin not available"
  exit 1
fi

# Preflight: show resolved images (helps debugging)
log "Resolved images:"
TAG="${TAG:-}" docker compose config 2>/dev/null | awk '/image:/ {print "  " $0}' || true

# Pull + run (idempotent)
log "Pulling images..."
TAG="${TAG:-}" docker compose pull

log "Applying compose (up -d)..."
TAG="${TAG:-}" docker compose up -d --remove-orphans

# Clean unused images (safe: does NOT remove volumes)
log "Pruning unused images..."
docker image prune -f || true

log "Deploy finished. Current containers:"
docker ps --format "table {{.Names}}\t{{.Image}}\t{{.Status}}"