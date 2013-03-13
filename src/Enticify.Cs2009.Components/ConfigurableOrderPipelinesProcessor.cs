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
        private const string ModelKey = "EnticifyPrimaryPcfSuffix";
        private OrderPipelinesProcessorConfiguration _realConfiguration;

        public new void Configure(ConfigurationElement configuration)
        {
            //Store the .config configuration section for us to maybe created modified version later (if requested on the model).
            //Note, we had to implement IConfigurable as well (despite it being implemented on our base class) or this would not be called.
            _realConfiguration  = configuration as OrderPipelinesProcessorConfiguration;
            base.Configure(configuration);
        }

        public override void Execute(CommerceOperation operation, OperationCacheDictionary operationCache, CommerceOperationResponse response)
        {
            if(OperationConfiguredForSecondaryPipelines(operation))
                ConfigureForSecondaryPipelines(operation);
            base.Execute(operation, operationCache, response);
        }

        protected virtual bool OperationConfiguredForSecondaryPipelines(CommerceOperation operation)
        {
            return operation.Model.Properties.ContainsProperty(ModelKey);
        }

        protected virtual void ConfigureForSecondaryPipelines(CommerceOperation operation)
        {
            //Configure will have been called with the "real" ConfigurationElement from the ChannelConfiguration.config.
            //We "re-Configure" if this key is present in the operation.Model.
            if(_realConfiguration == null)
                throw new InvalidOperationException("Configure should have been called before an Execute.");

            var suffix = operation.Model.GetPropertyValue(ModelKey).ToString();

            if(String.IsNullOrEmpty(suffix))
                throw new InvalidOperationException(string.Format("You must specify a non-empty suffix for {0}.", ModelKey));

            //Change this func if you want to switch names with a different convention.
            Func<string, string> nameChanger = s => !s.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase) ? s : s.Substring(0, s.Length - suffix.Length);
            var config = RuntimeOrderPipelinesProcessorConfiguration.Create(_realConfiguration, nameChanger);
            base.Configure(config);
        }

        /// <summary>
        /// Configure the <paramref name="model"/> so that <see cref="ConfigurableOrderPipelinesProcessor"/> will strip the 
        /// <paramref name="primaryPcfSuffix"/> from any configured pipelines that end with the string and use the non-suffix
        /// versions when executing the pipelines.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="primaryPcfSuffix"></param>
        static public void UserSecondaryPipelinesWhereApplicable(CommerceEntity model, string primaryPcfSuffix)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (primaryPcfSuffix == null) throw new ArgumentNullException("primaryPcfSuffix");
            model.SetPropertyValue(ModelKey, primaryPcfSuffix);
        }

    }
}