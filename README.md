ConfigurableOrderPipelineProcessor
====================================

Wish you could change *at runtime* the PCF files that Commerce Server 2009 runs?  This would let you have **normal** PCF files and **debug** PCF files (for those times).

OK.  Read on.

## Build this project

If this thing actually works, I'll make a Nuget.  Until then, use the source.

## Pick a suffix for your primary pipelines

Primary pipelines are the ones you want to run 99% of the time.  Secondary pipelines are the ones we switch to at runtime (if requested).

I'm going to use **_primary** as the suffix for the **pipeline names** that I want to run *most of the time*.
 
## Configure your primary and secondary pipelines

Configure your **ChannelConfiguration.config** to have primary/secondary pipelines **where required**.  I'm going to set this up for basket only.

```xml
<pipelines>
    <!-- I want basket to have Primary and Secondary -->
    <pipeline name="basket_primary" path="basket_primary.PCF" type="OrderPipeline" />
    <pipeline name="basket" path="basket.pcf" type="OrderPipeline" />

    <!-- Total is fine without a secondary -->
    <pipeline name="total" path="total.pcf" type="OrderPipeline" />
</pipelines>
```


*Logging and transaction attributes removed for brevity*
