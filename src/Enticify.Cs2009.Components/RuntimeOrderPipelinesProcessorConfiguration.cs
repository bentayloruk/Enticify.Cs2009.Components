using System;
using System.Xml.Linq;
using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components
{
    /// <summary>
    /// Responsible for creating an <see cref="OrderPipelinesProcessorConfiguration"/> from some XML.
    /// Why?  Means we can switch the pipelines we use, without having to duplicated MessageHandler config in our XML.
    /// </summary>
    public class RuntimeOrderPipelinesProcessorConfiguration : OrderPipelinesProcessorConfiguration
    {
        public RuntimeOrderPipelinesProcessorConfiguration(XNode xml)
        {
            if (xml == null) throw new ArgumentNullException("xml");
            using (var r = xml.CreateReader())
            {
                base.DeserializeElement(r, false);
            }
        }
    }
}