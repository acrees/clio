module Clio.Test.Utils

open Xunit
open Clio

[<Fact>]
let ``None is the bind identitiy`` () =
    None >>= (fun x -> Some 0) |> Option.isNone |> Assert.True