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
        key                  = "any-branch-name.tfstate"
    }
}

resource "azurerm_resource_group" "rg" {
  name      = "${var.prefix}Resource"
  location  = var.location
}

provider "azurerm" {
  features {}
}

