using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public string suit; // Suit of the card (hearts, diamonds, clubs, spades)
    public int value; // Value of the card (e.g., 1 for Ace, 11 for Jack, etc.)
    
    // Sprites for suits
    public Sprite heartsSprite;
    public Sprite diamondsSprite;
    public Sprite clubsSprite;
    public Sprite spadesSprite;
    public Sprite cardBackSprite;
    
    // Card state
    private bool isRevealed = false;
    
    // References to card elements - can be assigned in inspector
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI suitText;
    [SerializeField] private Image suitImage;
    
    void Awake()
    {
        // Attempt to find references if not assigned in inspector
        FindReferences();
        
        // Log warnings for missing components
        ValidateReferences();
    }
    
    private void FindReferences()
    {
        // Find CardFront and CardBack
        if (cardFront == null)
        {
            cardFront = transform.Find("CardFront")?.gameObject;
            // If no CardFront, we'll use the main card as the front
            if (cardFront == null) cardFront = gameObject;
        }
        
        if (cardBack == null)
        {
            cardBack = transform.Find("CardBack")?.gameObject;
            // If no CardBack exists yet, create one
            if (cardBack == null)
            {
                cardBack = new GameObject("CardBack");
                cardBack.transform.SetParent(transform);
                cardBack.transform.localPosition = Vector3.zero;
                Image backImage = cardBack.AddComponent<Image>();
                backImage.sprite = cardBackSprite;
                RectTransform rt = cardBack.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                }
            }
        }
        
        // Find UI components if not assigned
        if (valueText == null)
        {
            // First try to find by name
            valueText = transform.Find("ValueText")?.GetComponent<TextMeshProUGUI>();
            
            // If still null, try to find any TextMeshProUGUI that could be the value text
            if (valueText == null)
            {
                TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length > 0) valueText = texts[0]; // Assume first text is value
            }
        }
        
        if (suitText == null)
        {
            suitText = transform.Find("SuitText")?.GetComponent<TextMeshProUGUI>();
            if (suitText == null)
            {
                TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length > 1) suitText = texts[1]; // Assume second text is suit
            }
        }
        
        if (suitImage == null)
        {
            suitImage = transform.Find("SuitImage")?.GetComponent<Image>();
            if (suitImage == null)
            {
                // Find any image that isn't the card back
                Image[] images = GetComponentsInChildren<Image>();
                foreach (Image img in images)
                {
                    if (img.gameObject != cardBack && img.gameObject.name != "CardBack")
                    {
                        suitImage = img;
                        break;
                    }
                }
            }
        }
    }
    
    private void ValidateReferences()
    {
        if (valueText == null) Debug.LogWarning("ValueText not found on card " + gameObject.name);
        if (suitText == null) Debug.LogWarning("SuitText not found on card " + gameObject.name);
        if (suitImage == null) Debug.LogWarning("SuitImage not found on card " + gameObject.name);
        if (cardFront == null) Debug.LogWarning("CardFront not found on card " + gameObject.name);
        if (cardBack == null) Debug.LogWarning("CardBack not found on card " + gameObject.name);
    }
    
    void Start()
    {
        // Initially hide the card face
        HideCard();
    }
    
    public void SetCard(string suit, int value)
    {
        this.suit = suit;
        this.value = value;
        
        if (valueText != null)
            valueText.text = GetValueDisplay(value);
        
        if (suitText != null)
            suitText.text = suit;
        
        UpdateArtwork();
    }
    
    private string GetValueDisplay(int value)
    {
        // Convert numeric values to card labels
        switch (value)
        {
            case 1: return "A";
            case 11: return "J";
            case 12: return "Q";
            case 13: return "K";
            default: return value.ToString();
        }
    }
    
    void UpdateArtwork()
    {
        if (suitImage == null) return;
        
        switch (suit)
        {
            case "hearts":
                suitImage.sprite = heartsSprite;
                break;
            case "diamonds":
                suitImage.sprite = diamondsSprite;
                break;
            case "clubs":
                suitImage.sprite = clubsSprite;
                break;
            case "spades":
                suitImage.sprite = spadesSprite;
                break;
        }
    }
    
    // Show the card's face
    public void RevealCard()
    {
        isRevealed = true;
        if (cardFront != null && cardFront != gameObject) cardFront.SetActive(true);
        if (cardBack != null) cardBack.SetActive(false);
        
        // If cardFront is the same as the main card, enable individual elements
        if (cardFront == gameObject)
        {
            if (valueText != null) valueText.gameObject.SetActive(true);
            if (suitText != null) suitText.gameObject.SetActive(true);
            if (suitImage != null) suitImage.gameObject.SetActive(true);
        }
    }
    
    // Hide the card's face (show the back)
    public void HideCard()
    {
        isRevealed = false;
        if (cardFront != null && cardFront != gameObject) cardFront.SetActive(false);
        if (cardBack != null) cardBack.SetActive(true);
        
        // If cardFront is the same as the main card, disable individual elements
        if (cardFront == gameObject)
        {
            if (valueText != null) valueText.gameObject.SetActive(false);
            if (suitText != null) suitText.gameObject.SetActive(false);
            if (suitImage != null) suitImage.gameObject.SetActive(false);
        }
    }
    
    // Toggle the card between revealed and hidden
    public void FlipCard()
    {
        if (isRevealed)
            HideCard();
        else
            RevealCard();
    }
    
    // Check if the card is currently revealed
    public bool IsRevealed()
    {
        return isRevealed;
    }
}
