package tp4

import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class PenaltyShootoutTest {

    @Test
    fun `test shootPenalty return soit réussi soit manqué`() {
        val result = shootPenalty()
        assertTrue(result is PenaltyResult)
    }

    @Test
    fun `test updateScore avec équipe A marquant`() {
        val initialScore = Score(teamA = 1, teamB = 2)
        val updatedScore = updateScore(initialScore, teamAScored = true, teamBScored = false)
        assertEquals(Score(teamA = 2, teamB = 2), updatedScore)
    }

    @Test
    fun `test updateScore avec équipe B marquant`() {
        val initialScore = Score(teamA = 1, teamB = 2)
        val updatedScore = updateScore(initialScore, teamAScored = false, teamBScored = true)
        assertEquals(Score(teamA = 1, teamB = 3), updatedScore)
    }

    @Test
    fun `test updateScore avec les deux équipes marquant`() {
        val initialScore = Score(teamA = 1, teamB = 2)
        val updatedScore = updateScore(initialScore, teamAScored = true, teamBScored = true)
        assertEquals(Score(teamA = 2, teamB = 3), updatedScore)
    }

    @Test
    fun `test isWinBeforeEnd vrai quand l'équipe A a un avantage certain`() {
        val score = Score(teamA = 4, teamB = 0)
        assertTrue(isWinBeforeEnd(score, remainingAttempts = 1))
    }

    @Test
    fun `test isWinBeforeEnd faux quand le jeu est encore jouable`() {
        val score = Score(teamA = 3, teamB = 2)
        assertFalse(isWinBeforeEnd(score, remainingAttempts = 1))
    }

    @Test
    fun `test isGameOver quand les tentatives maximales sont atteintes et les scores sont différents`() {
        val state = GameState(attempt = 6, score = Score(teamA = 3, teamB = 2))
        assertTrue(isGameOver(state, maxAttempts = 5))
    }

    @Test
    fun `test isGameOver faux quand les tentatives maximales sont atteintes mais les scores sont égaux`() {
        val state = GameState(attempt = 6, score = Score(teamA = 3, teamB = 3))
        assertFalse(isGameOver(state, maxAttempts = 5))
    }

    @Test
    fun `test simulatePenalties se termine avec un état valide`() {
        val initialState = GameState()
        val finalState = simulatePenalties(initialState)

        assertTrue(finalState.score.teamA != finalState.score.teamB)
        assertTrue(finalState.attempt > 5 || isWinBeforeEnd(finalState.score, 5 - finalState.attempt))
    }
}