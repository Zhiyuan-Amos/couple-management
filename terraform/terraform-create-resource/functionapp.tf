resource "azurerm_app_service_plan" "changeevent" {
  name                = "changeevent-service-plan"
  location            = var.azurerm_region
  resource_group_name = azurerm_resource_group.app.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "changeevent" {
  name                       = "func-changeevent.azurewebsites.net"
  location                   = var.azurerm_region
  resource_group_name        = azurerm_resource_group.app.name
  app_service_plan_id        = azurerm_app_service_plan.change_event_asp.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  app_settings = {
    "AccountEndpoint"           = azurerm_cosmosdb_account.storage.endpoint
    "AccountKey"                = azurerm_cosmosdb_account.storage.primary_key
    "DatabaseConnectionString"  = azurerm_cosmosdb_account.storage.connection_strings[0]
    "DatabaseName"              = "database"
    "ImagesConnectionString"    = "DefaultEndpointsProtocol=https;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
    "FUNCTIONS_WORKER_RUNTIME"  = "dotnet"
    "WEBSITE_RUN_FROM_PACKAGE"  = "1"
  }
}

resource "azurerm_app_service_plan" "couple_api" {
  name                = "coupleapi-service-plan"
  location            = var.azurerm_region
  resource_group_name = azurerm_resource_group.app.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "coupleapi" {
  name                       = "func-coupleapi.azurewebsites.net"
  location                   = var.azurerm_region
  resource_group_name        = azurerm_resource_group.app.name
  app_service_plan_id        = azurerm_app_service_plan.couple_api_asp.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key

  app_settings = {
    "AccountEndpoint"           = azurerm_cosmosdb_account.storage.endpoint
    "AccountKey"                = azurerm_cosmosdb_account.storage.primary_key
    "DatabaseConnectionString"  = azurerm_cosmosdb_account.storage.connection_strings[0]
    "DatabaseName"              = "database"
    "ImagesConnectionString"    = "DefaultEndpointsProtocol=https;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};EndpointSuffix=core.windows.net"
    "FUNCTIONS_WORKER_RUNTIME"  = "dotnet"
    "WEBSITE_RUN_FROM_PACKAGE"  = "1"
  }
}