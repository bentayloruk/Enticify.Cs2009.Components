using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components
{
    /// <summary>
    /// Responsible for copying the existing pipeline config but removing the specified suffix from pipeline names that have it.
    /// E.g. if you set Suffix to _prod basket_prod becomes basket when.
    /// </summary>
    [DataContract]
    public class SuffixConfig : ICreateOrderPipelinesConfig
    {
        [DataMember]
        public string Suffix { get; set; }

        public OrderPipelinesProcessorConfiguration Create(OrderPipelinesProcessorConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            var config = new XElement("OrderPipelines",
                                      configuration.OrderPipelines.Cast<PipelineConfigurationElement>()
                                                   .Select(pe =>
                                                   {
                                                       var xElement = new XElement("Pipeline");
                                                       var name = pe.PipelineName;
                                                       if (name.EndsWith(Suffix, StringComparison.InvariantCultureIgnoreCase))
                                                       {
                                                           name = name.Substring(0, name.Length - Suffix.Length);
                                                       }
                                                       xElement.SetAttributeValue(XName.Get("name"), name);
                                                       xElement.SetAttributeValue(XName.Get("type"), pe.PipelineType);
                                                       return (object) xElement;
                                                   }).ToArray()
                );

            return new RuntimeOrderPipelinesProcessorConfiguration(config);
        }
    }
}