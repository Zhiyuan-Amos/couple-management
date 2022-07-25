resource "azurerm_cosmosdb_account" "db" {
    access_key_metadata_writes_enabled    = true
    analytical_storage_enabled            = false
    enable_automatic_failover             = false
    # Only one free tier cosmosDB per subscription
    # enable_free_tier                      = true
    enable_multiple_write_locations       = false
    is_virtual_network_filter_enabled     = false
    kind                                  = "GlobalDocumentDB"
    local_authentication_disabled         = false
    location                              = var.azurerm_region
    name                                  = "${var.prefix}-couple-management-storage"
    network_acl_bypass_for_azure_services = false
    network_acl_bypass_ids                = []
    offer_type                            = "Standard"
    public_network_access_enabled         = true
    resource_group_name                   = azurerm_resource_group.rg.name

    capabilities {
        name = "EnableServerless"
    }

    backup {
        interval_in_minutes = 240
        retention_in_hours  = 8
        type                = "Periodic"
    }

    consistency_policy {
        consistency_level       = "ConsistentPrefix"
        max_interval_in_seconds = 5
        max_staleness_prefix    = 100
    }

    geo_location {
        failover_priority = 0
        location          = "southeastasia"
        zone_redundant    = false
    }
}