ConfigurableOrderPipelineProcessor
====================================

Wish you could change *at runtime* the PCF files that Commerce Server 2009 runs?  This would let you have **normal** PCF files and **debug** PCF files (for those times).

OK.  Read on.

**Build this project**

If this thing actually works, I'll make a Nuget.  Until then, use the source.

**Pick a suffix for your primary pipelines.**

Primary pipelines are the ones you want to run 99% of the time.

Secondary pipelines are the ones we switch to at runtime (if requested).

I'm using the suffix **_topbanana** for the primary pipeline names.
 
**Add Primary pipelines where you need them

Configure your **ChannelConfiguration.config** to have primary/secondary pipelines **where required**.  I'm going to set this up for basket only.

```xml
<pipelines>
    <!-- I want basket to have Primary and Secondary -->
    <pipeline name="basket_topbanana" path="basket_banana.PCF" type="OrderPipeline" />
    <pipeline name="basket" path="basket.pcf" type="OrderPipeline" />

    <!-- Total is fine without a secondary -->
    <pipeline name="total" path="total.pcf" type="OrderPipeline" />
</pipelines>
```

*Logging and transaction attributes removed for brevity*

**Swap out CS' OrderPipelineProcessor for ConfigurableOrderPipelineProcessor**

```xml
<Component
  name="Order Pipelines Processor"
  type="Enticify.Cs2009.Components.ConfigurableOrderPipelinesProcessor, Enticify.Cs2009.Components, Version=0.1.0.0, Culture=neutral, PublicKeyToken=10ff57ed14d5fefa">
  <Configuration
    customElementName="OrderPipelinesProcessorConfiguration"
    customElementType="Microsoft.Commerce.Providers.Components.OrderPipelinesProcessorConfiguration, Microsoft.Commerce.Providers, Version=1.0.0.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35">
    <OrderPipelinesProcessorConfiguration>
      <OrderPipelines>
        <Pipeline name="basket_primary" type="Basket"/>
        <Pipeline name="total_primary" type="Total"/>
      </OrderPipelines>
    </OrderPipelinesProcessorConfiguration>
  </Configuration>
</Component>
```
