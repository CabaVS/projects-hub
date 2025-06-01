#!/bin/bash
set -e

# Conditionally create admin user
if [[ -n "$KC_BOOTSTRAP_ADMIN_USERNAME" && -n "$KC_BOOTSTRAP_ADMIN_PASSWORD" ]]; then
  exec /opt/keycloak/bin/kc.sh start --optimized --http-port=8080 \
    --bootstrap-admin-username="${KC_BOOTSTRAP_ADMIN_USERNAME}" \
    --bootstrap-admin-password="${KC_BOOTSTRAP_ADMIN_PASSWORD}"
else
  exec /opt/keycloak/bin/kc.sh start --optimized --http-port=8080
fi
