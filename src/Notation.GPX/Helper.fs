[<AutoOpen>]
module Notation.GPX.Helper

open AlphaTab.Model

let loadScore path =
    let data = System.IO.File.ReadAllBytes(path)
    let settings = AlphaTab.Settings()
    AlphaTab.Importer.ScoreLoader.LoadScoreFromBytes(data, settings)

let convertNote (track : Track) (staff : Staff) (bar : Bar) (voice : Voice) (beat : Beat) (note : Note) =
    printfn "\t\t\tconvertNote: %f:%f:%f IsStringed: %b" voice.Index beat.Index note.Index note.IsStringed
    if note.IsStringed then
        printfn "\t\t\t\tstring: %f fret: %f" note.String note.Fret

let convertBeat (track : Track) (staff : Staff) (bar : Bar) (voice : Voice) (beat : Beat) =
    printfn "\t\tconvertBeat: voice: %f beat: %f duration: %s" voice.Index beat.Index (beat.Duration.ToString()) 
    if beat.HasChord then
        let chord = beat.Chord
        printfn "\t\tChord: %s" chord.Name
    for note in beat.Notes do
        convertNote track staff bar voice beat note

let convertBar (track : Track) (staff : Staff) (bar : Bar) =
    printfn "\tconvertBar: [%f]" bar.Index
    for voice in bar.Voices do
        for beat in voice.Beats do
            convertBeat track staff bar voice beat

let convertStaff (track : Track) (staff : Staff) =
    printfn "convertStaff: track: [%f] %s staff: [%f] %s" track.Index track.Name staff.Index (staff.StringTuning.ToString())
    for bar in staff.Bars do
        convertBar track staff bar

let convertScore (score : Score) =
    printfn "Tempo: %f %s Title: %s" score.Tempo score.TempoLabel score.Title
    for track in score.Tracks do
        for staff in track.Staves do
            convertStaff track staff