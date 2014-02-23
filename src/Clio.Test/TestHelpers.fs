[<AutoOpen>]
module Clio.Test.TestHelpers

open Xunit
open System

let should op x y = op x y |> Assert.True
let shouldnt op x y = op x y |> Assert.False

let be = (=)
let satisfy op v = op v = true

let have = satisfy
let value = Option.isSome

let contain v seq = Seq.exists (fun x -> x = v)

let throw e f =
    let mutable called = false
    try f() with | ex -> called <- ex.GetType().IsAssignableFrom(e)
    called