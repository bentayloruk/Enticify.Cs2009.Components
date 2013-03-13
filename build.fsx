(*
#I @".\src\packages\FAKE.2.1.155-alpha\tools"
#r @".\src\packages\FAKE.2.1.155-alpha\tools\FakeLib.dll"
*)
#I @".\src\packages\FAKE.1.64.7\tools"
#r @".\src\packages\FAKE.1.64.7\tools\FakeLib.dll"

open System
open Fake

let version = "0.5.0"
let buildOutputPath = @".\build\output"
let nugetOutputPath = @".\build\packages"
let buildDirs = [ buildOutputPath; nugetOutputPath; ]
let packageDesc = "ConfigurableOrderPipelinesProcessor Component for Commerce Server 2009."

let projFiles =
    !+ @".\src\**\*.csproj"
        -- @".\src\**\*Tests.csproj"
            |> Scan

Target "Clean" (fun _ -> CleanDirs buildDirs)

//Build
Target "BuildProjects" (fun _ ->
    MSBuild buildOutputPath "Build" ["Configuration", "Release"]  projFiles
    |> Log "BuildOutput:"
)

Target "AssInfo" (fun _ ->
    AssemblyInfo 
        (fun p -> 
        {p with
            CodeLanguage = CSharp;
            AssemblyVersion = version;
            AssemblyTitle = "Enticify.Cs2009.Components";
            AssemblyCopyright = "Copyright Shape Factory Limited " + DateTime.Now.Year.ToString();
            AssemblyCompany = "Shape Factory Limited";
            AssemblyDescription = packageDesc;
            AssemblyProduct = "";
            AssemblyConfiguration = "Release"; 
            Guid = "95BBC977-73B1-4C86-8737-FAFF734E8F93";
            OutputFileName = @".\src\Enticify.Cs2009.Components\Properties\AssemblyInfo.cs"})
)

Target "Nuget" (fun _ ->

    let nuspecFileName = @".\build\nuget\the.nuspec"
    ensureDirectory (nugetOutputPath @@ "lib")
    XCopy (buildOutputPath @@ "Enticify.Cs2009.Components.dll") (nugetOutputPath @@ "lib")
    XCopy (buildOutputPath @@ "Enticify.Cs2009.Components.pdb") (nugetOutputPath @@ "lib")

    NuGet (fun p -> 
        {p with               
            ToolPath = @".\src\.nuget\nuget.exe"
            Authors = ["enticify"; "bentayloruk";]
            Project = "Enticify.Cs2009.Components"
            Version = version
            Dependencies = [] 
            Description = packageDesc 
            OutputPath = nugetOutputPath
            AccessKey = getBuildParamOrDefault "enticifynugetkey" ""
            Publish = false }) nuspecFileName
)

"Clean"
    ==> "AssInfo"
    ==> "BuildProjects" 
    ==> "Nuget"

Run "Nuget" 
