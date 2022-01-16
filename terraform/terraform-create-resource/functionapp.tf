resource "azurerm_app_service_plan" "asp" {
  name                = "${var.prefix}-service-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_function_app" "fa" {
  name                       = "${var.prefix}-change-event"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  app_service_plan_id        = azurerm_app_service_plan.asp.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  app_settings = {
    "AccountEndpoint"           = azurerm_cosmosdb_account.db.endpoint
    "AccountKey"                = azurerm_cosmosdb_account.db.primary_key
    "DatabaseConnectionString"  = azurerm_cosmosdb_account.db.connection_strings[0]
    "DatabaseName"              = "database"
    "ImagesConnectionString"    = "DefaultEndpointsProtocol=https;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
    "FUNCTIONS_WORKER_RUNTIME"  = "dotnet"
    "WEBSITE_RUN_FROM_PACKAGE"  = "1"
  }
}