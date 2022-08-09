resource "azurerm_storage_account" "tfstate" {
    name                     = "stcouplemgmtstate"
    resource_group_name      = azurerm_resource_group.tfstate.name
    location                 = var.azurerm_region
    account_tier             = "Standard"
    account_replication_type = "LRS"
    account_kind             = "StorageV2"
    access_tier              = "Hot"
    allow_blob_public_access = true
}

resource "azurerm_storage_container" "tfstate" {
  name                  = "container-tfstate"
  storage_account_name  = azurerm_storage_account.tfstate.name
  container_access_type = "blob"
}