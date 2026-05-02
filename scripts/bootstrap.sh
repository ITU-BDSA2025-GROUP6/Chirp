#!/usr/bin/env bash
set -euo pipefail

# Bootstraps a fresh Ubuntu 22.04 droplet for ITU-MiniTwit.
# Idempotent: safe to run multiple times.
# Run as root (or with sudo) on the target droplet.

log() { echo "[$(date '+%Y-%m-%d %H:%M:%S')] $*"; }

# ── Docker ────────────────────────────────────────────────────────────────────
if command -v docker >/dev/null 2>&1; then
  log "Docker already installed ($(docker --version)). Skipping."
else
  log "Installing Docker..."
  apt-get update -qq
  apt-get install -y -qq ca-certificates curl gnupg

  install -m 0755 -d /etc/apt/keyrings
  curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
    | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
  chmod a+r /etc/apt/keyrings/docker.gpg

  echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \
    https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
    > /etc/apt/sources.list.d/docker.list

  apt-get update -qq
  apt-get install -y -qq docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

  systemctl enable --now docker
  log "Docker installed."
fi

# ── Docker Compose plugin sanity check ────────────────────────────────────────
if ! docker compose version >/dev/null 2>&1; then
  echo "ERROR: docker compose plugin not available after install." >&2
  exit 1
fi
log "Docker Compose plugin OK ($(docker compose version --short))."

# ── App directories ───────────────────────────────────────────────────────────
for dir in /opt/itu-minitwit /opt/monitoring; do
  if [[ -d "$dir" ]]; then
    log "Directory $dir already exists. Skipping."
  else
    mkdir -p "$dir"
    log "Created $dir."
  fi
done

log "Bootstrap complete."
