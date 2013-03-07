using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Microsoft.Commerce.Broker;
using Microsoft.Commerce.Common;
using Microsoft.Commerce.Contracts.Messages;
using Microsoft.Commerce.Providers;
using Microsoft.Commerce.Providers.Components;
using Microsoft.CommerceServer.Runtime.Orders;

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

	/// <summary>
	/// Simple type responsible for describing a Pipeline entry. 
	/// </summary>
	[DataContract]
    public class PipelineElement
    {
		[DataMember]
        public string Name { get; set; }
		[DataMember]
        public OrderPipelineType PipelineType { get; set; }
    }

    public class RuntimeOrderPipelinesProcessorConfiguration : OrderPipelinesProcessorConfiguration
    {
        protected RuntimeOrderPipelinesProcessorConfiguration(XNode xml)
        {
            using (var r = xml.CreateReader())
            {
                base.DeserializeElement(r, false);
            }
        }

        public static OrderPipelinesProcessorConfiguration Create(IList<PipelineElement> pipelineElements)
        {
            if (pipelineElements == null) throw new ArgumentNullException("pipelineElements");

            var config = new XElement("OrderPipelines",
										   pipelineElements.Select(pe =>
										   {
											   var xElement = new XElement("Pipeline");
											   xElement.SetAttributeValue(XName.Get("name"), pe.Name);
											   xElement.SetAttributeValue(XName.Get("type"), pe.PipelineType.ToString());
											   return (object)xElement;
										   }).ToArray()
								  );

			return new RuntimeOrderPipelinesProcessorConfiguration(config);
        }

        public static ConfigurationElement MaybeCreate(CommerceOperation commerceOperation)
        {
			//TODO try and create.
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Helper extensions that duplicate some internal behaviours of the Commerce Server operations.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static T RequireAs<T>(this ConfigurationElement configuration) where T : ConfigurationElement
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            var x = configuration as T;
            if (x != null) return x;

            var message = ProviderResources.ExceptionMessages.GetMessage("UnsupportedPipelineConfigurationType", new object[]
            {
                configuration.GetType().ToString(),
                configuration.GetType(),
                typeof (T)
            });
            throw new ConfigurationErrorsException(message);
        }
    }
}