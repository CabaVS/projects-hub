variable "resource_group_name" {
  type        = string
  description = "Resource group where the ACA is deployed"
}

variable "location" {
  type        = string
  description = "Azure region"
}

variable "acr_id" {
  type        = string
  description = "Resource ID of the ACR (used for role assignment)"
}

variable "acr_login_server" {
  type        = string
  description = "Login server URL for the Azure Container Registry"
}

variable "sql_server_id" {
  type        = string
  description = "ID of the SQL Server"
}

variable "sql_server_fqdn" {
  type        = string
  description = "Fully Qualified Domain Name of the SQL Server"
}

variable "sql_username" {
  type        = string
  description = "Username of the SQL Database"
}

variable "sql_password" {
  type        = string
  description = "Password of the SQL Database"
}

variable "keycloak_login" {
  type        = string
  description = "Username of the Admin for Keycloak console"
}

variable "keycloak_password" {
  type        = string
  description = "Password of the Admin for Keycloak console"
}

variable "container_app_environment_id" {
  type        = string
  description = "ID of the Container App Environment"
}

variable "container_app_hostname" {
  type        = string
  description = "ACA hostname (required by Keycloak admin console). Warning: unknown till creation (use static custom domain when possible)."
}
