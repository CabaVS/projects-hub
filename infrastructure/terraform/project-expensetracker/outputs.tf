output "container_app_expensetrackerapi_id" {
  value = azurerm_container_app.aca_expensetrackerapi.id
}

output "uami_expensetrackerapi_principal_id" {
  value = azurerm_user_assigned_identity.uami_aca_expensetrackerapi.principal_id
}
