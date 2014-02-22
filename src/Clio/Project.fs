[<RequireQualifiedAccess>]
module Clio.Project 

open System
open System.IO
open System.Xml
open System.Xml.Linq

open Clio.ProjectItems

let split (s:string) = s.Split [|'/'|] |> List.ofArray

type NPPI = Trie.Node<PersistentProjectItem>

type Project (path:string, root:XElement) =
    let fileItemGroup () =
        root.Elements()
        |> Seq.filter (fun y -> y.Name.LocalName = "ItemGroup")
        |> Seq.last

    let mutable trie = Trie.empty
    do
        fileItemGroup().Elements()
        |> Seq.map (fun y -> PersistentProjectItem(y))
        |> Seq.iter (fun y -> trie <- Trie.add (split y.Include) y trie)

    member private x.FileItemGroup () = fileItemGroup()

    member private x.Printf dir level key (node:NPPI) =
        let pad = List.init level (fun _ -> " ") |> (fun x -> String.Join("", x))
        match node with
        | Trie.Node(None, _) ->
            if dir then printfn "%s/%s" pad key
            else printfn "%s- %s" pad key
        | Trie.Node(Some v, _) -> printfn "%s- %s (%s)" pad key v.Action

    member val Path = path with get
    member val Dir = (new FileInfo(path)).DirectoryName

    member x.Find k = Trie.findn (split k) trie
    member x.FindOrDefault k d = match x.Find k with | None -> d | Some n -> n
    member x.Contains k = Trie.containsn (split k) trie
    member x.Write () = File.WriteAllText(path, root.ToString())
    member x.Add (item:ProjectItem) =
        root.Name.NamespaceName
        |> item.Persist
        |> (fun y -> y.Node)
        |> x.FileItemGroup().Add

    member x.PrintShallow path parent =
        let node = x.FindOrDefault path trie
        let f k n = x.Printf false 0 k n
        if parent then f path node else ()
        match node with | Trie.Node(_, m) -> Map.iter f m

    member x.PrintRecursive path parent =
        let node = x.FindOrDefault path trie
        let rec traverse i k n =
            x.Printf true i k n
            match n with | Trie.Node(_, m) -> Map.iter (traverse (i + 1)) m
        let key = if parent then path else ""
        traverse 0 key node

    member x.Print path =
        let node = x.Find path
        match node with
        | None -> failwith "Could not find file"
        | Some n -> x.Printf true 0 path n

let rec find path =
    let getProjectFiles =
        Directory.EnumerateFiles >> Seq.tryFind (fun f -> f.EndsWith "proj")

    let recurse = Directory.GetParent >> (fun d -> d.FullName) >> find

    if path.EndsWith "proj" && File.Exists path then Some path
    else match getProjectFiles path with
         | Some y -> Some y 
         | None -> try recurse path with | _ -> None

let load (prj:string) =
    let e = XElement.Load prj
    Project(prj, e) |> Some
