module Clio.ProjectItems

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
            |> Seq.find (fun y -> y.Name.LocalName = "Include")
            |> (fun y -> y.Value)
        and set (v) =
            e.Attributes()
            |> Seq.find (fun y -> y.Name.LocalName = "Include")
            |> (fun y -> y.Value <- v)

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

type ProjectItem () =
    member val Action = "" with get, set
    member val Include = ""  with get, set
    member val Data = Map.empty with get, set
    member x.Persist (ns:string) =
        let e = XElement(XName.Get(x.Action, ns))
        let item = PersistentProjectItem(e)
        item.Include <- x.Include
        //?item.Data <- x.Data
        item
