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
        public override void Execute(CommerceOperation operation, OperationCacheDictionary operationCache, CommerceOperationResponse response)
        {
            MaybeConfigureFromOperation(operation);
            base.Execute(operation, operationCache, response);
        }

        public override void ExecuteCreate(CommerceCreateOperation createOperation, OperationCacheDictionary operationCache, CommerceCreateOperationResponse response)
        {
            MaybeConfigureFromOperation(createOperation);
            base.ExecuteCreate(createOperation, operationCache, response);
        }

        public override void ExecuteDelete(CommerceDeleteOperation deleteOperation, OperationCacheDictionary operationCache, CommerceDeleteOperationResponse response)
        {
            MaybeConfigureFromOperation(deleteOperation);
            base.ExecuteDelete(deleteOperation, operationCache, response);
        }

        public override void ExecuteQuery(CommerceQueryOperation queryOperation, OperationCacheDictionary operationCache, CommerceQueryOperationResponse response)
        {
            MaybeConfigureFromOperation(queryOperation);
            base.ExecuteQuery(queryOperation, operationCache, response);
        }

        public override void ExecuteUpdate(CommerceUpdateOperation updateOperation, OperationCacheDictionary operationCache, CommerceUpdateOperationResponse response)
        {
            MaybeConfigureFromOperation(updateOperation);
            base.ExecuteUpdate(updateOperation, operationCache, response);
        }

        private void MaybeConfigureFromOperation(CommerceOperation operation)
        {
            //Configure will have been called with the "real" ConfigurationElement from the ChannelConfiguration.config.
            //We "re-Configure" if we can create a new config from the operation.
            var config = RuntimeOrderPipelinesProcessorConfiguration.MaybeCreate(operation);
            if(config != null) Configure(config);
        }
    }
}