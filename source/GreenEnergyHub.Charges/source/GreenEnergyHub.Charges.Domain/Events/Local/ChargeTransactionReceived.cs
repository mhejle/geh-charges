﻿// Copyright 2020 Energinet DataHub A/S
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
using GreenEnergyHub.Charges.Domain.ChangeOfCharges.Transaction;
using NodaTime;

namespace GreenEnergyHub.Charges.Domain.Events.Local
{
    public class ChargeTransactionReceived : ILocalEvent
    {
        public ChargeTransactionReceived(
            string correlationId,
            [NotNull] ChangeOfChargesTransaction transaction)
        {
            CorrelationId = correlationId;
            Transaction = transaction;
            Filter = transaction.GetType().Name;
        }

        public Instant PublishedTime { get; } = SystemClock.Instance.GetCurrentInstant();

        public string CorrelationId { get; }

        public ChangeOfChargesTransaction Transaction { get; }

        public string Filter { get; }
    }
}
