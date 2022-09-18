terraform {
  required_version = ">=0.12"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.91.0"
    }
  }

  backend "azurerm" {
        resource_group_name  = "rg-couplemgmt-tfstate"
        storage_account_name = "stcouplemgmtstate"
        container_name       = "container-tfstate"
        key                  = "terraform.tfstate"
    }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "app" {
  name      = "rg-couplemgmt"
  location  = var.azurerm_region
}
