using System;
using System.Runtime.Serialization;
using Microsoft.CommerceServer.Runtime.Orders;

namespace Enticify.Cs2009.Components.Configs
{
    [DataContract]
    public class PipelineConfigurationElementData
    {
        public PipelineConfigurationElementData(string name, OrderPipelineType type)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            Type = type;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public OrderPipelineType Type { get; set; }
    }
}
