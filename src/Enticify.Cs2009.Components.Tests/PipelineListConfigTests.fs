module PipelineListConfigTests

open Xunit
open FsUnit
open FsUnit.Xunit
open System.Xml
open System.Xml.Linq
open Enticify.Cs2009.Components
open Microsoft.CommerceServer.Runtime.Orders


[<Fact>]
let createsList () =
    
    let realConfig = parseConfigXml """<OrderPipelines></OrderPipelines>"""

    let pipelines = 
        toPipelineList 
            [
                "basket", OrderPipelineType.Basket
                "total", OrderPipelineType.Total
                "checkout", OrderPipelineType.Checkout
                "custom", OrderPipelineType.Custom
            ]

    let configMaker = PipelineListConfig(Pipelines = pipelines) :> ICreateOrderPipelinesConfig
    let newConfig = configMaker.Create(realConfig)

    [
        ("basket", OrderPipelineType.Basket)
        ("total", OrderPipelineType.Total)
        ("checkout", OrderPipelineType.Checkout)
        ("custom", OrderPipelineType.Custom)
    ]
    |> should equal newConfig.PipelineElements