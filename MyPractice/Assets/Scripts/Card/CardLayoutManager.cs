using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLayoutManager : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab for the card
    public GridLayoutGroup cardsGrid; // Reference to the GridLayoutGroup component
    public Transform cardParent; // Parent transform for the cards

   
   
    public List<CardController> InitCardLayout(List<CardData> cardDataList, Vector2Int layoutSize)
    {
         List<CardController> spawnedCards = new List<CardController>();
        // Set the layout size based on the provided data
        cardsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // Use fixed column count
        cardsGrid.constraintCount = layoutSize.x;


        // Create new cards based on the provided data
        foreach (var cardData in cardDataList)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardParent);
            CardController cardController = cardObject.GetComponent<CardController>();
            if (cardController != null)
            {
                cardController.SetData(cardData);
                spawnedCards.Add(cardController);
            }
            else
            {
                Debug.LogError("CardController component is missing on the card prefab!");
            }
        }
        return spawnedCards; // Return the list of spawned cards
        
    }

   

   
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        
       
    }
}
