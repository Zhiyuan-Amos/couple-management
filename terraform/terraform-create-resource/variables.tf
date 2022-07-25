# Shortnames for regions can be found here:
# https://github.com/claranet/terraform-azurerm-regions/blob/master/REGIONS.md
variable "location" {
  description = "Location of the resources"
  type        = string
  default     = "southeastasia"
}

variable "env" {
  description = "Environment for resource names"
  type        = string
  default     = "test"
}