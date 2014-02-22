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

[<Fact>]
let ``GetRelativePath throws when given an empty path`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "C:/path/c/" "") |> throwsArgumentException

[<Fact>]
let ``GetRelativePath throws when given an empty cwd`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "" "/x.fs") |> throwsArgumentException

[<Fact>]
let ``GetRelativePath returns path if same directory`` () =
    getRelativePath "C:/src/" "C:/src/" "file.fs" |> is "file.fs"

[<Fact>]
let ``GetRelativePath returns cwd for .`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "." |> is "x/y/"

[<Fact>]
let ``GetRelativePath returns correct path for unmatched paths`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "z/file.fs" |> is "x/y/z/file.fs"