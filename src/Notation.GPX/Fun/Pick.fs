[<AutoOpen>]
[<RequireQualifiedAccess>]
module Notation.GPX.Fun.Pick

open AlphaTab.Model
open Notation.GPX

let default_duration = Duration.Eighth

let toPickString (note : Note) : string =
    sprintf "%i@%i" (7 - (Util.indexId note.String)) (Util.indexId note.Fret)    

let toDurationTweak (duration : Duration) : string =
    let mutable segments = []
    let duration = unbox<int>(duration)
    let base_duration = unbox<int>(default_duration)
    if duration < base_duration then
        let mutable scale = base_duration / duration
        while scale > 1 do
            segments <- "*" :: segments
            scale <- scale / 2
    elif duration > base_duration then
        let mutable scale = duration / base_duration
        while scale > 1 do
            segments <- "," :: segments
            scale <- scale / 2
    segments |> String.concat ""

let toMultiPick (beat : Beat) (notes : Note list) : string list =
    [
        yield "("
        for note in notes do
            yield toPickString note
        if beat.Duration <> default_duration then
            yield toDurationTweak beat.Duration
        yield ")"
    ]

let toSinglePick (beat : Beat) (note : Note) : string list =
    if beat.Duration <> default_duration then
        toMultiPick beat [ note ]
    else
        [ 
            yield toPickString note
        ]

let toBar (id: string) (bar: Bar) : string list =
    let picks =
        bar
            |> Util.mapBarBeats (fun beat ->
                let notes = Util.stringBeatNotes beat
                match notes.Length with
                | 0 -> []
                | 1 -> toSinglePick beat (notes[0])
                | _ -> toMultiPick beat notes
            )|> List.concat
    let line =
        [
            yield sprintf "\"%s\"" id
            yield "Pick"
            yield "["
        ] @ picks @ [
            yield "]"
            yield "|"
        ]
        |> String.concat " "
    [ 
        yield line 
    ]

let toBars (track : Track) (staff : Staff) : string List =
    staff
        |> Util.mapBars (fun bar ->
            let id = Util.barId ("pick") track staff bar
            toBar id bar
        )|> List.concat