[<AutoOpen>]
module Clio.Utils

open System
open System.IO
open System.Text.RegularExpressions

let (>>=) opt f = match opt with | Some x -> f x | None -> None
let (>>|) opt f = match opt with | Some x -> f x | None -> ()

let ifNone opt f = match opt with | Some x -> Some x | None -> f (); None

let getRelativePath basePath cwd path =
    if String.IsNullOrWhiteSpace basePath then raise <| ArgumentException()
    if String.IsNullOrWhiteSpace cwd then raise <| ArgumentException()
    if String.IsNullOrWhiteSpace path then raise <| ArgumentException()

    let relCwd = if basePath = cwd then "" else cwd.Substring(basePath.Length)
    if path = "." then relCwd else Path.Combine(relCwd, path)