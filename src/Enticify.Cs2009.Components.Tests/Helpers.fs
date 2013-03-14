[<AutoOpen>]
module Helpers

open Xunit
open System.Xml
open System.Xml.Linq
open Enticify.Cs2009.Components
open Enticify.Cs2009.Components.Configs
open System.Collections.Generic
open Microsoft.Commerce.Providers.Components

type OrderPipelinesProcessorConfiguration with
    member __.PipelineElements with get() = 
        __.OrderPipelines
        |> Seq.cast<PipelineConfigurationElement>
        |> Seq.map (fun x -> (x.PipelineName, x.PipelineType))
        |> List.ofSeq
    
let parseConfigXml xml = 
    let xml = XElement.Parse(xml)
    RuntimeOrderPipelinesProcessorConfiguration(xml)

let toPipelineList pipelines = 
    let pipelines = 
        pipelines
        |> Seq.map (fun (name, pipelineType) -> PipelineConfigurationElementData(name, pipelineType)) 
    List<PipelineConfigurationElementData>(pipelines)

    