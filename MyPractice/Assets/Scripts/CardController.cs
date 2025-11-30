using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Image frontImage; // Reference to the front image of the card
    public GameObject back; 
    public GameObject front;
    public Button button; // Reference to the button component
    public CardData cardData; // Reference to the CardData scriptable object
    private bool isOpen = false; // Track if the card is open

    public CardData GetCardData()
    {
        return cardData;
    }
    public bool IsCardOpen()
    {
        return isOpen;
    }
    public void SetData(CardData data)
    {
        cardData = data;
        if (cardData != null)
        {
            frontImage.sprite = cardData.frontImage; // Set the front image from CardData
        }
    }
    public void OnClick()
    {
        if (isOpen)
        {
            isOpen = true; // Set the card as open
            front.SetActive(true); // Show the front of the card
            back.SetActive(false); // Hide the back of the card
        }
        else
        {
            isOpen = false; // Set the card as open
            front.SetActive(false); // Show the front of the card
            back.SetActive(true); // Hide the back of the card
        }
    }
}
