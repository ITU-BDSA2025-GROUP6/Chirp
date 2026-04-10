#!/usr/bin/env bash
set -euo pipefail

# Provisions a DigitalOcean droplet for ITU-MiniTwit via API.
# Required env vars:
#   DO_TOKEN               DigitalOcean API token
#   DO_SSH_KEY_FINGERPRINT Fingerprint of SSH key already added in DO
# Optional env vars:
#   DO_REGION              Default: fra1
#   DO_SIZE                Default: s-1vcpu-1gb
#   DO_IMAGE               Default: ubuntu-22-04-x64
#   DO_DROPLET_NAME        Default: itu-minitwit

: "${DO_TOKEN:?DO_TOKEN is required}"
: "${DO_SSH_KEY_FINGERPRINT:?DO_SSH_KEY_FINGERPRINT is required}"

DO_REGION="${DO_REGION:-fra1}"
DO_SIZE="${DO_SIZE:-s-1vcpu-1gb}"
DO_IMAGE="${DO_IMAGE:-ubuntu-22-04-x64}"
DO_DROPLET_NAME="${DO_DROPLET_NAME:-itu-minitwit}"

create_payload() {
  cat <<JSON
{
  "name": "${DO_DROPLET_NAME}",
  "region": "${DO_REGION}",
  "size": "${DO_SIZE}",
  "image": "${DO_IMAGE}",
  "ssh_keys": ["${DO_SSH_KEY_FINGERPRINT}"],
  "tags": ["itu-minitwit"],
  "ipv6": false,
  "monitoring": true
}
JSON
}

echo "Verifying existence of droplet '${DO_DROPLET_NAME}' in region '${DO_REGION}'..."
existing_response="$(curl -sS -X GET "https://api.digitalocean.com/v2/droplets?name=${DO_DROPLET_NAME}" \
  -H "Authorization: Bearer ${DO_TOKEN}" \
  -H "Content-Type: application/json")"
  
count="$(echo "${existing_response}" | jq '.droplets | length')"

if [[ "$count" -gt 0 ]]; then 
  echo "Droplet '${DO_DROPLET_NAME}' already exists. Skipping creation."
  droplet_id="$(echo "${existing_response}" | jq -r '.droplets[0].id')"
  ip="$(echo "${existing_response}" | jq -r '.droplets[0].networks.v4[] | select(.type=="public") | .ip_address')"
  echo "VM ready."
  echo "DROPLET_ID=${droplet_id}"
  echo "DROPLET_IP=${ip}"
  echo
  echo "Next step:"
  echo "export DEPLOY_HOST=${ip}"
  echo "./scripts/deploy_itu_minitwit.sh"
  exit 0
fi

echo "Creating droplet '${DO_DROPLET_NAME}' in region '${DO_REGION}'..."
create_response="$(curl -sS -X POST "https://api.digitalocean.com/v2/droplets" \
  -H "Authorization: Bearer ${DO_TOKEN}" \
  -H "Content-Type: application/json" \
  -d "$(create_payload)")"

droplet_id="$(echo "${create_response}" | jq -r '.droplet.id // empty')"

if [[ -z "${droplet_id}" ]]; then
  echo "Failed to create droplet. API response:" >&2
  echo "${create_response}" >&2
  exit 1
fi

echo "Droplet created with id: ${droplet_id}"
echo "Waiting for public IPv4..."

for _ in $(seq 1 60); do
  droplet_response="$(curl -sS "https://api.digitalocean.com/v2/droplets/${droplet_id}" \
    -H "Authorization: Bearer ${DO_TOKEN}")"

  ip="$(echo "${droplet_response}" | jq -r '.droplet.networks.v4[] | select(.type=="public") | .ip_address' 2>/dev/null || true)"
  status="$(echo "${droplet_response}" | jq -r '.droplet.status')"
  
  if [[ -n "${ip}" ]]; then
    echo "VM ready."
    echo "DROPLET_ID=${droplet_id}"
    echo "DROPLET_IP=${ip}"
    echo
    echo "Next step:"
    echo "  export DEPLOY_HOST=${ip}"
    echo "  ./scripts/deploy_itu_minitwit.sh"
    exit 0
  fi

  echo "status=${status:-unknown}, waiting..."
  sleep 5
done

echo "Timed out waiting for IP address." >&2
exit 1
