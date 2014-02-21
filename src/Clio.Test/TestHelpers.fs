[<AutoOpen>]
module Clio.Test.TestHelpers

open Xunit;

let isSome o = (match o with | None -> false | _ -> true) |> Assert.True
let isNone o = (match o with | None -> true | _ -> false) |> Assert.True

let is (x:'a) (y:'a) = Assert.Equal<'a>(x, y)