using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    public int score;
    public int turns;
    public Vector2Int layoutSize;
    public int[] cardStates; // 0: Closed, 1: Opening, 2: Open, 3: Closing, 4: Matched
    public int[] cardIds; // Unique IDs for each card


}