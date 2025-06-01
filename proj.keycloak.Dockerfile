# Stage 1: Build the fat JAR using Maven
FROM maven:3.9.6-eclipse-temurin-17 AS msal-builder

WORKDIR /app

# Copy Maven config and dependencies
COPY projects/keycloak/pom.xml .

# Let Maven resolve dependencies and cache
RUN mvn dependency:go-offline

# Package fat jar (shaded)
RUN mvn package -DskipTests

# Install zip tool to strip digital signatures
RUN apt-get update && apt-get install -y zip unzip

# Unpack the shaded JAR
RUN mkdir -p /tmp/unjar && cd /tmp/unjar && jar xf /app/target/keycloak-msal4j.jar

# Remove signature metadata
RUN find /tmp/unjar/META-INF -regex '.*\.\(SF\|RSA\|DSA\|EC\)' -exec rm -f {} \;

# Rebuild unsigned JAR
RUN cd /tmp/unjar && jar cf /app/target/clean-msal4j.jar .

# Stage 2: Build Keycloak with MSAL4J support
FROM quay.io/keycloak/keycloak:26.2.5 AS builder

# Set DB vendor to prevent interactive build
ENV KC_DB=mssql

# Switch to root user and Install custom JARs
USER root
COPY --from=msal-builder /app/target/clean-msal4j.jar /opt/keycloak/providers/msal4j.jar
USER 1000

# Build the Keycloak server
RUN /opt/keycloak/bin/kc.sh build

# Stage 3: Final runtime image
FROM quay.io/keycloak/keycloak:26.2.5

# Copy optimized Keycloak from builder
COPY --from=builder /opt/keycloak/ /opt/keycloak/

# Switch to root user and Copy a custom startup script
USER root
COPY projects/keycloak/bootstrap-and-start.sh /opt/keycloak/bootstrap-and-start.sh
RUN chmod +x /opt/keycloak/bootstrap-and-start.sh
USER 1000

# Run custom startup script
CMD ["/opt/keycloak/bootstrap-and-start.sh"]
