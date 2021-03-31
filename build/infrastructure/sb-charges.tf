module "sbn_charges" {
  source              = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//service-bus-namespace?ref=1.0.0"
  name                = "sbn-${var.project}-${var.organisation}-${var.environment}"
  resource_group_name = data.azurerm_resource_group.main.name
  location            = data.azurerm_resource_group.main.location
  sku                 = "basic"
  tags                = data.azurerm_resource_group.main.tags
}

module "sbt_local_events" {
  source              = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//service-bus-topic?ref=1.0.0"
  name                = "sbt-local-events"
  namespace_name      = module.sbn_charges.name
  resource_group_name = data.azurerm_resource_group.main.name
  dependencies        = [module.sbn_charges]
}

module "sbtar_local_events_listener" {
  source                    = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//service-bus-topic-auth-rule?ref=1.0.0"
  name                      = "sbtar-local-events-listener"
  namespace_name            = module.sbn_charges.name
  resource_group_name       = data.azurerm_resource_group.main.name
  listen                    = true
  dependencies              = [module.sbn_charges]
  topic_name                = module.sbt_local_events.name
}

module "sbtar_local_events_sender" {
  source                    = "git::https://github.com/Energinet-DataHub/geh-terraform-modules.git//service-bus-topic-auth-rule?ref=1.0.0"
  name                      = "sbtar-local-events-sender"
  namespace_name            = module.sbn_charges.name
  resource_group_name       = data.azurerm_resource_group.main.name
  send                      = true
  dependencies              = [module.sbn_charges]
  topic_name                = module.sbt_local_events.name
}

resource "azurerm_servicebus_subscription" "sbs-local-events-charge-transaction-received-subscription" {
  name                = "sbs-local-events-charge-transaction-received-subscription"
  resource_group_name = data.azurerm_resource_group.main.name
  namespace_name      = module.sbn_charges.name
  topic_name          = module.sbt_local_events.name
  max_delivery_count  = 1
  dependencies        = [module.sbn_charges, module.sbt_local_events]
}

resource "azurerm_servicebus_subscription_rule" "sbs-local-events-charge-transaction-filter" {
  name                = "sbsr-local-events-charge-transaction-filter"
  resource_group_name = data.azurerm_resource_group.main.name
  namespace_name      = module.sbn_charges.name
  topic_name          = module.sbt_local_events.name
  subscription_name   = azurerm_servicebus_subscription.sbs-local-events-charge-transaction-received-subscription.name
  dependencies        = [module.sbn_charges, module.sbt_local_events]
  filter_type         = "CorrelationFilter"

  correlation_filter {
    subject = "ChargeTransactionReceived"
    }
  }
}