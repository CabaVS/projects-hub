# Stage 1: Build optimized Keycloak distribution
FROM quay.io/keycloak/keycloak:26.2.5 AS builder

# Set DB vendor to prevent interactive build
ENV KC_DB=mssql

# Build the Keycloak server (avoids needing --optimized at runtime)
RUN /opt/keycloak/bin/kc.sh build

# Stage 2: Final runtime image
FROM quay.io/keycloak/keycloak:26.2.5

COPY --from=builder /opt/keycloak/ /opt/keycloak/

# Expose port and default to start in optimized mode
CMD [ "start", "--optimized", "--http-port=8080" ]
