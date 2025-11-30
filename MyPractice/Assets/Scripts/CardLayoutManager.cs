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

    private List<CardController> spawnedCards = new List<CardController>();

    
    private Coroutine cardRevealCoroutine;
    public void InitCardLayout(List<CardData> cardDataList, Vector2Int layoutSize)
    {

       
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
        cardRevealCoroutine = StartCoroutine(RevealCardsThenClose(0.5f)); // Start revealing cards with a delay
    }

    private IEnumerator RevealCardsThenClose(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var card in spawnedCards)
        {
            card.Close(); // Close the card after the delay
        }
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
        
        if (cardRevealCoroutine != null)
        {
            StopCoroutine(cardRevealCoroutine); // Stop the coroutine if it is running
            cardRevealCoroutine = null; // Clear the reference
        }
    }
}
