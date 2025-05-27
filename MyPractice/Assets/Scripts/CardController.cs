using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;           // Reference to CardView
    public CardData cardData;       // Card info
    public bool IsOpen { get; private set; }

    public void Awake()
    {
        IsOpen = true;
        view.SetInteractable(false);
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
        if (!IsOpen && !view.IsFlipping)
        {
            GamePlayManager.Instance.OnCardClicked(this);
        }
    }

    public void Open()
    {
        if (IsOpen || view.IsFlipping) return;
        IsOpen = true;
        view.SetInteractable(false);
        view.FlipCard(true);
       
        
    }

    public void Close()
    {
        if (!IsOpen || view.IsFlipping) return;
        IsOpen = false;
        view.FlipCard(false);
        view.SetInteractable(true);
        
    }

    public void OnDestroy()
    {
       
    }
}
