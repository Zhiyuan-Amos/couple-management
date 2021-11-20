resource "azurerm_storage_account" "storageAccount" {
    name                     = "couplemgmtstates"
    resource_group_name      = azurerm_resource_group.rg.name
    location                 = azurerm_resource_group.rg.location
    account_tier             = "Standard"
    account_replication_type = "LRS"
    account_kind             = "StorageV2"
    access_tier              = "Hot"
    allow_blob_public_access = true

    timeouts {}
}

resource "azurerm_storage_container" "storageContainer" {
  name                  = "tfstates"
  storage_account_name  = azurerm_storage_account.storageAccount.name
  container_access_type = "blob"
}