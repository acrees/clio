module Clio.Test.Trie

open Xunit
open Clio

let getChild k = function | Trie.Node(None, m) -> Map.tryFind k m | _ -> None
let getValue = function | Trie.Node(v, _) -> v

[<Fact>]
let ``Empty trie is empty`` () =
    Trie.empty |> Trie.isEmpty |> shouldBe true

[<Fact>]
let ``Non-empty trie is not empty`` () =
    Trie.empty |> Trie.add ["a"] 0 |> Trie.isEmpty |> shouldBe false

[<Fact>]
let ``Adding an empty key sets the root`` () =
    Trie.empty
    |> Trie.add [] 0
    |> getValue
    |> shouldBe (Some 0)

[<Fact>]
let ``Adding to an existing node adds a direct child`` () =
    Trie.empty
    |> Trie.add ["a"] 0
    |> getChild "a" >>= getValue
    |> shouldBe (Some 0)

[<Fact>]
let ``Adding an existing value sets the value`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 0
    |> Trie.add ["a"] 1
    |> getChild "a" >>= getValue
    |> shouldBe (Some 1)

[<Fact>]
let ``Removing an empty key clears root`` () =
    Trie.empty
    |> Trie.add [] 0
    |> Trie.remove []
    |> getValue
    |> shouldBe None

[<Fact>]
let ``Removing a value doesn't remove it's children`` () =
    Trie.empty
    |> Trie.add [] 0
    |> Trie.add ["a"] 1
    |> Trie.remove []
    |> getChild "a" >>= getValue
    |> shouldBe (Some 1)

[<Fact>]
let ``Find returns nothing when key does not exist`` () =
    Trie.empty
    |> Trie.find ["a"]
    |> shouldBe None

[<Fact>]
let ``Find returns root's value for an empty key`` () =
    Trie.empty
    |> Trie.add [] 0
    |> Trie.find []
    |> shouldBe (Some 0)

[<Fact>]
let ``Find for an existing key returns it's value`` () =
    Trie.empty
    |> Trie.add ["a"] 1
    |> Trie.find ["a"]
    |> shouldBe (Some 1)

[<Fact>]
let ``Find returns None for a node with no value`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 1
    |> Trie.find ["a"]
    |> shouldBe None

[<Fact>]
let ``Findn returns nothing when key does not exist`` () =
    Trie.empty
    |> Trie.findn ["a"]
    |> shouldBe None

[<Fact>]
let ``Findn returns root's value for an empty key`` () =
    Trie.empty
    |> Trie.add [] 0
    |> Trie.findn [] >>= getValue
    |> shouldBe (Some 0)

[<Fact>]
let ``Findn for an existing key returns it's value`` () =
    Trie.empty
    |> Trie.add ["a"] 1
    |> Trie.findn ["a"] >>= getValue
    |> shouldBe (Some 1)

[<Fact>]
let ``Findn returns nodes without values`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 1
    |> Trie.findn ["a"]
    |> shouldBeSome

[<Fact>]
let ``Contains returns false when key does not exist`` () =
    Trie.empty
    |> Trie.contains ["a"]
    |> shouldBe false

[<Fact>]
let ``Contains returns true when the key does exist`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 0
    |> Trie.contains ["a"; "b"]
    |> shouldBe true

[<Fact>]
let ``Contains returns false for nodes without values`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 0
    |> Trie.contains ["a"]
    |> shouldBe false

[<Fact>]
let ``Containsn returns false when key does not exist`` () =
    Trie.empty
    |> Trie.containsn ["a"]
    |> shouldBe false

[<Fact>]
let ``Containsn returns true when the key does exist`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 0
    |> Trie.containsn ["a"; "b"]
    |> shouldBe true

[<Fact>]
let ``Containsn returns true for nodes without values`` () =
    Trie.empty
    |> Trie.add ["a"; "b"] 0
    |> Trie.containsn ["a"]
    |> shouldBe true

[<Fact>]
let ``Empty trie is the identity for map`` () =
    Trie.empty
    |> Trie.map (fun v -> v + 1)
    |> shouldBe Trie.empty

[<Fact>]
let ``Map applies function to values`` () =
    let result =
        Trie.empty
        |> Trie.add ["a"] 1
        |> Trie.add ["b"; "c"] 2
        |> Trie.add ["b"; "c"; "d"] 3
        |> Trie.map (fun v -> v + 1)
    Trie.find ["a"] result |> shouldBe (Some 2)
    Trie.find ["b"; "c"] result |> shouldBe (Some 3)
    Trie.find ["b"; "c"; "d"] result |> shouldBe (Some 4)

[<Fact>]
let ``Iter does nothing for empty tries`` () =
    let value = ref 0
    Trie.empty |> Trie.iter (fun _ _ -> value := !value + 1)
    !value |> shouldBe 0

[<Fact>]
let ``Iter calls function for each node with a value`` () =
    let keys = ref []
    let values = ref []
    Trie.empty
    |> Trie.add ["a"] 0
    |> Trie.add ["b"; "c"] 1
    |> Trie.add ["b"; "c"; "d"] 2
    |> Trie.iter (fun k v -> values := v :: !values; keys := k :: !keys)
    !keys |> List.sort |> shouldBe [["a"]; ["b"; "c"]; ["b"; "c"; "d"]]
    !values |> List.sort |> shouldBe [0; 1; 2]

[<Fact>]
let ``Empty trie is the identity for fold`` () =
    Trie.empty
    |> Trie.fold (fun acc key value -> acc + value) 12
    |> shouldBe 12

[<Fact>]
let ``Fold folds over every node with a value`` () =
    Trie.empty
    |> Trie.add ["a"] 10
    |> Trie.add ["b"; "c"] 15
    |> Trie.add ["b"; "c"; "d"] 20
    |> Trie.fold (fun (total, n) key value -> (total + value, n + 1)) (0, 0)
    |> shouldBe (45, 3)