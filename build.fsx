#I @".\src\packages\FAKE.1.64.7\tools\"
#r @".\src\packages\FAKE.1.64.7\tools\FakeLib.dll"
(*
#I @".\src\packages\FAKE.2.1.155-alpha\tools"
#r @".\src\packages\FAKE.2.1.155-alpha\tools\FakeLib.dll"
*)

open System
open Fake
//open Fake.AssemblyInfoFile


let version = "0.1.0"
let buildOutputPath = @".\build\output"
let buildDirs = [ buildOutputPath; ]

let projFiles =
    !+ @".\src\**\.csproj"
    -- @".\src\**\*Tests.csproj"
    |> Scan

Target "Clean" (fun _ -> CleanDirs buildDirs)

//Build
Target "BuildProjects" (fun _ ->
    MSBuild buildOutputPath "Build" ["Configuration", "Release"]  projFiles
    |> Log "BuildOutput:"
)

(*
Target "AssInfo" (fun _ ->
    CreateCSharpAssemblyInfo @".\src\Enticify.Cs2009.Components\Properties\AssemblyInfo.cs"
        [
            Attribute.Title "Enticify.Cs2009.Components"
            Attribute.Product "Enticify Components for Commerce Server 2009."
            Attribute.Version version
            Attribute.FileVersion version
            Attribute.Guid "BA5DBFF6-A308-4324-AD3C-A33BE0844A68"
        ]
)
*)

"Clean"
//   ==> "AssInfo"
    ==> "BuildProjects" 

Run "BuildProjects" 
