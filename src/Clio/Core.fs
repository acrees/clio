[<RequireQualifiedAccess>]
module Clio.Core

open System
open System.IO
open System.Xml
open System.Xml.Linq

open Clio.ProjectItem

let show cwd path full = Project.find cwd >>= Project.load >>| (fun prj ->
    let path' = getRelativePath prj.Dir cwd path
    if full then prj.PrintRecursive path' false else prj.PrintShallow path' false)

let find cwd (file:string) children isRecursive =
    Project.find cwd >>= Project.load >>| (fun prj ->
        let file' = getRelativePath prj.Dir cwd file
        if not <| prj.Contains file' then
            printfn "This file does not exist in the project."
        elif isRecursive then prj.PrintRecursive file' true
        elif children then prj.PrintShallow file' true
        else prj.Print file')

let add cwd (action:string) (file:string) =
    let processChanges (prj:Project.Project) f =
        let item = new ProjectItem()
        item.Action <- action
        item.Include <- f
        prj.Add item
        prj.Write ()
    
    if not <| File.Exists file then printfn "This file doesn't appear to exist."
    else Project.find cwd >>= Project.load >>| (fun prj -> 
        let file' = getRelativePath prj.Dir cwd file
        if prj.Contains file'
        then printfn "This file is already part of the project."
        else processChanges prj file')
