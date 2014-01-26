[<AutoOpen>]
module Clio.Utils

open System
open System.IO

let (>>=) opt f = match opt with | Some x -> f x | None -> None
let (>>|) opt f = match opt with | Some x -> f x | None -> ()

let ifNone opt f = match opt with | Some x -> Some x | None -> f (); None
