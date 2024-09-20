module Tests

open Expecto
open Types
open Functions
open System

let pizzas = [
    { id = "1"; name = "Margherita"; pizzaBase = "Tomate"; price = 10.0; ingredients = ["Tomate"; "Mozzarella"; "Basilic"] }
    { id = "2"; name = "Poulet Crème"; pizzaBase = "Crème"; price = 12.0; ingredients = ["Crème"; "Poulet"; "Champignons"] }
    { id = "3"; name = "Jambon"; pizzaBase = "Tomate"; price = 15.0; ingredients = ["Tomate"; "Mozzarella"; "Jambon Cuit"] }
    { id = "4"; name = "Pesto"; pizzaBase = "Pesto"; price = 13.0; ingredients = ["Pesto"; "Mozzarella"; "Tomates Confites"] }
]

let orders = [
    { id = "1"; orderedAt = DateTime.Now; readyAt = DateTime.Now.AddMinutes(15.0); orderType = "Delivery"; status = "Completed"; amount = 44.0; totalAmount = 44.0; deliveryCosts = 5.0; items = [{ pizzaId = "1"; quantity = 2; price = 10.0; amount = 20.0 }] }
    { id = "2"; orderedAt = DateTime.Now; readyAt = DateTime.Now.AddMinutes(20.0); orderType = "Take Away"; status = "Completed"; amount = 26.0; totalAmount = 26.0; deliveryCosts = 0.0; items = [{ pizzaId = "2"; quantity = 1; price = 12.0; amount = 12.0 }; { pizzaId = "3"; quantity = 1; price = 14.0; amount = 14.0 }] }
    { id = "3"; orderedAt = DateTime.Now; readyAt = DateTime.Now.AddMinutes(30.0); orderType = "Delivery"; status = "Completed"; amount = 15.0; totalAmount = 15.0; deliveryCosts = 3.0; items = [{ pizzaId = "4"; quantity = 1; price = 15.0; amount = 15.0 }] }
]


[<Tests>]
let tests = 
    testList "Pizza and Order Functions Tests" [

        // Test 1: Count Distinct Bases
        test "Count Distinct Bases" {
            let result = countDistinctBases pizzas
            Expect.equal result 3 "Should be 3 distinct bases"
        }

        // Test 2: Count Tomato-Based Pizzas
        test "Count Tomato-Based Pizzas" {
            let result = countTomatoBasedPizzas pizzas
            Expect.equal result 2 "Should be 2 pizzas with tomato base"
        }

        // Test 3: Count Distinct Ingredients
        test "Count Distinct Ingredients" {
            let result = countDistinctIngredients pizzas
            Expect.floatClose Accuracy.medium result 8 "Should be 8 distinct ingredients"
        }

        // Test 4: Find Unique Ingredient
        test "Find Unique Ingredient" {
            let result = findUniqueIngredient pizzas
            Expect.contains result "Basilic" "Should contain 'Basilic'"
            Expect.contains result "Poulet" "Should contain 'Poulet'"
        }

        // Test 5: Count Pizzas with Less than 4 Ingredients
        test "Count Pizzas with Less than 4 Ingredients" {
            let result = countPizzasWithLessThan4Ingredients pizzas
            Expect.floatClose Accuracy.medium result 4 "Should be 4 pizzas with less than 4 ingredients"
        }

        // Test 6: Find Never Sold Pizzas
        test "Find Never Sold Pizzas" {
            let result = findNeverSoldPizzas pizzas orders
            Expect.isEmpty result "Should be no pizzas that were never sold"
        }

        // Test 7: Calculate Average Order Amount
        test "Calculate Average Order Amount" {
            let result = Math.Round(calculateAverageOrderAmount orders, 2)
            Expect.floatClose Accuracy.medium result 28.33 "Should be 28.33"
        }

        // Test 8: Calculate Tomato Pizza Price Average
        test "Calculate Tomato Pizza Price Average" {
            let result = calculateTomatoPizzaPriceAverage pizzas
            Expect.floatClose Accuracy.medium result 12.5 "Should be 12.5 average price for tomato-based pizzas"
        }

        // Test 9: Count Vegetarian Pizzas
        test "Count Vegetarian Pizzas" {
            let result = countVegetarianPizzas pizzas
            Expect.floatClose Accuracy.medium result 2 "Should be 2 vegetarian pizzas"
        }

        // Test 10: Find Most Sold Pizza
        test "Find Most Sold Pizza" {
            let result = findMostSoldPizza pizzas orders
            Expect.equal result.id "1" "Should be the pizza with id '1'"
        }

        // Test 11: Calculate Average Pizzas Per Order
        test "Calculate Average Pizzas Per Order" {
            let result = calculateAveragePizzasPerOrder orders
            Expect.floatClose Accuracy.medium result 0.33 "Should be approximately 0.33"
        }

        // Test 12: Find Unused Ingredients
        test "Find Unused Ingredients" {
            let result = findUnusedIngredients pizzas orders
            Expect.isEmpty result "Should be no unused ingredients"
        }

        // Test 13: Count Pizzas Ordered Once
        test "Count Pizzas Ordered Once" {
            let result = countPizzasOrderedOnce orders
            Expect.floatClose Accuracy.medium result 3 "Should be 3 pizzas ordered only once"
        }

        // Test 14: Calculate Average Preparation Time
        test "Calculate Average Preparation Time" {
            let result = Math.Round(calculateAveragePreparationTime orders, 2)
            Expect.floatClose Accuracy.medium result 21.67 "Should be 21.67"
        }

        // Test 15: Calculate Average Delivery Costs for Takeaway
        test "Calculate Average Delivery Costs for Takeaway" {
            let result = calculateAverageDeliveryCostsForTakeaway orders
            Expect.floatClose Accuracy.medium result 4.0 "Should be 4.0 average delivery cost"
        }

        // Test 16: Find Most Sold Pizza in Takeaway Orders
        test "Find Most Sold Pizza in Takeaway Orders" {
            let result = findMostSoldPizzaInTakeawayOrders pizzas orders
            match result with
            | Some pizza -> Expect.equal pizza.id "4" "Should be the pizza with id '4'"
            | None -> failwith "No pizza found"
        }
    ]

let main argv =
    runTestsWithCLIArgs [] argv tests
