resource "azurerm_storage_account" "storage" {
  name                      = "${var.prefix}couplemgmtstorage"
  resource_group_name       = azurerm_resource_group.rg.name
  location                  = "southeastasia"
  account_tier              = "Standard"
  account_replication_type  = "LRS"
  allow_blob_public_access  = true
}

resource "azurerm_storage_container" "images" {
  name                  = "images"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}
