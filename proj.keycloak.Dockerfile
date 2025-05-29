# Stage 1: Build optimized Keycloak distribution
FROM quay.io/keycloak/keycloak:26.2.5 AS builder

# Set DB vendor to prevent interactive build
ENV KC_DB=mssql

# Download MSAL4J JAR for Entra Id authentication
ADD https://repo1.maven.org/maven2/com/microsoft/azure/msal4j/1.13.3/msal4j-1.13.3.jar /tmp/msal4j-1.13.3.jar

# Copy JAR into the final Keycloak directory via root-writable temp path
USER root
RUN mkdir -p /opt/keycloak/providers \
    && cp /tmp/msal4j-1.13.3.jar /opt/keycloak/providers/ \
    && chmod 644 /opt/keycloak/providers/msal4j-1.13.3.jar

# Switch back to keycloak user for build
USER 1000

# Build the Keycloak server (avoids needing --optimized at runtime)
RUN /opt/keycloak/bin/kc.sh build

# Stage 2: Final runtime image
FROM quay.io/keycloak/keycloak:26.2.5

# Copy optimized Keycloak from builder
COPY --from=builder /opt/keycloak/ /opt/keycloak/

# Expose port and default to start in optimized mode
CMD [ "start", "--optimized", "--http-port=8080" ]
