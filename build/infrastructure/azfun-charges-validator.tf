# Copyright 2020 Energinet DataHub A/S
#
# Licensed under the Apache License, Version 2.0 (the "License2");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
module "azfun_charges_validator" {
  source                                    = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//function-app?ref=1.2.0"
  name                                      = "azfun-validator-${var.project}-${var.organisation}-${var.environment}"
  resource_group_name                       = data.azurerm_resource_group.main.name
  location                                  = data.azurerm_resource_group.main.location
  storage_account_access_key                = module.azfun_charges_validator_stor.primary_access_key
  app_service_plan_id                       = module.azfun_charges_validator_plan.id
  storage_account_name                      = module.azfun_charges_validator_stor.name
  application_insights_instrumentation_key  = module.appi.instrumentation_key
  tags                                      = data.azurerm_resource_group.main.tags
  app_settings                              = {
    # Region: Default Values
    WEBSITE_ENABLE_SYNC_UPDATE_SITE              = true
    WEBSITE_RUN_FROM_PACKAGE                     = 1
    WEBSITES_ENABLE_APP_SERVICE_STORAGE          = true
    FUNCTIONS_WORKER_RUNTIME                     = "dotnet"
    LOCAL_EVENTS_SENDER_CONNECTION_STRING        = trimsuffix(module.sbtar_local_events_sender.primary_connection_string, ";EntityPath=${module.sbt_local_events.name}")
    LOCAL_EVENTS_LISTENER_CONNECTION_STRING      = trimsuffix(module.sbtar_local_events_listener.primary_connection_string, ";EntityPath=${module.sbt_local_events.name}")
    LOCAL_EVENTS_TOPIC_NAME                      = module.sbt_local_events.name
    LOCAL_EVENTS_SUBSCRIPTION_NAME               = azurerm_servicebus_subscription_rule.sbs-charge-transaction-received-filter.subscription_name
    CHARGE_DB_CONNECTION_STRING                  = local.CHARGE_DB_CONNECTION_STRING
  }
  dependencies                              = [
    module.appi.dependent_on,
    module.azfun_charges_validator_plan.dependent_on,
    module.azfun_charges_validator_stor.dependent_on,
    module.sbtar_local_events_listener.dependent_on,
    module.sbtar_local_events_sender.dependent_on,
    module.sbt_local_events.dependent_on,
  ]
}

module "azfun_charges_validator_plan" {
  source              = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//app-service-plan?ref=1.2.0"
  name                = "asp-validator-${var.project}-${var.organisation}-${var.environment}"
  resource_group_name = data.azurerm_resource_group.main.name
  location            = data.azurerm_resource_group.main.location
  kind                = "FunctionApp"
  sku                 = {
    tier  = "Free"
    size  = "F1"
  }
  tags                = data.azurerm_resource_group.main.tags
}

module "azfun_charges_validator_stor" {
  source                    = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//storage-account?ref=1.2.0"
  name                      = "stormsgrcvr${random_string.charges_validator.result}"
  resource_group_name       = data.azurerm_resource_group.main.name
  location                  = data.azurerm_resource_group.main.location
  account_replication_type  = "LRS"
  access_tier               = "Cool"
  account_tier              = "Standard"
  tags                      = data.azurerm_resource_group.main.tags
}

# Since all functions need a storage connected we just generate a random name
resource "random_string" "charges_validator" {
  length  = 6
  special = false
  upper   = false
}
