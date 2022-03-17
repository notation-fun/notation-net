[<AutoOpen>]
[<RequireQualifiedAccess>]
module Notation.GPX.Fun.Section

open AlphaTab.Model
open Notation.GPX

let toBar (id: string) (bar: Bar) : string List =
    [
        yield "} {"
        yield sprintf """    guitar [ "%s" | ]""" id 
    ]

let toBars (track : Track) (staff : Staff) : string List =
    staff
        |> Util.mapBars (fun bar ->
            let id = Util.barId ("pick") track staff bar
            toBar id bar
        )|> List.concat