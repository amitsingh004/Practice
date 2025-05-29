using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    public int score;
    public int turns;
    public int gameConfigId; // 
    public int matchedPairs; // Number of pairs matched
    public int[] cardStates; // 0: Closed, 1: Opening, 2: Open, 3: Closing, 4: Matched
    public int[] cardIds; // Unique IDs for each card


}