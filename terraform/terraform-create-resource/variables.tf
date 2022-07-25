# Shortnames for regions can be found here:
# https://github.com/claranet/terraform-azurerm-regions/blob/master/REGIONS.md
variable "azurerm_region" {
  description = "Standard Azure region in shortname format for resource naming purpose"
  type        = string
  default     = "southeastasia"
}

variable "env" {
  description = "Environment for resource names"
  type        = string
  default     = "test"
}