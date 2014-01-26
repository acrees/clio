[<RequireQualifiedAccess>]
module Clio.Core

open System
open System.IO
open System.Xml
open System.Xml.Linq

open Clio.ProjectItems

let show p path full = Project.find p >>= Project.load >>| (fun prj ->
    if full then prj.Print path else prj.PrintShallow path)

let contains p (file:string) =
    Project.find p >>= Project.load >>| (fun prj ->
        if prj.Contains file 
        then printfn "Yes, this file is indeed included in the project."
        else printfn "No, I'm afraid this file is not part of the project.")

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
