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
    private LocalStrgMgr localStorageManager;
    void Awake()
    {
        // Simple singleton pattern without DontDestroyOnLoad
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gameStats = new GameStats(); // Initialize game stats
        uiManager.Initialize(gameStats);

        localStorageManager = new LocalStrgMgr();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (localStorageManager.HasSavedGame())
        {
            localStorageManager.DeleteSaveData();
             //LoadGameState(); // Load saved game state if available
            StartNewGame();
        }
        else
        {
            StartNewGame(); // Start a new game when the script is initialized
        }
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
            gameStats.AddTurnCount(1);
            // Check if they match
            bool isMatch = true;
            var startId = matchGroup[0].GetCardData().id;
            for (int i = 1; i < matchGroup.Count; i++)
            {
                if (matchGroup[i].GetCardData().id != startId)
                {
                    isMatch = false;
                    break;
                }
            }

            if (!isMatch)
            {
                AudioMgr.Instance.PlaySound(SoundType.CardMismatch); // Play mismatch sound
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
        if (localStorageManager != null)
        {
            localStorageManager.DeleteSaveData();
        }
        
        matchQueue.Clear(); // Clear the match queue
        
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
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGameState();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    private void SaveGameState()
    {
        if (gameStats.IsGameComplete)
        {
            Debug.Log("Game already completed. No need to save.");
            localStorageManager.DeleteSaveData(); // Delete save data if the game is complete
            return; // Don't save if the game is already complete
        }
        var gameSaveState = new GameSaveState
        {
            score = gameStats.Score,
            turns = gameStats.TurnCount,
            layoutSize = gameConfig.layoutSize,
            cardIds = new int[spawnedCards.Count],
            cardStates = new int[spawnedCards.Count]
        };
     
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            gameSaveState.cardIds[i] = spawnedCards[i].GetCardId();
            gameSaveState.cardStates[i] = (int)spawnedCards[i].CurrentState; // Save the current state of each card
        }
        
        
        localStorageManager.SaveGame(gameSaveState); // Save the game state using LocalStrgMgr
        Debug.Log("Game state saved successfully.");

    }
    private void LoadGameState()
    {
        GameSaveState gameSaveState;
        localStorageManager.LoadGame(out gameSaveState); // Load the game state using LocalStrgMgr
        if (gameSaveState != null)
        {
            //Recreate the card layout based on the saved state
            gameConfig.layoutSize = gameSaveState.layoutSize;
            // gegerate the card data based on the saved IDs
            List<CardData> cardDataList = new List<CardData>();
            for (int i = 0; i < gameSaveState.cardIds.Length; i++)
            {
                int cardId = gameSaveState.cardIds[i];
                CardData cardDataSO = cardSOList.Find(card => card.id == cardId);
                if (cardDataSO != null)
                {
                    cardDataList.Add(cardDataSO);
                }
            }
            // Initialize the card layout with the loaded card data
            spawnedCards = cardLayoutManager.InitCardLayout(cardDataList, gameConfig.layoutSize);
            //Apply the saved states to the cards
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                CardController card = spawnedCards[i];
                var savedState = (CardStates)gameSaveState.cardStates[i];
                //Apply the saved state to the card
                switch (savedState)
                {
                    case CardStates.Open:
                    case CardStates.Closed:
                        card.CloseWithoutAnimation();
                        break;
                    case CardStates.Matched:
                        card.SetMatchedWithoutAnimation();
                        break;
                    default:
                        card.CloseWithoutAnimation(); // Default to closed state if unknown
                        break;
                }

            }
            //Reset the game progress controller with the loaded score and turns
            gameStats.Initialize(spawnedCards.Count / 2);
            gameStats.AddScore(gameSaveState.score);
            gameStats.AddTurnCount(gameSaveState.turns);

            RegisterEvents(); // Register card click events

            // if (gameSaveState.matchQueue.Length > 0)
            // {
            //     // Rebuild the match queue from the saved state
            //     foreach (var cardId in gameSaveState.matchQueue)
            //     {
            //         var card = spawnedCards.Find(c => c.GetCardId() == cardId);
            //         if (card != null && card.CurrentState != CardStates.Matched && card.CurrentState != CardStates.Closed)
            //         {
            //             matchQueue.Enqueue(card);
            //         }
            //     }
            //     isProcessingQueue = false; // Reset the processing flag
            //     cardMatchCoroutine = StartCoroutine(CheckMatch()); // Restart the match checking coroutine
            // }
            // else
            // {
            //     matchQueue.Clear(); // No cards in the match queue
            // }
           
            
            Debug.Log("Game state loaded successfully.");
        }
    }
}
