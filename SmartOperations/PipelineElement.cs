using System.Runtime.Serialization;
using Microsoft.CommerceServer.Runtime.Orders;

namespace SmartOperations
{
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
}