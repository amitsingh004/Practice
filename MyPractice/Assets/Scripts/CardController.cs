using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Image frontImage; // Reference to the front image of the card
    public GameObject back;
    public GameObject front;
    public Button button; // Reference to the button component
    public CardData cardData; // Reference to the CardData scriptable object
    private bool isCardOpen = true; // Track if the card is open
    [SerializeField] private float flipDuration = 0.2f; // Duration of the flip animation
    [SerializeField] private float closeDelay = 1f; // Delay before flipping back
    [SerializeField] private float fadeDuration = 0.5f; // Duration of the fade-out animation
    Coroutine flipCoroutine; // Coroutine for flipping the card
    private bool isOpening = false; // Track if the card is currently opening

    void Start()
    {
        button.interactable = false;
    }
    public CardData GetCardData()
    {
        return cardData;
    }
    public bool IsCardOpen()
    {
        return isCardOpen;
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
        if (!isCardOpen && !isOpening)
        {
            GamePlayManager.Instance.OnCardClicked(this); // Notify the GamePlayManager that this card was clicked
        }
    }

    private IEnumerator FlipCard(bool isOpen)
    {
        // Determine the current rotation
        isOpening = true;
        float currentRotationY = transform.rotation.eulerAngles.y;
        float startAngle = currentRotationY;
        float endAngle = isOpen ? 180f : 0f;
        yield return StartCoroutine(Flip(startAngle, 90)); // Rotate halfway
        back.SetActive(!isOpen);
        front.SetActive(isOpen);
        yield return StartCoroutine(Flip(90, endAngle)); // Complete the flip
        this.isCardOpen = isOpen; // Update the open state
        isOpening = false; // Reset the opening state
    }
    private IEnumerator Flip(float startAngle, float endAngle)
    {
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / flipDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, progress);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, endAngle, 0);
    }
     public void OpenCard()
    {
        if( isCardOpen || isOpening) return; // Ignore if the card is already open or opening
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine); // Stop any ongoing flip animation
        }
        button.interactable = false; // Disable the button to prevent further clicks
        flipCoroutine = StartCoroutine(FlipCard(true));
    }
    public void CloseCard()
    {
        if (!isCardOpen || isOpening) return; // Ignore if the card is already closed or closing
        if (flipCoroutine != null) StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(FlipCard(false));
        button.interactable = true; // Re-enable the button
    }
     void OnDestroy()
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine); // Stop the coroutine if it is running
            flipCoroutine = null; // Clear the reference
        }
    }
}
