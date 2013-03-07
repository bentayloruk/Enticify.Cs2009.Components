using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Commerce.Contracts.Messages;
using Microsoft.Commerce.Providers.Components;

namespace SmartOperations
{
    /// <summary>
    /// Responsible for making a valid OrderPipelinesProcessorConfiguration with pipelines we specify at runtime.
    /// This is a subclass of OrderPipelinesProcessorConfiguration so that we can call the protected DeserializeElement
    /// with our new XML structure. 
    /// </summary>
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
}