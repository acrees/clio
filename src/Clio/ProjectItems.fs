module Clio.ProjectItem

open System
open System.Xml
open System.Xml.Linq
open Clio.PersistentProjectItem

type ProjectItem () =
    member val Action = "" with get, set
    member val Include = ""  with get, set
    member val Data = Map.empty<string, string> with get, set
    member x.Persist (ns:string) =
        let e = XElement(XName.Get(x.Action, ns))
        let item = PersistentProjectItem(e)
        item.Include <- x.Include
        x.Data |> Map.iter (fun k v -> item.Data(k) <- Some v)
        item