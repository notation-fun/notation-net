(* FAKE: 5.22.0 *)
#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.IO.Globbing.Operators

module DotNet = Dap.Build.DotNet
module NuGet = Dap.Build.NuGet

let feed =
    NuGet.Feed.Create (
        apiKey = NuGet.Environment "API_KEY_nuget_org"
    )

let libProjects =
    !! "src/Notation.GPX/*.fsproj"

let allProjects =
    libProjects
    ++ "src/Notation.Tools/*.fsproj"

let options = NuGet.mixed libProjects

DotNet.create options allProjects

NuGet.extend options feed libProjects

Target.runOrDefault DotNet.Build
