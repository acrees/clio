module Clio.Main

open System
open System.Text.RegularExpressions

[<EntryPoint>]
let main argv =
    let fail () = printfn "I'm sorry - I couldn't understand that!"

    let r = new Regex(@"^-\w.*")
    let (opts, args) = argv |> List.ofArray |> List.partition (r.IsMatch)
    let hasOption o = match opts with
                      | os::_ -> os |> Seq.exists (fun x -> x = o)
                      | [] -> false

    match args with
    | [] | _::[] -> fail ()
    | d::o::args ->
        match o with
        | "list" ->
            if hasOption 'p' && args.Length = 0
            then printfn "I'm sorry, did you mean to add another argument?"
            else let full = hasOption 'f'
                 let path = if hasOption 'p' then args.[0] else d
                 Core.show d path full
        | "contains" -> match args with
                        | file::[] -> Core.contains d file
                        | _ -> fail ()
        | "add"      -> match args with 
                        | action::file::[] -> Core.add d action file
                        | _ -> fail ()
        | _          -> fail ()
    0
