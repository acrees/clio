module Clio.PersistentProjectItem

open System
open System.Xml
open System.Xml.Linq

type PersistentProjectItem (e:XElement) =
    let mutable data =
        e.Elements()
        |> Seq.map (fun x -> (x.Name.LocalName, x.Value))
        |> (fun x -> Map x)

    member x.Node with get () = e
    member x.Action with get () = e.Name.LocalName

    member x.Include
        with get () = 
            let attrs =
                XName.Get("Include", e.Name.NamespaceName) |> e.Attributes
            if Seq.isEmpty attrs then ""
            else attrs |> Seq.head |> (fun x -> x.Value)
        and set (v:string) =
            if String.IsNullOrWhiteSpace v then
                raise <| new ArgumentException()
            (XName.Get("Include", e.Name.NamespaceName), v)
            |> e.SetAttributeValue

    member x.Data
        with get (name) = data.TryFind name
        and set (name) value =
            match value with
            | Some v ->
                data <- data.Add (name, v)
                let xn = XName.Get(name, e.Name.NamespaceName)
                let attrs = xn |> e.Elements
                if Seq.isEmpty attrs then new XElement(xn, v) |> e.AddFirst
                else attrs |> Seq.head |> (fun x -> x.Value <- v)
            | None ->
                data <- data.Remove name
                XName.Get(name, e.Name.NamespaceName)
                |> e.Elements
                |> (fun y -> y.Remove())