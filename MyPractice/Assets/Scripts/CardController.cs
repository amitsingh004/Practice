using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;           // Reference to CardView
    private CardData cardData;       // Card info
    public bool IsOpen { get; private set; }

    public CardStates CurrentState { get; private set; } 

    public void Awake()
    {
        IsOpen = true;
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
            GamePlayManager.Instance.OnCardClicked(this);
        }
    }

    public void Open()
    {
        if (CurrentState != CardStates.Closed || view.IsFlipping) return;
        CurrentState = CardStates.Open; ;
        view.SetInteractable(false);
        view.FlipCard(true);
       
        
    }

    public void Close()
    {
        if (CurrentState != CardStates.Open || view.IsFlipping) return;
        CurrentState = CardStates.Closed;
        view.FlipCard(false);
        view.SetInteractable(true);
        
    }

    public void OnDestroy()
    {
       
    }
}
