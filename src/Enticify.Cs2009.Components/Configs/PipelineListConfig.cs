using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components.Configs
{
    [DataContract]
    public class PipelineListConfig : ICreateOrderPipelinesConfig
    {
        [DataMember]
        public IList<PipelineConfigurationElementData> Pipelines { get; set; }

        public OrderPipelinesProcessorConfiguration Create(OrderPipelinesProcessorConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if(Pipelines == null) throw new InvalidOperationException("Pipelines is null.");
            if(Pipelines.Count == 0) throw new InvalidOperationException("Pipelines is an empty list.  You must specify at least one Pipeline to run.");

            var config = new XElement("OrderPipelines",
                                      Pipelines.Select(x =>
                                      {
                                          var xElement = new XElement("Pipeline");
                                          xElement.SetAttributeValue(XName.Get("name"), x.Name);
                                          xElement.SetAttributeValue(XName.Get("type"), x.Type);
                                          return (object) xElement;
                                      }).ToArray());

            return new RuntimeOrderPipelinesProcessorConfiguration(config);
        }
    }
}
