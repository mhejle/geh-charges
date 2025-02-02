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
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using GreenEnergyHub.Charges.Application.ChangeOfCharges;
using GreenEnergyHub.Charges.Domain.ChangeOfCharges.Transaction;
using GreenEnergyHub.Charges.Tests.Builders;
using GreenEnergyHub.TestHelpers;
using GreenEnergyHub.TestHelpers.Traits;
using Moq;
using Xunit;

namespace GreenEnergyHub.Charges.Tests.Application.ChangeOfCharges
{
    [Trait(TraitNames.Category, TraitValues.UnitTest)]
    public class ChangeOfChargesMessageHandlerTests
    {
        [Theory]
        [InlineAutoDomainData]
        public async Task HandleAsync_WhenCalledWithMultipleTransactions_ShouldCallMultipleTimes(
            [NotNull] [Frozen] Mock<IChangeOfChargesTransactionHandler> changeOfChargesTransactionHandler,
            [NotNull] ChangeOfChargesMessageHandler sut)
        {
            // Arrange
            var transactionBuilder = new ChangeOfChargesTransactionBuilder();
            var changeOfChargesMessage = new ChangeOfChargesMessageBuilder()
                .WithTransaction(transactionBuilder.Build())
                .WithTransaction(transactionBuilder.Build())
                .WithTransaction(transactionBuilder.Build())
                .Build();

            // Act
            var response = await sut.HandleAsync(changeOfChargesMessage).ConfigureAwait(false);

            // Assert
            changeOfChargesTransactionHandler
                .Verify(v => v.HandleAsync(It.IsAny<ChangeOfChargesTransaction>()), Times.Exactly(3));
            response.IsSucceeded.Should().BeTrue();
        }
    }
}
