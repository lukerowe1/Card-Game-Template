using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    // Card data
    private string _suit;
    private int _value;
    public string suit { get { return _suit; } }
    public int value { get { return _value; } }
    
    // Card visuals
    public Sprite cardBackSprite;
    
    // State
    private bool isRevealed = false;
    
    // Card components
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    private TextMeshProUGUI valueText;
    private TextMeshProUGUI suitText;
    private Image suitImage;
    private Image backgroundImage;
    
    void Awake()
    {
        // Find CardBack and CardFront containers
        if (cardBack == null) cardBack = transform.Find("CardBack")?.gameObject;
        if (cardFront == null) cardFront = transform.Find("CardFront")?.gameObject;
        
        if (cardFront != null)
        {
            // Find components within CardFront
            valueText = cardFront.transform.Find("ValueText")?.GetComponent<TextMeshProUGUI>();
            suitText = cardFront.transform.Find("SuitText")?.GetComponent<TextMeshProUGUI>();
            suitImage = cardFront.transform.Find("SuitImage")?.GetComponent<Image>();
            backgroundImage = cardFront.transform.Find("Background")?.GetComponent<Image>();
        }
        
        // Make sure CardBack has a sprite
        if (cardBack != null)
        {
            Image cardBackImage = cardBack.GetComponent<Image>();
            if (cardBackImage != null && cardBackImage.sprite == null)
            {
                cardBackImage.sprite = cardBackSprite;
            }
        }
        
        // Debug log what we found
        Debug.Log($"Card setup: CardFront={cardFront!=null}, CardBack={cardBack!=null}, " +
                 $"ValueText={valueText!=null}, SuitText={suitText!=null}, " +
                 $"SuitImage={suitImage!=null}, Background={backgroundImage!=null}");
    }
    
    void Start()
    {
        HideCard();
    }
    
    public void SetCard(string suit, int value)
    {
        this._suit = suit;
        this._value = value;
        
        if (valueText != null) valueText.text = GetValueDisplay(value);
        if (suitText != null) suitText.text = suit;
    }
    
    private string GetValueDisplay(int value)
    {
        switch (value)
        {
            case 1: return "A";
            case 11: return "J";
            case 12: return "Q";
            case 13: return "K";
            default: return value.ToString();
        }
    }
    
    public void RevealCard()
    {
        isRevealed = true;
        
        // Show front, hide back
        if (cardFront != null) cardFront.SetActive(true);
        if (cardBack != null) cardBack.SetActive(false);
    }
    
    public void HideCard()
    {
        isRevealed = false;
        
        // Hide front, show back
        if (cardFront != null) cardFront.SetActive(false);
        if (cardBack != null) cardBack.SetActive(true);
    }
    
    public bool IsRevealed()
    {
        return isRevealed;
    }
    
    public void SetPosition(Vector2 position)
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = position;
        }
        else
        {
            Debug.LogError($"Card {gameObject.name} missing RectTransform!");
        }
    }
    
    // Alternative method for Vector3 positioning
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        Debug.Log($"World Card {gameObject.name} positioned at {position}");
    }
}
