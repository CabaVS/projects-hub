# Container App for Keycloak
resource "azurerm_container_app" "aca_keycloak" {
  name                         = "aca-keycloak"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.uami_aca_keycloak.id]
  }

  ingress {
    allow_insecure_connections = false
    external_enabled           = true
    target_port                = 8080
    transport                  = "auto"

    traffic_weight {
      percentage      = 100
      label           = "primary"
      latest_revision = true
    }
  }

  registry {
    server   = var.acr_login_server
    identity = azurerm_user_assigned_identity.uami_aca_keycloak.id
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "keycloak"
      image  = "${var.acr_login_server}/keycloak:latest"
      cpu    = 0.5
      memory = "1Gi"

      command = [
        "/opt/keycloak/bin/kc.sh",
        "start",
        "--optimized",
        "--http-port=8080"
      ]

      env {
        name  = "KC_DB"
        value = "mssql"
      }

      env {
        name  = "KC_DB_URL"
        value = "jdbc:sqlserver://${var.sql_server_fqdn}:1433;databaseName=${azurerm_mssql_database.db_keycloak.name};encrypt=true;authentication=ActiveDirectoryManagedIdentity;user=${azurerm_user_assigned_identity.uami_aca_keycloak.client_id}"
      }

      env {
        name  = "KC_HTTP_ENABLED"
        value = "true"
      }

      env {
        name  = "KC_HTTP_PORT"
        value = "8080"
      }

      env {
        name  = "KC_HOSTNAME_STRICT"
        value = "false"
      }

      env {
        name  = "KC_PROXY"
        value = "edge"
      }
    }
  }
}

# SQL Database for Keycloak
resource "azurerm_mssql_database" "db_keycloak" {
  name      = "sqldb-keycloak"
  server_id = var.sql_server_id

  sku_name             = "GP_S_Gen5_1"
  storage_account_type = "Local"

  auto_pause_delay_in_minutes = 15
  max_size_gb                 = 2
  min_capacity                = 0.5
  read_replica_count          = 0
  read_scale                  = false
  zone_redundant              = false

  lifecycle {
    prevent_destroy = true
  }
}

# User-Assigned Managed Identity
resource "azurerm_user_assigned_identity" "uami_aca_keycloak" {
  name                = "uami-aca-keycloak"
  location            = var.location
  resource_group_name = var.resource_group_name
}

# Role assignments
resource "azurerm_role_assignment" "acr_pull_for_aca_expensetrackerapi" {
  scope                = var.acr_id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.uami_aca_keycloak.principal_id
}
