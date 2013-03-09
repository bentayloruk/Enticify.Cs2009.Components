using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components
{
    /// <summary>
    /// Responsible for taking an existing OrderPipelinesProcessorConfiguration and returning a new one with the Pipeline
    /// names maybe modified.  This is a subclass of OrderPipelinesProcessorConfiguration specificatlly so that we can call
    /// the protected DeserializeElement with our new XML structure. 
    /// Why?  Means we can switch the pipelines we use, without having to duplicated MessageHandler config in our XML.
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

        /// <summary>
        /// Creates an OrderPipelinesProcessorConfiguration with the same number and type of pipelines as the provided
        /// <paramref name="configuration"/>, but with the names maybe modified using <paramref name="nameChanger"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="nameChanger"></param>
        /// <returns></returns>
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

            return new RuntimeOrderPipelinesProcessorConfiguration(config);
        }
    }
}