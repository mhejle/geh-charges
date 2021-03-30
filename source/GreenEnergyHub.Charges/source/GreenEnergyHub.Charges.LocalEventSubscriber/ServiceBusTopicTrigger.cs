// Copyright 2020 Energinet DataHub A/S
//
// Licensed under the Apache License, Version 2.0 (the "License2");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GreenEnergyHub.Charges.Domain.ChangeOfCharges.Transaction;
using GreenEnergyHub.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GreenEnergyHub.Charges.LocalEventSubscriber
{
    public static class ServiceBusTopicTrigger
    {
        private const string FunctionName = "ServiceBusTopicTrigger";

        [Function(FunctionName)]
        public static Task RunAsync(
            [ServiceBusTrigger("LOCAL_EVENTS_TOPIC_NAME", Connection = "LOCAL_EVENTS_LISTENER_CONNECTION_STRING")]
            string jsonSerializedQueueItem,
            [NotNull] FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(FunctionName);
            logger.LogInformation("Function {FunctionName} received a local event", FunctionName);

            // POC: Verify that we can receive an event from the service bus topic
            logger.LogDebug("Received JSON serialized event {Json}", jsonSerializedQueueItem);
            var jsonDeserializer = GetRequireService<IJsonSerializer>(executionContext);
            var transaction = jsonDeserializer.Deserialize<ChangeOfChargesTransaction>(jsonSerializedQueueItem);
            logger.LogDebug("Received event with charge type mRID '{mRID}'", transaction.ChargeTypeMRid);
            return Task.CompletedTask;
        }

        private static T GetRequireService<T>(FunctionContext functionContext)
            where T : notnull
        {
            return functionContext.InstanceServices.GetRequiredService<T>();
        }
    }
}
