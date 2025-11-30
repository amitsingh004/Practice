using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData")]
public class CardData :ScriptableObject
{
  public int id; // Unique identifier for the card
  public Sprite frontImage; // Image for the front of the card
}
