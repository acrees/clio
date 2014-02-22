[<AutoOpen>]
module Clio.Test.TestHelpers

open Xunit
open System

let shouldBeSome o = (match o with | None -> false | _ -> true) |> Assert.True
let shouldBeNone o = (match o with | None -> true | _ -> false) |> Assert.True

let shouldBe (x:'a) (y:'a) = Assert.Equal<'a>(x, y)

let shouldThrowArgumentException f =
    let mutable called = false
    try f() with | :? ArgumentException -> called <- true
    Assert.True called