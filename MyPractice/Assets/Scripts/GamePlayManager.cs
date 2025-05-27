using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;
    [SerializeField] public CardLayoutManager cardLayoutManager; // Reference to the CardLayoutManager
    public List<CardData> cardSOList; // List of CardData objects to initialize the layout

    [SerializeField] private Vector2Int layoutSize ; 
    void Awake()
    {
        Instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame(); // Start a new game when the script is initialized
    }

    void StartNewGame()
    {
        int totalCards = layoutSize.x * layoutSize.y; // Calculate total cards based on layout size
        var cards = GererateCards(totalCards);
        cardLayoutManager.InitCardLayout(cards, layoutSize); // Initialize the card layout with the generated cards
    }
    List<CardData> GererateCards(int count)
    {
        List<CardData> generatedCards = new List<CardData>();
        for (int i = 0; i < count/2; i++)
        {
            int index = i % cardSOList.Count;
            var cardData = cardSOList[index];
            generatedCards.Add(cardData);
            generatedCards.Add(cardData); // Add each card twice for matching
        }
        ShuffleCards(generatedCards); // Shuffle the cards to randomize their order
        return generatedCards;
    }

    void ShuffleCards(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardData temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public void OnCardClicked(CardController card)
    {

        if (card.IsCardOpen())
            card.CloseCard(); // Ignore clicks on already open cards
        else
             card.OpenCard();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
