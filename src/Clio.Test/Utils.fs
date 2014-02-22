module Clio.Test.Utils

open Xunit
open Clio

[<Fact>]
let ``None shouldBe the bind identitiy`` () =
    None >>= (fun x -> Some 0) |> shouldBeNone

[<Fact>]
let ``Some x bind f applies f to x`` () =
    Some 4 >>= (fun x -> Some x) |> (fun x -> x.Value) |> shouldBe 4

[<Fact>]
let ``None >>| f does not call f`` () =
    let value = ref None
    None >>| (fun x -> value := Some x)
    !value |> shouldBeNone

[<Fact>]
let ``Some x >>| f calls f with x`` () =
    let value = ref None
    Some 3 >>| (fun x -> value := Some x)
    value.Value.Value |> shouldBe 3

[<Fact>]
let ``Some x shouldBe the ifNone identity`` () =
    let value = ref None
    ifNone (Some 10) (fun x -> value := Some x) |> (fun x -> x.Value) |> shouldBe 10
    !value |> shouldBeNone

[<Fact>]
let ``None ifNone f calls f then returns None`` () =
    let value = ref None
    ifNone None (fun x -> value := Some ()) |> shouldBeNone
    value.Value.Value |> shouldBe ()

[<Fact>]
let ``GetRelativePath throws when given an empty path`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "C:/path/c/" "") |> shouldThrowArgumentException

[<Fact>]
let ``GetRelativePath throws when given an empty cwd`` () =
    (fun () -> ignore <| getRelativePath "C:/path/" "" "/x.fs") |> shouldThrowArgumentException

[<Fact>]
let ``GetRelativePath returns path if same directory`` () =
    getRelativePath "C:/src/" "C:/src/" "file.fs" |> shouldBe "file.fs"

[<Fact>]
let ``GetRelativePath returns cwd for .`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "." |> shouldBe "x/y/"

[<Fact>]
let ``GetRelativePath returns correct path for unmatched paths`` () =
    getRelativePath "C:/src/" "C:/src/x/y/" "z/file.fs" |> shouldBe "x/y/z/file.fs"