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
            e.Attributes()
            |> Seq.tryFind (fun y -> y.Name.LocalName = "Include")
            >>= (fun y -> Some y.Value)
            >>? String.Empty
        and set (v:string) =
            (XName.Get("Include", e.Name.NamespaceName), v)
            |> e.SetAttributeValue

    member x.Data
        with get (name) = data.TryFind name
        and set (name) value =
            match value with
            | None -> ()
            | Some v -> data <- data.Add (name, v)

    member x.RemoveData (name) =
        data <- data.Remove name

        e.Elements()
        |> Seq.find (fun y -> y.Name.LocalName = name)
        |> (fun y -> y.Remove())