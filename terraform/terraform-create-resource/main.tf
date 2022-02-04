terraform {
  required_version = ">=0.12"
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "~>2.84.0"
    }
  }

  backend "azurerm" {
        resource_group_name  = "StateResource"
        storage_account_name = "couplemgmtstates"
        container_name       = "tfstates"
        key                  = "test.terraform.tfstate"
    }
}

resource "random_uuid" "test" {
}

resource "azurerm_resource_group" "rg" {
  name      = "${random_uuid.test.result}-rg-${var.prefix}"
  location  = var.location
}

provider "azurerm" {
  features {}

  subscription_id = "716e9fd7-2458-49f5-a95a-329619d39339"
}

