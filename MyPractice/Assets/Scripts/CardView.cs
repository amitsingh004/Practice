using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject front;
    [SerializeField] private Button button;
    public bool IsFlipping { get; private set; }
    [SerializeField] private float flipDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    private Coroutine fadeCoroutine;    
    private Coroutine flipCoroutine;

    void Awake()
    {
        IsFlipping = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetCardImage(Sprite sprite)
    {
        frontImage.sprite = sprite;
    }
    private void ShowFront(bool show)
    {
        back.SetActive(!show);
        front.SetActive(show);
    }
    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
    public void FlipCard(bool openCard)
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
        }
        flipCoroutine = StartCoroutine(FlipRoutine(openCard));
    }
    private IEnumerator FlipRoutine(bool openCard)
    {
        IsFlipping = true;

        float startAngle = transform.rotation.eulerAngles.y;
        float midAngle = 90f;
        float endAngle = openCard ? 180f : 0f;

        yield return StartCoroutine(Flip(startAngle, midAngle)); // Rotate to halfway
        ShowFront(openCard);
        yield return StartCoroutine(Flip(midAngle, endAngle)); // Complete the flip


        IsFlipping = false;
    }
    
    private IEnumerator Flip(float startAngle, float endAngle)
    {
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, elapsedTime / flipDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, endAngle, 0f);
    }
      public void FadeOutCard()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        float startAlpha = 1f;
        float endAlpha = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        
    }
    void OnDestroy()
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
            flipCoroutine = null;
        }
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
