variable "prefix" {
  default       = "test"
  description   = "Prefix of the resource group name that's combined with a random ID so name is unique in your Azure subscription."
}


# Shortnames for regions can be found here:
# https://github.com/claranet/terraform-azurerm-regions/blob/master/REGIONS.md
variable "location" {
  default = "southeastasia"
  description   = "Location of the resource group."
}