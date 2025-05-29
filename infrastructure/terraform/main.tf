terraform {
  required_version = "1.11.4"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.26.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

# Variables
variable "resource_group_name" {}
variable "storage_account_name" {}
variable "subscription_id" {}
variable "sql_admin_group_display_name" {}
variable "sql_admin_group_object_id" {}
variable "sql_admin_group_tenant_id" {}

variable "container_name_for_app_configs" {
  type    = string
  default = "app-configs"
}

# Existing Resource Group
data "azurerm_resource_group" "existing" {
  name = var.resource_group_name
}

# Existing Storage Account
data "azurerm_storage_account" "existing" {
  name                = var.storage_account_name
  resource_group_name = var.resource_group_name
}

# Modules: Shared
module "shared" {
  source = "./shared"

  resource_group_name          = var.resource_group_name
  location                     = data.azurerm_resource_group.existing.location
  sql_admin_group_display_name = var.sql_admin_group_display_name
  sql_admin_group_object_id    = var.sql_admin_group_object_id
  sql_admin_group_tenant_id    = var.sql_admin_group_tenant_id
}

# Modules: Project for Expense Tracker
module "project_expensetracker" {
  source = "./project-expensetracker"

  resource_group_name                    = var.resource_group_name
  location                               = data.azurerm_resource_group.existing.location
  sql_server_id                          = module.shared.sql_server_id
  container_app_environment_id           = module.shared.ace_id
  acr_login_server                       = module.shared.acr_login_server
  acr_id                                 = module.shared.acr_id
  blob_container_scope                   = "${data.azurerm_storage_account.existing.id}/blobServices/default/containers/${var.container_name_for_app_configs}"
  application_insights_connection_string = module.shared.application_insights_connection_string
}

# Modules: Project for Keycloak
module "project_keycloak" {
  source = "./project-keycloak"

  resource_group_name          = var.resource_group_name
  location                     = data.azurerm_resource_group.existing.location
  sql_server_id                = module.shared.sql_server_id
  sql_server_fqdn              = module.shared.sql_server_fqdn
  container_app_environment_id = module.shared.ace_id
  acr_login_server             = module.shared.acr_login_server
  acr_id                       = module.shared.acr_id
}
