using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] public CardLayoutManager cardLayoutManager; // Reference to the CardLayoutManager
    [SerializeField] private UIManager uiManager;
    public List<CardData> cardSOList; // List of CardData objects to initialize the layout
    private List<CardController> spawnedCards = new List<CardController>();
    Coroutine cardMatchCoroutine; // Coroutine for matching cards
    private Coroutine cardRevealCoroutine;
    private Queue<CardController> matchQueue = new Queue<CardController>();
    private bool isProcessingQueue = false;
    private GameStats gameStats;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Singleton instance
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        gameStats = new GameStats(); // Initialize game stats
        uiManager.Initialize(gameStats);

    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame(); // Start a new game when the script is initialized
    }

    void StartNewGame()
    {
        int totalCards = gameConfig.layoutSize.x * gameConfig.layoutSize.y; // Calculate total cards based on layout size
        var cards = GererateCards(totalCards);
        spawnedCards = cardLayoutManager.InitCardLayout(cards, gameConfig.layoutSize); // Initialize the card layout with the generated cards
        RegisterEvents(); // Register card click events
        gameStats.Initialize(spawnedCards.Count / 2); // Initialize game stats with the number of pairs
        cardRevealCoroutine = StartCoroutine(RevealCardsThenClose(gameConfig.revealDelay)); // Start revealing cards with a delay
    }
    List<CardData> GererateCards(int count)
    {
        List<CardData> generatedCards = new List<CardData>();
        for (int i = 0; i < count / 2; i++)
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

        while (matchQueue.Count >= gameConfig.cardsToMatch)
        {
            List<CardController> matchGroup = new List<CardController>();

            // Dequeue N cards for this match check
            for (int i = 0; i < gameConfig.cardsToMatch; i++)
            {
                matchGroup.Add(matchQueue.Dequeue());
            }

            // Wait a moment to allow the last card animation to finish
            yield return new WaitForSeconds(gameConfig.matchCheckDelay);
            gameStats.IncrementTurnCount();
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
                yield return new WaitForSeconds(0.5f); // Allow time to see unmatched cards
                foreach (var card in matchGroup)
                {
                    card.Close();
                }
            }
            else
            {
                gameStats.AddScore(1);
                foreach (var card in matchGroup)
                {
                    card.SetMatched(); // Disable interaction for matched cards
                }
            }
        }

        isProcessingQueue = false;
    }
    private IEnumerator RevealCardsThenClose(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var card in spawnedCards)
        {
            card.Close(); // Close the card after the delay
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void RegisterEvents()
    {
        //Card events registration
        foreach (var card in spawnedCards)
        {
            card.OnCardClicked += OnCardClicked; // Register the click event for each card
        }
        // Game restart event registration
        GameEvents.OnGameRestart += RestartGame; // Register the game restart event
    }
    void UnregisterEvents()
    {
        // Unregister the game restart event
        foreach (var card in spawnedCards)
        {
            card.OnCardClicked -= OnCardClicked; // Unregister the click event for each card
        }
        GameEvents.OnGameRestart -= RestartGame; // Unregister the game restart event
    }
    public void Reset()
    {
        if (cardMatchCoroutine != null)
        {
            StopCoroutine(cardMatchCoroutine);
            cardMatchCoroutine = null;
        }
        if (cardRevealCoroutine != null)
        {
            StopCoroutine(cardRevealCoroutine);
            cardRevealCoroutine = null;
        }

        UnregisterEvents(); // Unregister all card events to prevent memory leaks
        spawnedCards.Clear(); // Clear the list of spawned cards
        cardLayoutManager = null; // Clear the reference to the CardLayoutManager
        cardSOList.Clear(); // Clear the list of CardData objects
        matchQueue.Clear(); // Clear the match queue
        Instance = null;
    }
    public void OnDisable()
    {
        Reset();

    }
    public void RestartGame()
    {
        StopAllCoroutines();
        Reset(); // Reset the game state
       // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
