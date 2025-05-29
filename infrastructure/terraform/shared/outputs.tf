output "log_analytics_workspace_id" {
  value = azurerm_log_analytics_workspace.law.id
}

output "application_insights_connection_string" {
  value = azurerm_application_insights.app_insights.connection_string
}

output "ace_id" {
  value = azurerm_container_app_environment.ace.id
}

output "acr_login_server" {
  value = azurerm_container_registry.acr.login_server
}

output "acr_id" {
  value = azurerm_container_registry.acr.id
}

output "sql_server_id" {
  value = azurerm_mssql_server.mssql_server.id
}

output "sql_server_fqdn" {
  value = azurerm_mssql_server.mssql_server.fully_qualified_domain_name
}
