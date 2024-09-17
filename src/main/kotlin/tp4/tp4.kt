package tp4

import kotlin.random.Random

data class Score(val teamA: Int = 0, val teamB: Int = 0)
data class PenaltyHistory(val attempt: Int, val score: Score, val teamAScored: Boolean, val teamBScored: Boolean)
data class GameState(val history: List<PenaltyHistory> = emptyList(), val score: Score = Score(), val attempt: Int = 1)

sealed class PenaltyResult
object Scored : PenaltyResult()
object Missed : PenaltyResult()

fun shootPenalty(): PenaltyResult = if (Random.nextBoolean()) Scored else Missed

fun updateScore(score: Score, teamAScored: Boolean, teamBScored: Boolean): Score =
    score.copy(
        teamA = score.teamA + (if (teamAScored) 1 else 0),
        teamB = score.teamB + (if (teamBScored) 1 else 0)
    )

fun isWinBeforeEnd(score: Score, remainingAttempts: Int): Boolean =
    score.teamA > score.teamB + remainingAttempts || score.teamB > score.teamA + remainingAttempts

fun isGameOver(state: GameState, maxAttempts: Int): Boolean =
    state.attempt > maxAttempts && state.score.teamA != state.score.teamB

fun simulatePenalties(state: GameState, maxAttempts: Int = 5): GameState {
    val teamAScored = shootPenalty() is Scored
    val teamBScored = shootPenalty() is Scored

    val updatedScore = updateScore(state.score, teamAScored, teamBScored)
    val newHistory = PenaltyHistory(state.attempt, updatedScore, teamAScored, teamBScored)
    val newState = state.copy(
        history = state.history + newHistory,
        score = updatedScore,
        attempt = state.attempt + 1
    )

    return when {
        isWinBeforeEnd(updatedScore, maxAttempts - state.attempt) -> newState
        isGameOver(newState, maxAttempts) -> newState
        else -> simulatePenalties(newState, maxAttempts)
    }
}

fun displayHistory(state: GameState): Unit =
    state.history
        .map { history ->
            "Tir ${history.attempt} | Score : ${history.score.teamA}/${history.score.teamB} (équipe A : ${if (history.teamAScored) "+1" else "0"}, équipe B : ${if (history.teamBScored) "+1" else "0"})"
        }
        .forEach(::println)
        .also {
            println("Victoire : ${if (state.score.teamA > state.score.teamB) "Equipe A" else "Equipe B"}")
        }

fun main() {
    println("Début de la séance de tirs au but...")
    val finalState = simulatePenalties(GameState())
    displayHistory(finalState)
}
