module Functions

open System.IO
open System.Text.Json
open Types
open JsonConverter

(* Load JSONs datas *)
let options = JsonSerializerOptions()
options.Converters.Add(DateTimeConverter())

let loadJson<'T> (filePath: string) : 'T =
    let json = File.ReadAllText(filePath)
    JsonSerializer.Deserialize<'T>(json, options)


let pizzas: Pizza list = loadJson "../../../pizzas.json"
let orders: Order list = loadJson "../../../orders.json"

printfn "Liste des pizzas : %A" pizzas
printfn "Liste des commandes : %A" orders

(* 1/ *)
let countDistinctBases (pizzas: Pizza list) =
    pizzas |> List.map (fun pizza -> pizza.pizzaBase) |> List.distinct |> List.length


(* 2/ *)
let countTomatoBasedPizzas (pizzas: Pizza list) =
    pizzas |> List.filter (fun pizza -> pizza.pizzaBase = "Tomate") |> List.length


(* 3/ *)
let countDistinctIngredients (pizzas: Pizza list) =
    let allIngredients = pizzas |> List.collect (fun pizza -> pizza.ingredients)
    let distinctIngredients = allIngredients |> List.distinct
    printfn "Distinct Ingredients: %A" distinctIngredients
    distinctIngredients |> List.length


(* 4/ *)
let findUniqueIngredient (pizzas: Pizza list) =
    pizzas
    |> List.collect (fun pizza -> pizza.ingredients)
    |> List.groupBy id
    |> List.filter (fun (_, lst) -> List.length lst = 1)
    |> List.map fst


(* 5/ *)
let countPizzasWithLessThan4Ingredients (pizzas: Pizza list) =
    pizzas |> List.filter (fun pizza -> List.length pizza.ingredients < 4) |> List.length


(* 6/ *)
let findNeverSoldPizzas (pizzas: Pizza list) (orders: Order list) =
    let soldPizzaIds = orders |> List.collect (fun order -> order.items |> List.map (fun item -> item.pizzaId)) |> List.distinct
    pizzas |> List.filter (fun pizza -> not (List.contains pizza.id soldPizzaIds))


(* 7/ *)
let calculateAverageOrderAmount (orders: Order list) =
    let totalAmount = orders |> List.sumBy (fun order -> order.totalAmount)
    totalAmount / float(List.length orders)


(* 8/ *)
let calculateTomatoPizzaPriceAverage (pizzas: Pizza list) =
    let tomatoPizzas = pizzas |> List.filter (fun pizza -> pizza.pizzaBase = "Tomate")
    let totalPrice = tomatoPizzas |> List.sumBy (fun pizza -> pizza.price)
    totalPrice / float(List.length tomatoPizzas)


(* 9/ *)
let countVegetarianPizzas (pizzas: Pizza list) =
    let meatIngredients = ["Jambon Cuît"; "Saucisson Piquant"; "Poulet"; "Jambon Cru"]
    pizzas
    |> List.filter (fun pizza -> pizza.ingredients |> List.forall (fun ingredient -> not (List.contains ingredient meatIngredients)))
    |> List.length


(* 10/ *)
let findMostSoldPizza (pizzas: Pizza list) (orders: Order list) =
    let pizzaSales =
        orders
        |> List.collect (fun order -> order.items)
        |> List.groupBy (fun item -> item.pizzaId)
        |> List.map (fun (pizzaId, items) -> pizzaId, List.sumBy (fun item -> item.quantity) items)
    let mostSoldPizzaId = pizzaSales |> List.maxBy snd |> fst
    pizzas |> List.find (fun pizza -> pizza.id = mostSoldPizzaId)


(* 11/ *)
let calculateAveragePizzasPerOrder (orders: Order list) =
    let totalPizzas = orders |> List.sumBy (fun order -> order.items |> List.sumBy (fun item -> item.quantity))
    totalPizzas / List.length orders


(* 12/ *)
let findUnusedIngredients (pizzas: Pizza list) (orders: Order list) =
    let usedPizzaIds = orders |> List.collect (fun order -> order.items |> List.map (fun item -> item.pizzaId)) |> List.distinct
    let usedIngredients = pizzas |> List.filter (fun pizza -> List.contains pizza.id usedPizzaIds) |> List.collect (fun pizza -> pizza.ingredients) |> List.distinct
    let allIngredients = pizzas |> List.collect (fun pizza -> pizza.ingredients) |> List.distinct
    allIngredients |> List.filter (fun ingredient -> not (List.contains ingredient usedIngredients))


(* 13/ *)
let countPizzasOrderedOnce (orders: Order list) =
    orders
    |> List.collect (fun order -> order.items)
    |> List.groupBy (fun item -> item.pizzaId)
    |> List.filter (fun (_, items) -> List.sumBy (fun item -> item.quantity) items = 1)
    |> List.length


(* 14/ *)
let calculateAveragePreparationTime (orders: Order list) =
    let totalMinutes = orders |> List.sumBy (fun order -> (order.readyAt - order.orderedAt).TotalMinutes)
    totalMinutes / float(List.length orders)


(* 15/ *)
let calculateAverageDeliveryCostsForTakeaway (orders: Order list) =
    let takeawayOrders = orders |> List.filter (fun order -> order.orderType = "Delivery")

    if List.isEmpty takeawayOrders then
        0.0
    else
        let totalCosts = takeawayOrders |> List.sumBy (fun order -> order.deliveryCosts)
        totalCosts / float(List.length takeawayOrders)




(* 16/ *)
let findMostSoldPizzaInTakeawayOrders (pizzas: Pizza list) (orders: Order list) =
    let takeawayOrders = orders |> List.filter (fun order -> order.orderType = "Delivery")
    let pizzaSales =
        takeawayOrders
        |> List.collect (fun order -> order.items)
        |> List.groupBy (fun item -> item.pizzaId)
        |> List.map (fun (pizzaId, items) -> pizzaId, List.sumBy (fun item -> item.quantity) items)
    if List.isEmpty pizzaSales then
        None
    else
        let mostSoldPizzaId = pizzaSales |> List.maxBy snd |> fst
        pizzas |> List.tryFind (fun pizza -> pizza.id = mostSoldPizzaId)


