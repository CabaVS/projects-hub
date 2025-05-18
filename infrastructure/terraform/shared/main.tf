# Log Analytics Workspace
resource "azurerm_log_analytics_workspace" "law" {
  name                = "log-cabavsprojectshub"
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

# Azure Container Registry
resource "azurerm_container_registry" "acr" {
  name                = "acrcabavsprojectshub"
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Basic"
  admin_enabled       = false
}

# Container App Environment
resource "azurerm_container_app_environment" "ace" {
  name                       = "ace-cabavsprojectshub"
  location                   = var.location
  resource_group_name        = var.resource_group_name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.law.id
}

# SQL Server
resource "azurerm_mssql_server" "mssql_server" {
  name                = "sql-cabavsprojectshub"
  resource_group_name = var.resource_group_name
  location            = var.location
  version             = "12.0"

  azuread_administrator {
    login_username              = var.sql_admin_group_display_name
    object_id                   = var.sql_admin_group_object_id
    tenant_id                   = var.sql_admin_group_tenant_id
    azuread_authentication_only = true
  }
}