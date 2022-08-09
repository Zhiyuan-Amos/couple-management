resource "azurerm_storage_account" "images" {
  name                      = "stcouplemgmtimages"
  resource_group_name       = azurerm_resource_group.app.name
  location                  = var.azurerm_region
  account_tier              = "Standard"
  account_replication_type  = "LRS"
  allow_blob_public_access  = true
}

resource "azurerm_storage_container" "images" {
  name                  = "container-images"
  storage_account_name  = azurerm_storage_account.images.name
  container_access_type = "private"
}
