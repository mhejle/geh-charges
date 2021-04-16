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

using System;
using System.Linq;

namespace GreenEnergyHub.Charges.ApplyDBMigrationsApp.Helpers
{
    public static class EnvironmentFilter
    {
        public static Func<string, bool> GetPreDeployFilter(string[] args)
        {
            return file => file.EndsWith(".sql", StringComparison.InvariantCulture) &&
                           file.Contains(".Scripts.PreDeploy.") && args.Contains("includePreDeploy");
        }

        public static Func<string, bool> GetFilter(string[] args)
        {
            return file => file.EndsWith(".sql", StringComparison.InvariantCulture) &&
                            ((file.Contains(".Scripts.Seed.") && args.Contains("includeSeedData")) ||
                             (file.Contains(".Scripts.Test.") && args.Contains("includeTestData")) ||
                             file.Contains(".Scripts.Model."));
        }

        public static Func<string, bool> GetPostDeployFilter(string[] args)
        {
            return file => file.EndsWith(".sql", StringComparison.InvariantCulture)
                           && file.Contains(".Scripts.PostDeploy.") && args.Contains("includePostDeploy");
        }
    }
}
