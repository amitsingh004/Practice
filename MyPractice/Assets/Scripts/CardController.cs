using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;           // Reference to CardView
    private CardData cardData;       // Card info

    public CardStates CurrentState { get; private set; } 

    public event System.Action<CardController> OnCardClicked;
    public void Awake()
    {
        
        view.SetInteractable(false);
        CurrentState = CardStates.Open;
    }
    public void Start()
    {
        
    }
    public CardData GetCardData()
    {
        return cardData;
    }
    public void SetData(CardData data)
    {
        cardData = data;
        view.SetCardImage(data.frontImage);
    }
    public int GetCardId()
    {
        return cardData.id;
    }
    public void OnClick()
    {
        if (CurrentState == CardStates.Closed && !view.IsFlipping)
        {
            OnCardClicked?.Invoke(this); // Invoke the event when the card is clicked
        }
    }

    public void Open()
    {
        if (CurrentState != CardStates.Closed || view.IsFlipping) return;
        CurrentState = CardStates.Open; ;
        view.SetInteractable(false);
        view.FlipCard(true);
       
        
    }

    public void OpenWithoutAnimation()
    {
        if (CurrentState != CardStates.Closed || view.IsFlipping) return;
        CurrentState = CardStates.Open;
        view.SetInteractable(false);
        view.FlipCardWithoutAnimation(true);
    }
    public void SetMatched()
    {
        
        CurrentState = CardStates.Matched;
        view.SetInteractable(false);
        view.FadeOutCard();
        
    }

    public void SetMatchedWithoutAnimation()
    {
        CurrentState = CardStates.Matched;
        view.SetInteractable(false);
        view.FadeOutCardWithoutAnimation();
    }
    public void Close()
    {
        if (CurrentState != CardStates.Open || view.IsFlipping) return;
        CurrentState = CardStates.Closed;
        view.FlipCard(false);
        view.SetInteractable(true);
        
    }

    public void CloseWithoutAnimation()
    {
        if (CurrentState != CardStates.Open || view.IsFlipping) return;
        CurrentState = CardStates.Closed;
        view.FlipCardWithoutAnimation(false);
        view.SetInteractable(true);
    }
    public void OnDestroy()
    {
       OnCardClicked = null; // Unsubscribe from the event to prevent memory leaks
    }
}
