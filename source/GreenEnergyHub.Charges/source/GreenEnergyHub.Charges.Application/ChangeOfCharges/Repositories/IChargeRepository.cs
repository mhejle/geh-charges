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

using System.Threading.Tasks;
using GreenEnergyHub.Charges.Domain.ChangeOfCharges.Transaction;

namespace GreenEnergyHub.Charges.Application.ChangeOfCharges.Repositories
{
    /// <summary>
    /// Contract defining the capabilities of the infrastructure component facilitating interaction with the charges data store.
    /// </summary>
    public interface IChargeRepository
    {
        /// <summary>
        /// Used to find single a single Charge in the database
        /// </summary>
        /// <param name="mrid">The unique mrid known to external partners</param>
        /// <param name="chargeTypeOwnerMRid">Owner of the document</param>
        /// <returns>Returns a mapped version of the found database object, if not found it returns null<see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<ChangeOfChargesTransaction?> GetChargeAsync(string mrid, string chargeTypeOwnerMRid);

        /// <summary>
        /// Stores the given <see cref="ChangeOfChargesTransaction"/> in persistent storage.
        /// </summary>
        /// <param name="transaction">The transaction to be persisted.</param>
        /// <returns>A <see cref="ChargeStorageStatus"/> to indicate if the operation was performed successfully.</returns>
        Task<ChargeStorageStatus> StoreChargeAsync(ChangeOfChargesTransaction transaction);
    }
}
