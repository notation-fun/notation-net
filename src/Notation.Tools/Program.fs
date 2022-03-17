open Notation.GPX
open Notation.GPX.Fun

loadScore "tabs/time-after-time.gp5"
    //|> Util.mapStaffs Section.toBars
    |> Util.mapStaffs Pick.toBars
    |> List.concat
    |> List.iter (fun line ->
        printfn "%s" line
    )


