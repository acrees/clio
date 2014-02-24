module Clio.Test.PersistentProjectItem

open Xunit
open Clio.PersistentProjectItem
open System
open System.Xml.Linq

[<Fact>]
let ``Keeps a reference to it's backing xml`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Node |> should be xe

[<Fact>]
let ``Action is the name of the xml node`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Action |> should be "Compile"

[<Fact>]
let ``Include defaults to the empty string`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Include |> should be ""

[<Fact>]
let ``Include returns the value of the attribute when it exists`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    (XName.Get("{myns}Include"), "src/file.fs") |> xe.SetAttributeValue
    let item = new PersistentProjectItem(xe)
    item.Include |> should be "src/file.fs"

[<Fact>]
let ``Setting include to null throws`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    (fun () -> item.Include <- null) |> should throw typeof<ArgumentException>

[<Fact>]
let ``Setting include to an empty string throws`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    (fun () -> item.Include <- "") |> should throw typeof<ArgumentException>

[<Fact>]
let ``Setting include to whitespace throws`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    (fun () -> item.Include <- "  ") |> should throw typeof<ArgumentException>

[<Fact>]
let ``Setting include sets the attribute if it does not exist`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Include <- "src/file.fs"
    XName.Get("{myns}Include")
    |> item.Node.Attributes
    |> Seq.head
    |> (fun x -> x.Value)
    |> should be "src/file.fs"

[<Fact>]
let ``Setting include overwrites existsing attribute`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    (XName.Get("{myns}Include"), "src/file.fs") |> xe.SetAttributeValue
    let item = new PersistentProjectItem(xe)
    item.Include <- "dir/newfile.fs"
    XName.Get("{myns}Include")
    |> item.Node.Attributes
    |> Seq.head
    |> (fun x -> x.Value)
    |> should be "dir/newfile.fs"

[<Fact>]
let ``Get data returns None when there is no matching items`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" |> shouldn't have value

[<Fact>]
let ``Get data returns matching node when it exists`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    new XElement(XName.Get("{myns}DependentUpon"), "file.fs") |> xe.AddFirst
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" |> should be (Some "file.fs")

[<Fact>]
let ``Setting data creates a node when new`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" <- Some "file.fs"
    XName.Get("{myns}DependentUpon")
    |> item.Node.Descendants
    |> Seq.head
    |> (fun n -> n.Value)
    |> should be "file.fs"

[<Fact>]
let ``Setting data overwrites existing elements`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    new XElement(XName.Get("{myns}DependentUpon"), "file.fs") |> xe.AddFirst
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" <- Some "newfile.fs"
    XName.Get("{myns}DependentUpon")
    |> item.Node.Descendants
    |> Seq.head
    |> (fun n -> n.Value)
    |> should be "newfile.fs"

[<Fact>]
let ``Setting data to none doesn't throw when it does not exist`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" <- Some "fils.fs"

[<Fact>]
let ``Setting data to none removes the element if it exists`` () =
    let xe = new XElement(XName.Get("{myns}Compile"))
    new XElement(XName.Get("{myns}DependentUpon"), "file.fs") |> xe.AddFirst
    let item = new PersistentProjectItem(xe)
    item.Data "DependentUpon" <- None
    XName.Get("{myns}DependentUpon")
    |> item.Node.Descendants
    |> should satisfy Seq.isEmpty