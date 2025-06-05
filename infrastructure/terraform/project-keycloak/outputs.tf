output "container_app_keycloak_id" {
  value = azurerm_container_app.aca_keycloak.id
}

output "uami_keycloak_principal_id" {
  value = azurerm_user_assigned_identity.uami_aca_keycloak.principal_id
}
