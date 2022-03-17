[<AutoOpen>]
[<RequireQualifiedAccess>]
module Notation.GPX.Util

open AlphaTab.Model

let iterStaffs (action : Track -> Staff -> unit) (score: Score) =
    for track in score.Tracks do
        for staff in track.Staves do
            action track staff

let mapStaffs<'item> (map : Track -> Staff -> 'item ) (score: Score) : 'item list =
    let mutable items = []
    for track in score.Tracks do
        for staff in track.Staves do
            items <- (map track staff) :: items
    List.rev items

let iterBars (action : Bar -> unit) (staff : Staff) =
    for bar in staff.Bars do
        action bar

let mapBars<'item> (map : Bar -> 'item) (staff : Staff) : 'item list =
    let mutable items = []
    for bar in staff.Bars do
        items <- (map bar) :: items
    List.rev items

let iterBarBeats (action : Beat -> unit) (bar : Bar) =
    for voice in bar.Voices do
        for beat in voice.Beats do
            action beat

let mapBarBeats<'item> (map : Beat -> 'item) (bar : Bar) : 'item list =
    let mutable items = []
    for voice in bar.Voices do
        for beat in voice.Beats do
            items <- (map beat) :: items
    List.rev items

let iterBeatNotes (action : Note -> unit) (beat : Beat) =
    for note in beat.Notes do
        action note

let mapBeatNotes<'item> (map : Note -> 'item) (beat : Beat) : 'item list =
    let mutable items = []
    for note in beat.Notes do
        items <- (map note) :: items
    List.rev items

let filterBeatNotes (check : Note -> bool) (beat : Beat) : Note list =
    let mutable items = []
    for note in beat.Notes do
        if check note then
            items <- note :: items
    List.rev items

let stringBeatNotes (beat : Beat) : Note list =
    filterBeatNotes (fun note ->
        note.IsStringed
    ) beat

let indexId (index : float) =
    System.Convert.ToInt32(index)

let barId (prefix : string) (track : Track) (staff : Staff) (bar : Bar) =
    sprintf "%s:%i-%i-%i" prefix (indexId track.Index) (indexId staff.Index) (indexId bar.Index)
