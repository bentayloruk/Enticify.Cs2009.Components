module SuffixConfigTests

open Xunit
open FsUnit
open FsUnit.Xunit
open System.Xml
open System.Xml.Linq
open System.Collections.Generic
open Enticify.Cs2009.Components
open Enticify.Cs2009.Components.Configs
open Microsoft.Commerce.Providers.Components
open Microsoft.CommerceServer.Runtime.Orders

[<Fact>]
let removesSuffix () =

    let inputConfig = parseConfigXml """
          <OrderPipelines>
            <Pipeline name="basket_prod" type="Basket"/>
            <Pipeline name="total" type="Total"/>
          </OrderPipelines>"""

    let suffixConfig = SuffixConfig(Suffix = "_prod") :> ICreateOrderPipelinesConfig
    let newConfig = suffixConfig.Create(inputConfig)

    [
        ("basket", OrderPipelineType.Basket)
        ("total", OrderPipelineType.Total)
    ]
    |> should equal newConfig.PipelineElements