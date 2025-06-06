using UnityEngine;
using System;
public class GameStats 
{
    private int score = 0;
    private int totalPairs;
    private int matchedPairs = 0;
    private int turnCount = 0;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnTurnCountChanged;
    public event Action OnGameWon;

    public int Score => score;
    public int TurnCount => turnCount;
    public bool IsGameComplete => matchedPairs >= totalPairs;
    public int MatchedPairs => matchedPairs;
    public void Initialize(int totalPairs)
    {
        this.totalPairs = totalPairs;
        score = 0;
        matchedPairs = 0;
        Debug.Log("GameStats initialized.");
    }
    public void ResetScore()
    {
        score = 0;
        matchedPairs = 0;
        OnScoreChanged?.Invoke(score);

    }
    public void ResetTurnCount()
    {
        turnCount = 0;
        OnTurnCountChanged?.Invoke(turnCount);
        Debug.Log("Turn count reset to 0.");
    }
    public void AddTurnCount(int count)
    {
        turnCount+=count;
        Debug.Log($"Turn count increased to {turnCount}");
        OnTurnCountChanged?.Invoke(turnCount);
    }
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score increased by {amount}. Current score: {score}");
        OnScoreChanged?.Invoke(score);
    
    }
    public void AddMatchedPairs(int pairCount)
    {
        matchedPairs+=pairCount;
        Debug.Log($"Matched pairs increased to {matchedPairs}");
        CheckGameWin();
    }
    public void CheckGameWin()
    {
        Debug.Log($"Matched pairs: {matchedPairs}/{totalPairs}");
        if (matchedPairs >= totalPairs)
        {
            OnGameWon?.Invoke();
            Debug.Log("All pairs matched! Game won!");
        }
    }
}
