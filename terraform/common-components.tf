resource "azurerm_resource_group" "resource-group" {
  name = "xi-portal-${var.environment}"
  location = var.region
}

resource "azurerm_storage_account" "appdata-storage" {
  name = "portalappdata${var.environment}"
  resource_group_name = azurerm_resource_group.resource-group.name
  location = azurerm_resource_group.resource-group.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}

output "appdata_storage_connection" {
  value = azurerm_storage_account.appdata-storage.primary_connection_string
  sensitive = true
}