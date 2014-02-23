module Clio.Test.ProjectItem

open Xunit
open Clio.ProjectItem

[<Fact>]
let ``Persist preserves action and include`` () =
    let item = new ProjectItem ()
    item.Action <- "Compile"
    item.Include <- "src/dir/file.fs"
    let item' = item.Persist "ns"
    item'.Node.Name.NamespaceName |> should be "ns"
    item'.Action |> should be "Compile"
    item'.Include |> should be "src/dir/file.fs"

[<Fact>]
let ``Persist preserves data collection`` () =
    let item = new ProjectItem ()
    item.Action <- "Compile"
    item.Include <- "src/dir/file.fs"
    item.Data <- Map.add "DependentUpon" "parent.fs" item.Data
    let item' = item.Persist "ns"
    item'.Data "DependentUpon" |> should be (Some "parent.fs")