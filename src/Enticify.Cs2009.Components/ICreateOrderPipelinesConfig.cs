using Microsoft.Commerce.Providers.Components;

namespace Enticify.Cs2009.Components
{
    public interface ICreateOrderPipelinesConfig
    {
        OrderPipelinesProcessorConfiguration Create(OrderPipelinesProcessorConfiguration configuration);
    }
}