using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameStats gameStats;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    

    [SerializeField] GameObject gameOverPanel;
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
        gameOverPanel.SetActive(true);
        AudioMgr.Instance.PlaySound(SoundType.GameOver);
        //Debug.Log("All pairs matched! You win!");
    }

    private void UpdateTurnsDisplay(int turns)
    {
       
        


        if (turnText != null)
            turnText.text = turns.ToString();
    }

    private void UpdateScoreDisplay(int totalScore)
    {
        


        if (scoreText != null)
            scoreText.text = totalScore.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void OnDisable()
    {
        //unregister events to prevent memory leaks
        if (gameStats == null) return;
        gameStats.OnScoreChanged -= UpdateScoreDisplay;
        gameStats.OnTurnCountChanged -= UpdateTurnsDisplay;
        gameStats.OnGameWon -= ShowWinMessage;
    }
}
