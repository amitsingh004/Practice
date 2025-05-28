using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GC",menuName = "GameConfigSet/GameConfig")]
public class GameConfig : ScriptableObject
{
    public Vector2Int layoutSize;
    public int cardsToMatch = 2;
    public float revealDelay = 0.5f;
    public float matchCheckDelay = 0.4f;
}

