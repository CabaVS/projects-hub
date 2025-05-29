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

variable "sql_server_fqdn" {
  type        = string
  description = "Fully Qualified Domain Name of the SQL Server"
}

variable "container_app_environment_id" {
  type        = string
  description = "ID of the Container App Environment"
}

variable "acr_login_server" {
  type        = string
  description = "ID of the Container App Environment"
}
