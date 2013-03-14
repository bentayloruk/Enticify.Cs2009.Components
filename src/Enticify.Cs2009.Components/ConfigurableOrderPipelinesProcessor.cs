using System;
using System.Configuration;
using Microsoft.Commerce.Application.Common.Configuration;
using Microsoft.Commerce.Broker;
using Microsoft.Commerce.Contracts;
using Microsoft.Commerce.Contracts.Messages;
using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components
{
    /// <summary>
    /// Responsible for allowing a developer to specify the pipelines to run as part of the CommerceOperation.
    /// When present, the alternate configuration is used rather than what is in ChannelConfiguration.config.
    /// </summary>
    public class ConfigurableOrderPipelinesProcessor : OrderPipelinesProcessor, IConfigurable
    {
        private OrderPipelinesProcessorConfiguration _realConfiguration;
        private const string ModelKey = "PipelineConfigurationData";

        public new void Configure(ConfigurationElement configuration)
        {
            //Store the .config configuration section for us to maybe created modified version later (if requested on the model).
            //Note, we had to implement IConfigurable as well (despite it being implemented on our base class) or this would not be called.
            _realConfiguration  = configuration as OrderPipelinesProcessorConfiguration;
            base.Configure(configuration);
        }

        public override void Execute(CommerceOperation operation, OperationCacheDictionary operationCache, CommerceOperationResponse response)
        {
            if (operation == null) throw new ArgumentNullException("operation");
            var maybeConfigCreator = TryGetConfigCreator(operation);
            if (maybeConfigCreator != null)
            {
                //Configure will have been called with the "real" ConfigurationElement from the ChannelConfiguration.config.
                //We "re-Configure" if this key is present in the operation.Model.
                if(_realConfiguration == null)
                    throw new InvalidOperationException("Configure should have been called before an Execute.");

                base.Configure(maybeConfigCreator.Create(_realConfiguration));
            }
            base.Execute(operation, operationCache, response);
        }

        protected virtual ICreateOrderPipelinesConfig TryGetConfigCreator(CommerceOperation commerceOperation)
        {
            return commerceOperation.Model.GetPropertyValue(ModelKey) as ICreateOrderPipelinesConfig;
        }

        static public void SetRuntimePipelineConfig(CommerceEntity model, ICreateOrderPipelinesConfig createOrderPipelinesConfig)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (createOrderPipelinesConfig == null) throw new ArgumentNullException("createOrderPipelinesConfig");
            model.SetPropertyValue(ModelKey, createOrderPipelinesConfig);
        }
    }
}