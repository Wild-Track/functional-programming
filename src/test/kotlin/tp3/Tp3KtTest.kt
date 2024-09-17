package tp3

import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class FruitStockTests {

    @Test
    fun `test add new fruit`() {
        val initialState = FruitStockState(mapOf())
        val result = addFruit(initialState, 1, "Mango", 10)
        assertTrue(result.isSuccess)

        val newState = result.getOrNull()!!
        assertEquals(1, newState.stock.size)
        assertEquals("Mango", newState.stock[1]?.name)
        assertEquals(10, newState.stock[1]?.quantity)
    }

    @Test
    fun `test add existing fruit`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = addFruit(initialState, 2, "Apple", 5)
        assertTrue(result.isFailure)

        val error = result.exceptionOrNull()
        assertEquals("Apple already exists in stock.", error?.message)
    }

    @Test
    fun `test delete existing fruit`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = deleteFruit(initialState, 1)
        assertTrue(result.isSuccess)

        val newState = result.getOrNull()!!
        assertTrue(newState.stock.isEmpty())
    }

    @Test
    fun `test delete non-existing fruit`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = deleteFruit(initialState, 2)
        assertTrue(result.isFailure)

        val error = result.exceptionOrNull()
        assertEquals("Fruit with ID 2 doesn't exist in stock.", error?.message)
    }

    @Test
    fun `test sell fruit with sufficient stock`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = sellFruit(initialState, "Apple", 5)
        assertTrue(result.isSuccess)

        val newState = result.getOrNull()!!
        assertEquals(5, newState.stock[1]?.quantity)
    }

    @Test
    fun `test sell fruit with insufficient stock`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = sellFruit(initialState, "Apple", 15)
        assertTrue(result.isFailure)

        val error = result.exceptionOrNull()
        assertEquals("Can't sell 15 Apple - insufficient stock.", error?.message)
    }

    @Test
    fun `test refull existing fruit`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = refullFruit(initialState, "Apple", 5)
        assertTrue(result.isSuccess)

        val newState = result.getOrNull()!!
        assertEquals(15, newState.stock[1]?.quantity)
    }

    @Test
    fun `test refull non-existing fruit`() {
        val initialState = FruitStockState(mapOf(1 to Fruit(1, "Apple", 10)))
        val result = refullFruit(initialState, "Mango", 5)
        assertTrue(result.isSuccess)

        val newState = result.getOrNull()!!
        assertEquals(2, newState.stock.size)
        assertEquals("Mango", newState.stock[2]?.name)
        assertEquals(5, newState.stock[2]?.quantity)
    }
}
