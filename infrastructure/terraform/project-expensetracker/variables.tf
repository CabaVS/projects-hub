variable "resource_group_name" {
  type        = string
  description = "Resource group where the ACA is deployed"
}

variable "location" {
  type        = string
  description = "Azure region"
}

variable "sql_server_id" {
  type        = string
  description = "ID of the SQL Server"
}

variable "container_app_environment_id" {
  type        = string
  description = "ID of the Container App Environment"
}

variable "acr_login_server" {
  type        = string
  description = "Login server URL for the Azure Container Registry"
}

variable "acr_id" {
  type        = string
  description = "Resource ID of the ACR (used for role assignment)"
}

variable "blob_container_scope" {
  type        = string
  description = "Scope path for the blob container (used for RBAC)"
}

variable "application_insights_connection_string" {
  type        = string
  description = "Connection string for the shared Application Insights instance"
}
