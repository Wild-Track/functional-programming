package tp3

import kotlin.Result.Companion.failure
import kotlin.Result.Companion.success

data class Fruit(val id: Int, val name: String, val quantity: Int)

data class FruitStockState(val stock: Map<Int, Fruit>)

sealed class StockAction {
    data class AddFruit(val id: Int, val name: String, val quantity: Int) : StockAction()
    data class DeleteFruit(val id: Int) : StockAction()
    data class SellFruit(val name: String, val quantity: Int) : StockAction()
    data class RefullFruit(val name: String, val quantity: Int) : StockAction()
}

fun handleAction(state: FruitStockState, action: StockAction): Result<FruitStockState> {
    return when (action) {
        is StockAction.AddFruit -> addFruit(state, action.id, action.name, action.quantity)
        is StockAction.DeleteFruit -> deleteFruit(state, action.id)
        is StockAction.SellFruit -> sellFruit(state, action.name, action.quantity)
        is StockAction.RefullFruit -> refullFruit(state, action.name, action.quantity)
    }
}

fun addFruit(state: FruitStockState, id: Int, fruitName: String, quantity: Int): Result<FruitStockState> {
    val stock = state.stock
    return if (stock.values.any { it.name == fruitName }) {
        failure(IllegalArgumentException("$fruitName already exists in stock."))
    } else {
        val newFruit = Fruit(id = id, name = fruitName, quantity = quantity)
        success(FruitStockState(stock = stock + (id to newFruit)))
    }
}

fun deleteFruit(state: FruitStockState, fruitId: Int): Result<FruitStockState> {
    val stock = state.stock
    return if (stock.containsKey(fruitId)) {
        success(FruitStockState(stock = stock - fruitId))
    } else {
        failure(IllegalArgumentException("Fruit with ID $fruitId doesn't exist in stock."))
    }
}

fun checkAvailability(state: FruitStockState, fruitName: String, quantity: Int): Boolean {
    val fruit = state.stock.values.find { it.name == fruitName }
    return when {
        fruit == null -> false
        fruit.quantity < quantity -> false
        else -> true
    }
}

fun sellFruit(state: FruitStockState, fruitName: String, quantity: Int): Result<FruitStockState> {
    return if (!checkAvailability(state, fruitName, quantity)) {
        failure(IllegalArgumentException("Can't sell $quantity $fruitName - insufficient stock."))
    } else {
        val updatedStock = state.stock.mapValues { (id, fruit) ->
            if (fruit.name == fruitName) fruit.copy(quantity = fruit.quantity - quantity) else fruit
        }
        success(FruitStockState(stock = updatedStock))
    }
}

fun refullFruit(state: FruitStockState, fruitName: String, quantity: Int): Result<FruitStockState> {
    val stock = state.stock
    return if (stock.values.any { it.name == fruitName }) {
        val updatedStock = stock.mapValues { (id, fruit) ->
            if (fruit.name == fruitName) fruit.copy(quantity = fruit.quantity + quantity) else fruit
        }
        success(FruitStockState(stock = updatedStock))
    } else {
        val newId = (stock.keys.maxOrNull() ?: 0) + 1
        addFruit(state, newId, fruitName, quantity)
    }
}

fun showStock(state: FruitStockState) {
    println("Current stock:")
    state.stock.values.forEach { fruit ->
        println("- ${fruit.name}: ${fruit.quantity} (ID: ${fruit.id})")
    }
}

fun main() {
    println("Initialization.")
    var state = FruitStockState(
        mapOf(
            1 to Fruit(id = 1, name = "Apple", quantity = 10),
            2 to Fruit(id = 2, name = "Pear", quantity = 5),
            3 to Fruit(id = 3, name = "Pineapple", quantity = 8)
        )
    )
    showStock(state)

    val actions = listOf(
        StockAction.RefullFruit(name = "Apple", quantity = 5),
        StockAction.AddFruit(id = 4, name = "Watermelon", quantity = 8),
        StockAction.SellFruit(name = "Pineapple", quantity = 2),
        StockAction.DeleteFruit(id = 3)
    )

    state = actions.fold(state) { currentState, action ->
        handleAction(currentState, action).getOrElse {
            println("Error: ${it.message}")
            currentState
        }
    }

    showStock(state)
}
