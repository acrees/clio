module Clio.Test.Utils

open Xunit
open Clio

[<Fact>]
let ``None is the bind identitiy`` () =
    None >>= (fun x -> Some 0) |> isNone

[<Fact>]
let ``Some x bind f applies f to x`` () =
    Some 4 >>= (fun x -> Some x) |> (fun x -> x.Value) |> is 4

[<Fact>]
let ``None >>| f does not call f`` () =
    let value = ref None
    None >>| (fun x -> value := Some x)
    !value |> isNone

[<Fact>]
let ``Some x >>| f calls f with x`` () =
    let value = ref None
    Some 3 >>| (fun x -> value := Some x)
    value.Value.Value |> is 3

[<Fact>]
let ``Some x is the ifNone identity`` () =
    let value = ref None
    ifNone (Some 10) (fun x -> value := Some x) |> (fun x -> x.Value) |> is 10
    !value |> isNone

[<Fact>]
let ``None ifNone f calls f then returns None`` () =
    let value = ref None
    ifNone None (fun x -> value := Some ()) |> isNone
    value.Value.Value |> is ()