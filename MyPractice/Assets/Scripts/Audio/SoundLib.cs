using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Audio/GamePlay/Sounds")]
public class SoundLib :ScriptableObject
{
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip cardMatch;
    [SerializeField] private AudioClip cardMismatch;
    [SerializeField] private AudioClip gameOver;

    public AudioClip GetSound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.CardFlip:
                return cardFlip;
            case SoundType.CardMatch:
                return cardMatch;
            case SoundType.CardMismatch:
                return cardMismatch;
            case SoundType.GameOver:
                return gameOver;
            default:
                return null;
        }
    }
}

