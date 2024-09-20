module Types

open System
open System.Text.Json.Serialization


type Pizza = {
    [<JsonPropertyName("id")>] id: string
    [<JsonPropertyName("name")>] name: string
    [<JsonPropertyName("price")>] price: float
    [<JsonPropertyName("base")>] pizzaBase: string
    [<JsonPropertyName("ingredients")>] ingredients: string list
}

type OrderItem = {
    [<JsonPropertyName("pizzaId")>] pizzaId: string
    [<JsonPropertyName("quantity")>] quantity: int
    [<JsonPropertyName("price")>] price: float
    [<JsonPropertyName("amount")>] amount: float
}

type Order = {
    [<JsonPropertyName("id")>] id: string
    [<JsonPropertyName("orderedAt")>] orderedAt: DateTime
    [<JsonPropertyName("readyAt")>] readyAt: DateTime
    [<JsonPropertyName("orderType")>] orderType: string
    [<JsonPropertyName("status")>] status: string
    [<JsonPropertyName("amount")>] amount: float
    [<JsonPropertyName("totalAmount")>] totalAmount: float
    [<JsonPropertyName("items")>] items: OrderItem list
    [<JsonPropertyName("deliveryCosts")>] deliveryCosts: float
}
