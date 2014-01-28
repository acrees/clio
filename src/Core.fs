[<RequireQualifiedAccess>]
module Clio.Core

open System
open System.IO
open System.Xml
open System.Xml.Linq

open Clio.ProjectItems

let show p path full = Project.find p >>= Project.load >>| (fun prj ->
    if full then prj.PrintRecursive path false else prj.PrintShallow path false)

let find p (file:string) children recursive =
    Project.find p >>= Project.load >>| (fun prj ->
        if not <| prj.Contains file then
            printfn "This file does not exist in the project."
        elif recursive then prj.PrintRecursive file true
        elif children then prj.PrintShallow file true
        else prj.Print file)

let add p (action:string) (file:string) =
    let processChanges (prj:Project.Project) =
        let item = new ProjectItem()
        item.Action <- action
        item.Include <- file
        prj.Add item
        prj.Write ()
    
    if not <| File.Exists file then printfn "This file doesn't appear to exist."
    else Project.find p >>= Project.load >>| (fun prj -> 
        if prj.Contains file
        then printfn "This file is already part of the project."
        else processChanges prj)
