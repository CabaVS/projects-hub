#!/bin/bash
set -e

# Conditionally create admin user
if [[ -n "$KC_BOOTSTRAP_ADMIN_USERNAME" && -n "$KC_BOOTSTRAP_ADMIN_PASSWORD" ]]; then
  echo "[CVS_BOOT] Bootstrapping admin user..."
  /opt/keycloak/bin/kc.sh bootstrap-admin \
    --user "$KC_BOOTSTRAP_ADMIN_USERNAME" \
    --password "$KC_BOOTSTRAP_ADMIN_PASSWORD"
else
  echo "[CVS_BOOT] Admin user creation skipped."
fi

# Start Keycloak
exec /opt/keycloak/bin/kc.sh start --optimized --http-port=8080
