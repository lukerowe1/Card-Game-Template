using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public string suit; // Suit of the card (hearts, diamonds, clubs, spades)
    public int value; // Value of the card (e.g., 1 for Ace, 11 for Jack, etc.)
    public Sprite heartsSprite;
    public Sprite diamondsSprite;
    public Sprite clubsSprite;
    public Sprite spadesSprite;
    public Sprite cardBackSprite; // Red card back sprite

    // Card state
    private bool isRevealed = false;
    
    // References to card elements
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    private TextMeshProUGUI valueText;
    private TextMeshProUGUI suitText;
    private Image suitImage;

    void Awake()
    {
        // Get references to card elements
        valueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        suitText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        suitImage = transform.GetChild(3).GetComponent<Image>();
        
        // Find or create card front/back objects if not assigned
        if (cardFront == null) cardFront = transform.Find("CardFront")?.gameObject;
        if (cardBack == null) cardBack = transform.Find("CardBack")?.gameObject;
        
        // If we still don't have references, the children might be differently named
        if (cardFront == null) Debug.LogWarning("CardFront object not found. Please assign it in the inspector.");
        if (cardBack == null) Debug.LogWarning("CardBack object not found. Please assign it in the inspector.");
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
        valueText.text = GetValueDisplay(value);
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
        if (cardFront != null) cardFront.SetActive(true);
        if (cardBack != null) cardBack.SetActive(false);
    }

    // Hide the card's face (show the back)
    public void HideCard()
    {
        isRevealed = false;
        if (cardFront != null) cardFront.SetActive(false);
        if (cardBack != null) cardBack.SetActive(true);
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
