// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace IIoTPlatform_E2E_Tests.Deploy {
    using Microsoft.Azure.Devices;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TestExtensions;

    public abstract class DeploymentConfiguration : IIoTHubEdgeDeployment {

        public DeploymentConfiguration(IIoTPlatformTestContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<bool> CreateOrUpdateLayeredDeploymentAsync(CancellationToken token) {
            var deploymentConfiguration = new Configuration(DeploymentName) {
                Content = new ConfigurationContent {
                    ModulesContent = CreateDeploymentModules()
                },
                TargetCondition = TestConstants.TargetCondition,
                Priority = Priority
            };

            var configuration = await _context.RegistryHelper.CreateOrUpdateConfigurationAsync(
                deploymentConfiguration, true, DeploymentName, token);

            return configuration != null;
        }

        protected readonly IIoTPlatformTestContext _context;

        /// <summary>
        /// Create a deployment modules object
        /// </summary>
        protected abstract IDictionary<string, IDictionary<string, object>> CreateDeploymentModules();

        /// <summary>
        /// The desired rank of deployment
        /// </summary>
        protected abstract int Priority { get; }

        /// <summary>
        /// Identifier of deployment
        /// </summary>
        protected abstract string DeploymentName { get; }
    }
}
