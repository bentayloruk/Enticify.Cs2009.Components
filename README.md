Enticify.Cs2009.Components
==========================

Got Basket MessageHandler sections in your ChannelConfiguration.config that use the OrderPipelinesProcessor operation?  Got it nicely configured with you tasty PCF files like basket.pcf, total.pcf and allmyoldjunk.pcf?  Ever wish you could change the PCFs at runtime without having duplicate MessageHandler sections (you know, to flip to some debug PCF versions or something)?  Yes?  Well this is for you.

**Pick a suffix string**

I'm going to use **_primary** as the suffix for the **pipeline names** that I want to run *most of the time*.
 
**Configure your primary and secondary pipelines**

```xml
<pipeline name="basket_primary" path="basket_primary.PCF" type="OrderPipeline" />
<pipeline name="basket" path="basket.pcf" type="OrderPipeline" />
<pipeline name="total_primary" path="total_primary.pcf" type="OrderPipeline" />
<pipeline name="total" path="total.pcf" type="OrderPipeline" />
```xml

*Logging and transaction attributes removed for brevity*
