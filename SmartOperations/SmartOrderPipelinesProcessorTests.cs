using System.Collections.Generic;
using System.Linq;
using Microsoft.Commerce.Providers.Components;
using Microsoft.CommerceServer.Runtime.Orders;
using Xunit;

namespace SmartOperations
{
    public class WhenRuntimeOrderPipelinesProcessorConfigurationCreate
    {
        private readonly OrderPipelinesProcessorConfiguration _config;

		public WhenRuntimeOrderPipelinesProcessorConfigurationCreate()
		{
            var pipelines = new List<PipelineElement>
            {
                new PipelineElement{Name = "bert", PipelineType = OrderPipelineType.Basket},
                new PipelineElement{Name = "ernie", PipelineType = OrderPipelineType.Total},
            };
            _config = RuntimeOrderPipelinesProcessorConfiguration.Create(pipelines);
		}

        [Fact]
        public void ShouldCreateTwoOrderPipelinesEntries()
        {
            Assert.Equal(2, _config.OrderPipelines.Count);
        }

        [Fact]
        public void ShouldBeOfCorrectTypes()
        {
            Assert.Equal(new[] {OrderPipelineType.Basket, OrderPipelineType.Total,}, _config.OrderPipelines.Cast<PipelineConfigurationElement>().Select(p => p.PipelineType));
        }

        [Fact]
        public void ShouldHaveCorrectNames()
        {
            Assert.Equal(new[] {"bert", "ernie"}, _config.OrderPipelines.Cast<PipelineConfigurationElement>().Select(p => p.PipelineName));
        }
    }
}