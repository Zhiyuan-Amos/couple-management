# Shortnames for regions can be found here:
# https://github.com/claranet/terraform-azurerm-regions/blob/master/REGIONS.md
variable "location" {
  default = "southeastasia"
  description   = "Location of the resource group."
}