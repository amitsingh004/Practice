using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameStats gameStats;

    public void Initialize(GameStats stats)
    {
        gameStats = stats;
        
        // Subscribe to events
        gameStats.OnScoreChanged += UpdateScoreDisplay;
        gameStats.OnTurnCountChanged += UpdateTurnsDisplay;
        gameStats.OnGameWon += ShowWinMessage;
    }

    private void ShowWinMessage()
    {
        Debug.Log("All pairs matched! You win!");
    }

    private void UpdateTurnsDisplay(int turns)
    {
       
    }

    private void UpdateScoreDisplay(int totalScore)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
