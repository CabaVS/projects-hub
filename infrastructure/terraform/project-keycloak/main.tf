# Container App for Keycloak
resource "azurerm_container_app" "aca_keycloak" {
  name                         = "aca-keycloak"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

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

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "keycloak"
      image  = "quay.io/keycloak/keycloak:26.2.5"
      cpu    = 0.5
      memory = "1Gi"

      command = [
        "/opt/keycloak/bin/kc.sh",
        "start",
        "--http-port=8080"
      ]

      env {
        name  = "KC_DB"
        value = "mssql"
      }

      env {
        name  = "KC_DB_URL"
        value = "jdbc:sqlserver://${var.sql_server_fqdn}:1433;databaseName=${azurerm_mssql_database.db_keycloak.name};encrypt=true;trustServerCertificate=false;loginTimeout=60"
      }

      env {
        name  = "KC_DB_USERNAME"
        value = var.sql_username
      }

      env {
        name  = "KC_DB_PASSWORD"
        value = var.sql_password
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
        name  = "KC_HOSTNAME"
        value = var.container_app_hostname
      }

      env {
        name  = "KC_PROXY"
        value = "edge"
      }

      env {
        name  = "KEYCLOAK_ADMIN"
        value = var.keycloak_login
      }

      env {
        name  = "KEYCLOAK_ADMIN_PASSWORD"
        value = var.keycloak_password
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
