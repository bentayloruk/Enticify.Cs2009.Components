ConfigurableOrderPipelineProcessor
====================================

Wish you could change *at runtime* the PCF files that Commerce Server 2009 runs?  This would let you have **normal** PCF files and **debug** PCF files (for those times).

Primary pipelines are the ones you want to run 99% of the time.

Secondary pipelines are the ones we switch to at runtime (if requested).

### Installation

1.  Build this project.  *I'll make a Nuget if this works.  Until then, use the source.*
2.  Add a reference to **Enticify.Cs2009.Components**

### Example: Adding a secondary basket pipeline

1.  Make a copy of basket.pcf with your chosen suffix.  E.g. basket_topbanana.pcf.
2.  basket.pcf is now your secondary pipeline so change it as you want (e.g. add debug stuff).
2.  Add your new pipeline to the ChannelConfiguration.config pipelines section:  
```xml
<pipelines>
    <!-- Basket now has a primary and secondary -->
    <pipeline name="basket_topbanana" path="basket_banana.PCF" type="OrderPipeline" />
    <pipeline name="basket" path="basket.pcf" type="OrderPipeline" />

    <!-- Total is fine without a secondary -->
    <pipeline name="total" path="total.pcf" type="OrderPipeline" />
</pipelines>
```

Configure your **ChannelConfiguration.config** to have primary/secondary pipelines **where required**.  I'm going to set this up for basket only.


*Logging and transaction attributes removed for brevity*

### Update uses of OrderPipelineProcessor in your ChannelConfiguration.config

1.  Swap out CS' OrderPipelineProcessor for ConfigurableOrderPipelineProcessor.

```xml
<Component name="Order Pipelines Processor" type="Enticify.Cs2009.Components.ConfigurableOrderPipelinesProcessor, Enticify.Cs2009.Components, Version=0.1.0.0, Culture=neutral, PublicKeyToken=10ff57ed14d5fefa">
  ...
</Component>
```

2.  Then update the PCF names where you want to use a primary pipeline.

```xml
<Component name="Order Pipelines Processor" type="Enticify.Cs2009.Components.ConfigurableOrderPipelinesProcessor, Enticify.Cs2009.Components, Version=0.1.0.0, Culture=neutral, PublicKeyToken=10ff57ed14d5fefa">
  <Configuration
    customElementName="OrderPipelinesProcessorConfiguration"
    customElementType="Microsoft.Commerce.Providers.Components.OrderPipelinesProcessorConfiguration, Microsoft.Commerce.Providers, Version=1.0.0.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35">
    <OrderPipelinesProcessorConfiguration>
      <OrderPipelines>
        <Pipeline name="basket_topbanana" type="Basket"/>
        <Pipeline name="total" type="Total"/>
      </OrderPipelines>
    </OrderPipelinesProcessorConfiguration>
  </Configuration>

</Component>
```


### Tell us when to make the switch

So far, all this works as normal.  The pipelines in your config will run.


