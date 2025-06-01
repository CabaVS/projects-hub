#!/bin/bash
set -e

exec /opt/keycloak/bin/kc.sh start --http-port=8080 \
  --bootstrap-admin-username="${KC_BOOTSTRAP_ADMIN_USERNAME}" \
  --bootstrap-admin-password="${KC_BOOTSTRAP_ADMIN_PASSWORD}"
