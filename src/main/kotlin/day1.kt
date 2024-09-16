typealias FruitStock = MutableMap<String, Int>

val stock: FruitStock = mutableMapOf(
    "Apple" to 10,
    "Pear" to 5,
    "Pineapple" to 8
)

fun log(message: String) {
    println("[LOG] $message")
}

fun sellFruit(fruit: String, quantity: Int) {
    if (!checkAvailability(fruit, quantity)) {
        log("Can't sell $quantity $fruit - insufficient stock.")
        return
    }
    stock[fruit] = stock[fruit]!! - quantity
    log("$quantity - $fruit sell.")
}

fun addFruit(fruit: String, quantity: Int) {
    if (stock.containsKey(fruit)) {
        log("$fruit already exist in stock.")
        return
    }
    stock[fruit] = quantity
    log("$fruit add to stock with $quantity quantity.")
}

fun refullFruit(fruit: String, quantity: Int) {
    if (!stock.containsKey(fruit)) {
        log("Can't refull $fruit - fruit not found.")
        return
    }
    stock[fruit] = stock[fruit]!! + quantity
    log("$quantity $fruit add to stock.")
}

fun deleteFruit(fruit: String) {
    if (!stock.containsKey(fruit)) {
        log("$fruit doesn't exist in stock.")
        return
    }
    stock.remove(fruit)
    log("$fruit delete from stock.")
}

fun checkAvailability(fruit: String, quantity: Int): Boolean {
    if (!stock.containsKey(fruit)) {
        log("$fruit is not available for sale.")
        return false
    }
    if (stock[fruit]!! < quantity) {
        log("insufficient quantity of $fruit - available : ${stock[fruit]}.")
        return false
    }
    return true
}

fun showStock() {
    println("Actual stock :")
    for ((fruit, quantity) in stock) {
        println("- $fruit : $quantity")
    }
}


fun main() {
    log("Initialisation.")
    showStock()

    refullFruit("Apple", 5)

    addFruit("Watermelon", 8)

    sellFruit("Pineapple", 2)

    showStock()

    deleteFruit("Pineapple")

    showStock()
}
