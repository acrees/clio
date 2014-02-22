[<RequireQualifiedAccessAttribute>]
module Clio.Trie

type Node<'a> = Node of 'a option * Map<string, Node<'a>>
let empty = Node (None, Map.empty)

let isEmpty = function | Node (None, m) -> m.IsEmpty | _ -> false

let add k v t = 
  let rec add' k' n =
    match (k', n) with
    | [], Node (_, m) -> Node (Some v, m)
    | x::xs, Node (a, m) ->
      let t' = match m.TryFind x with
               | None -> empty
               | Some n' -> n'
               |> add' xs
      Node (a, m.Add(x, t'))
  add' k t
    
let rec remove k t =
  match (k, t) with
  | [], Node (_, m) -> Node (None, m)
  | x::xs, Node (v, m) ->
    match m.TryFind x with
    | None -> t
    | Some n ->
      let t' = remove xs n
      let m' = if t' = empty then m.Remove x else m.Add(x, t')
      Node (v, m')

let rec find k t =
  match (k, t) with
  | [], Node (None, _) -> None
  | [], Node (Some v, _) -> Some v
  | x::xs, Node (_, m) ->
    match m.TryFind x with | None -> None | Some n -> find xs n

let rec findn k t =
  match (k, t) with
  | [], node -> Some node
  | x::xs, Node (_, m) ->
    match m.TryFind x with | None -> None | Some n -> findn xs n
        
let rec contains k t =
  match find k t with
  | None -> false
  | Some _ -> true

let rec containsn k t =
  match findn k t with
  | None -> false
  | Some _ -> true
        
let rec map f =
  let mmap f = Map.map (fun k v -> f v)
  function
  | Node (None, m) -> Node (None, mmap (map f) m)
  | Node (Some v, m) -> Node (Some (f v), mmap (map f) m)
    
let rec iter f t =
  let rec traverse revp =
    function
    | Node (None, m) -> Map.iter (fun x -> traverse (x::revp)) m
    | Node (Some v, m) ->
      f (List.rev revp) v
      Map.iter (fun x t -> traverse (x::revp) t) m
  traverse [] t
            
let rec fold f acc t =
  let rec traverse acc key t = 
    let mf = Map.fold (fun a k v -> traverse a (k::key) v) acc
    match t with
    | Node (None, m) -> mf m
    | Node (Some v, m) -> f (mf m) (List.rev key) v
  traverse acc [] t
