terraform {
  required_version = ">=0.12"
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "~>2.84.0"
    }
  }

  backend "azurerm" {
        resource_group_name  = "testResource"
        storage_account_name = var.storagename
        container_name       = "tfstate"
        key                  = "terraform.tfstate"
    }
}

provider "azurerm" {
  features {}
}

