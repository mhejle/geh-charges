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

namespace GreenEnergyHub.Charges.Domain.ChangeOfCharges.Fee
{
    public class TestDomainModel
    {
        private string _name;
        private int _id;
        private List<Error> _erros;

        public TestDomainModel(string name, int id)
        {
            _name = name;
            _id = id;
        }

        public string name
        {
            get => _name;
            private set
            {
                _errors.add(_Rule501.Validate(value, _namee));

                _name = value;
            }
        }

        public int Id
        {
            get => _id;
            private set => _id = value;
        }
    }
}
