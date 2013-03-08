using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Commerce.Providers.Components;

namespace SmartOperations
{
    /// <summary>
    /// Responsible for making a valid OrderPipelinesProcessorConfiguration with pipelines we specify at runtime.
    /// This is a subclass of OrderPipelinesProcessorConfiguration so that we can call the protected DeserializeElement
    /// with our new XML structure. 
    /// </summary>
    public class SmartOrderPipelinesProcessorConfiguration : OrderPipelinesProcessorConfiguration
    {
        protected SmartOrderPipelinesProcessorConfiguration(XNode xml)
        {
            using (var r = xml.CreateReader())
            {
                base.DeserializeElement(r, false);
            }
        }

        public static OrderPipelinesProcessorConfiguration Create(OrderPipelinesProcessorConfiguration configuration, Func<string,string> nameChanger)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (nameChanger == null) throw new ArgumentNullException("nameChanger");

            var config = new XElement("OrderPipelines",
                                      configuration.OrderPipelines.Cast<PipelineConfigurationElement>()
                                      .Select(pe =>
                                      {
                                          var xElement = new XElement("Pipeline");
                                          xElement.SetAttributeValue(XName.Get("name"), nameChanger(pe.PipelineName));
                                          xElement.SetAttributeValue(XName.Get("type"), pe.PipelineType);
                                          return (object)xElement;
                                      }).ToArray()
                );

            return new SmartOrderPipelinesProcessorConfiguration(config);
        }
    }
}