open System.IO
open System.Text.Json
open Types
open Functions

// Load JSON data
let options = JsonSerializerOptions()
options.Converters.Add(JsonConverter.DateTimeConverter())

let loadJson<'T> (filePath: string) : 'T =
    let json = File.ReadAllText(filePath)
    JsonSerializer.Deserialize<'T>(json, options)

let pizzas: Pizza list = loadJson "../../../pizzas.json"
let orders: Order list = loadJson "../../../orders.json"

printfn "Liste des pizzas : %A" pizzas
printfn "Liste des commandes : %A" orders

printfn "1. Nombre de bases de pizzas différentes : %d" (countDistinctBases pizzas)
printfn "2. Nombre de recettes de pizzas à base de tomate : %d" (countTomatoBasedPizzas pizzas)
printfn "3. Nombre d'ingrédients distincts proposés : %d" (countDistinctIngredients pizzas)
printfn "4. Ingrédients présents dans une seule recette : %A" (findUniqueIngredient pizzas)
printfn "5. Nombre de recettes de pizzas avec moins de 4 ingrédients : %d" (countPizzasWithLessThan4Ingredients pizzas)
printfn "6. Recettes de pizzas jamais vendues : %A" (findNeverSoldPizzas pizzas orders)
printfn "7. Montant moyen des commandes de pizzas : %.2f" (calculateAverageOrderAmount orders)
printfn "8. Prix moyen des pizzas à base de tomate : %.2f" (calculateTomatoPizzaPriceAverage pizzas)
printfn "9. Nombre de recettes de pizzas végétariennes : %d" (countVegetarianPizzas pizzas)
printfn "10. Recette de pizza la plus vendue : %A" (findMostSoldPizza pizzas orders)
printfn "11. Nombre moyen de pizzas par commande : %d" (calculateAveragePizzasPerOrder orders)
printfn "12. Ingrédients non utilisés dans les pizzas vendues : %A" (findUnusedIngredients pizzas orders)
printfn "13. Nombre de recettes de pizzas commandées une seule fois : %d" (countPizzasOrderedOnce orders)
printfn "14. Durée moyenne de préparation d'une commande (en minutes) : %.2f" (calculateAveragePreparationTime orders)
printfn "15. Montant moyen des frais de livraison pour les commandes à emporter : %.2f" (calculateAverageDeliveryCostsForTakeaway orders)
printfn "16. Recette de pizza la plus vendue dans les commandes à emporter : %A" (findMostSoldPizzaInTakeawayOrders pizzas orders)
