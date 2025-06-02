variable "resource_group_name" {
  type        = string
  description = "Name of the existing resource group"
}

variable "location" {
  type        = string
  description = "Azure region for the resources"
}

variable "sql_admin_login" {
  type        = string
  description = "Login for SQL Admin"
}

variable "sql_admin_password" {
  type        = string
  description = "Password for SQL Admin"
}
