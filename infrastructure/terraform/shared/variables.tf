variable "resource_group_name" {
  type        = string
  description = "Name of the existing resource group"
}

variable "location" {
  type        = string
  description = "Azure region for the resources"
}

variable "sql_admin_group_display_name" {
  type        = string
  description = "AAD group display name for SQL Admin"
}

variable "sql_admin_group_object_id" {
  type        = string
  description = "AAD group object ID for SQL Admin"
}

variable "sql_admin_group_tenant_id" {
  type        = string
  description = "AAD tenant ID for SQL Admin"
}
