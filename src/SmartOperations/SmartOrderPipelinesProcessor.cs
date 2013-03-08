using System;
using System.Configuration;
using Microsoft.Commerce.Broker;
using Microsoft.Commerce.Contracts.Messages;
using Microsoft.Commerce.Providers.Components;

namespace SmartOperations
{
    /// <summary>
    /// Responsible for allowing a developer to specify the pipelines to run as part of the CommerceOperation.
    /// When present, the alternate configuration is used rather than what is in ChannelConfiguration.config.
    /// </summary>
    public class SmartOrderPipelinesProcessor : OrderPipelinesProcessor
    {
        private OrderPipelinesProcessorConfiguration _realConfiguration;

        public new void Configure(ConfigurationElement configuration)
        {
            //Store the .config configuration section for us to use as base later.
            _realConfiguration  = configuration as OrderPipelinesProcessorConfiguration;
            base.Configure(configuration);
        }
        public override void Execute(CommerceOperation operation, OperationCacheDictionary operationCache, CommerceOperationResponse response)
        {
            AltConfigurePipelinesIfRequestedByOperation(operation);
            base.Execute(operation, operationCache, response);
        }

        public override void ExecuteCreate(CommerceCreateOperation createOperation, OperationCacheDictionary operationCache, CommerceCreateOperationResponse response)
        {
            AltConfigurePipelinesIfRequestedByOperation(createOperation);
            base.ExecuteCreate(createOperation, operationCache, response);
        }

        public override void ExecuteDelete(CommerceDeleteOperation deleteOperation, OperationCacheDictionary operationCache, CommerceDeleteOperationResponse response)
        {
            AltConfigurePipelinesIfRequestedByOperation(deleteOperation);
            base.ExecuteDelete(deleteOperation, operationCache, response);
        }

        public override void ExecuteQuery(CommerceQueryOperation queryOperation, OperationCacheDictionary operationCache, CommerceQueryOperationResponse response)
        {
            AltConfigurePipelinesIfRequestedByOperation(queryOperation);
            base.ExecuteQuery(queryOperation, operationCache, response);
        }

        public override void ExecuteUpdate(CommerceUpdateOperation updateOperation, OperationCacheDictionary operationCache, CommerceUpdateOperationResponse response)
        {
            AltConfigurePipelinesIfRequestedByOperation(updateOperation);
            base.ExecuteUpdate(updateOperation, operationCache, response);
        }

        private void AltConfigurePipelinesIfRequestedByOperation(CommerceOperation operation)
        {
            //Configure will have been called with the "real" ConfigurationElement from the ChannelConfiguration.config.
            //We "re-Configure" if this key is present in the operation.Model.
            if (!operation.Model.Properties.ContainsProperty("AltPipelineConfig")) return;

            if(_realConfiguration == null)
                throw new InvalidOperationException("Configure should have been called before an Execute.");

            //Change this func if you want to switch names with a different convention.
            Func<string, string> nameChanger = s =>
            {
                const string suffix = "_enticify";
                return !s.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase) ? s : s.Substring(0, s.Length - suffix.Length);
            };
            var config = SmartOrderPipelinesProcessorConfiguration.Create(_realConfiguration, nameChanger);
            Configure(config);
        }
    }
}