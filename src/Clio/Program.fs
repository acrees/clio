module Clio.Main

open System
open System.IO
open System.Text.RegularExpressions

exception MissingArgument of unit

[<EntryPoint>]
let main argv =
    let fail () = raise <| InvalidOperationException()

    let r = new Regex(@"^-\w.*")
    let (opts, args) = argv |> List.ofArray |> List.partition (r.IsMatch)
    let hasOption o = match opts with
                      | os::_ -> os |> Seq.exists (fun x -> x = o)
                      | [] -> false

    try
        match args with
        | [] | _::[] -> fail ()
        | d::_ when not <| Directory.Exists d -> raise <| DirectoryNotFoundException()
        | d::o::args ->
            let cwd = (new DirectoryInfo(d)).FullName
            match o with
            | "list" ->
                if hasOption 'p' && args.Length = 0
                then raise <| MissingArgument()
                else let isRecursive = hasOption 'r'
                     let path = if hasOption 'p' then args.[0] else d
                     Core.show cwd path isRecursive
            | "find" ->
                let children = hasOption 'c'
                let isRecursive = hasOption 'r'
                match args with
                | file::[] -> Core.find cwd file children isRecursive
                | _ -> fail ()
            | "add" ->
                match args with 
                | action::file::[] -> Core.add cwd action file
                | _ -> fail ()
            | _ -> fail ()
    with
        | :? InvalidOperationException -> printfn "I'm sorry - I couldn't understand that!"
        | :? DirectoryNotFoundException -> printfn "Unfortunately this directory doesn't appear to exist!"
        | :? MissingArgument -> printfn "I'm sorry, did you mean to add another argument?"
        | _ -> printfn "Oh, dear! Something really unusual happend."
    0
