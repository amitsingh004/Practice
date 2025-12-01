using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;
    [SerializeField] public CardLayoutManager cardLayoutManager; // Reference to the CardLayoutManager
    public List<CardData> cardSOList; // List of CardData objects to initialize the layout
    [SerializeField] private Vector2Int layoutSize;
    [SerializeField] private int cardsToMatch = 2; // Number of cards to match
    Coroutine cardMatchCoroutine; // Coroutine for matching cards
    private Queue<CardController> matchQueue = new Queue<CardController>();
    private bool isProcessingQueue = false;
    int score = 0; // Player's score
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
        if (card.CurrentState == CardStates.Open || matchQueue.Contains(card))
            return;

        card.Open();
        matchQueue.Enqueue(card);

        if (!isProcessingQueue)
        {
            cardMatchCoroutine = StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isProcessingQueue = true;

        while (matchQueue.Count >= cardsToMatch)
        {
            List<CardController> matchGroup = new List<CardController>();

            // Dequeue N cards for this match check
            for (int i = 0; i < cardsToMatch; i++)
            {
                matchGroup.Add(matchQueue.Dequeue());
            }

            // Wait a moment to allow the last card animation to finish
            yield return new WaitForSeconds(0.5f);

            // Check if they match
            bool isMatch = true;
            var reference = matchGroup[0].GetCardData();
            for (int i = 1; i < matchGroup.Count; i++)
            {
                if (matchGroup[i].GetCardData() != reference)
                {
                    isMatch = false;
                    break;
                }
            }

            if (!isMatch)
            {
                yield return new WaitForSeconds(1f); // Allow time to see unmatched cards
                foreach (var card in matchGroup)
                {
                    card.Close();
                }
            }
            else
            {
                score++;
                // Optionally fade out or disable matched cards
            }
        }

        isProcessingQueue = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnDestroy()
    {
        if (cardMatchCoroutine != null)
        {
            StopCoroutine(cardMatchCoroutine); // Stop the coroutine if it is running
            cardMatchCoroutine = null; // Clear the reference
        }
        Instance = null; // Clear the instance reference
        
    }
}
