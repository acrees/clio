module Clio.Test.Utils

open Xunit
open Clio
open System

[<Fact>]
let ``None should be the bind identitiy`` () =
    None >>= (fun x -> Some 0) |> should be None

[<Fact>]
let ``Some x bind f applies f to x`` () =
    Some 4 >>= (fun x -> Some x) |> (fun x -> x.Value) |> should be 4

[<Fact>]
let ``None >>| f does not call f`` () =
    let value = ref None
    None >>| (fun x -> value := Some x)
    !value |> should be None

[<Fact>]
let ``Some x >>| f calls f with x`` () =
    let value = ref None
    Some 3 >>| (fun x -> value := Some x)
    value.Value.Value |> should be 3

[<Fact>]
let ``Some x should be the ifNone identity`` () =
    let value = ref None
    ifNone (Some 10) (fun x -> value := Some x)
    |> (fun x -> x.Value)
    |> should be 10
    !value |> should be None

[<Fact>]
let ``None ifNone f calls f then returns None`` () =
    let value = ref None
    ifNone None (fun x -> value := Some ()) |> should be None
    value.Value.Value |> should be ()

[<Fact>]
let ``GetRelativePath throws when given an empty path`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "C:/path/c/" "")
    |> should throw typeof<ArgumentException>

[<Fact>]
let ``GetRelativePath throws when given an empty cwd`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "" "/x.fs")
    |> should throw typeof<ArgumentException>

[<Fact>]
let ``GetRelativePath returns path if same directory`` () =
    getRelativePath "C:/src/" "C:/src/" "file.fs" |> should be "file.fs"

[<Fact>]
let ``GetRelativePath returns cwd for .`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "." |> should be "x/y/"

[<Fact>]
let ``GetRelativePath returns correct path for unmatched paths`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "z/file.fs"
    |> should be "x/y/z/file.fs"