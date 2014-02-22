[<AutoOpen>]
module Clio.Test.TestHelpers

open Xunit
open System

let isSome o = (match o with | None -> false | _ -> true) |> Assert.True
let isNone o = (match o with | None -> true | _ -> false) |> Assert.True

let is (x:'a) (y:'a) = Assert.Equal<'a>(x, y)

let throwsArgumentException f =
    let mutable called = false
    try f() with | :? ArgumentException -> called <- true
    Assert.True called