using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    private string _suit;
    private int _value;
    public string suit { get { return _suit; } }
    public int value { get { return _value; } }
    
    public Sprite cardBackSprite;
    
    private bool isRevealed = false;
    
    // References to card elements
    private TextMeshProUGUI valueText;
    private TextMeshProUGUI suitText;
    private Image suitImage;
    private Image backgroundImage;
    private Image cardBackImage;
    
    void Awake()
    {
        // Find all required components
        valueText = transform.Find("ValueText")?.GetComponent<TextMeshProUGUI>();
        suitText = transform.Find("SuitText")?.GetComponent<TextMeshProUGUI>();
        suitImage = transform.Find("SuitImage")?.GetComponent<Image>();
        backgroundImage = GetComponent<Image>();
        cardBackImage = transform.Find("CardBack")?.GetComponent<Image>();
        
        // Create card back if needed
        if (cardBackImage == null && cardBackSprite != null)
        {
            GameObject cardBack = new GameObject("CardBack");
            cardBack.transform.SetParent(transform);
            cardBack.transform.localPosition = Vector3.zero;
            cardBackImage = cardBack.AddComponent<Image>();
            cardBackImage.sprite = cardBackSprite;
            
            RectTransform rt = cardBackImage.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
        }

        // Make sure the card back is in front
        if (cardBackImage != null)
        {
            cardBackImage.transform.SetAsLastSibling();
        }
    }
    
    void Start()
    {
        HideCard();
    }
    
    public void SetCard(string suit, int value)
    {
        this._suit = suit;
        this._value = value;
        
        if (valueText != null)
        {
            valueText.text = GetValueDisplay(value);
            valueText.enabled = true;
        }
        
        if (suitText != null)
        {
            suitText.text = suit;
            suitText.enabled = true;
        }
        
        if (suitImage != null)
        {
            suitImage.enabled = true;
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.enabled = true;
        }
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
        
        if (valueText != null) valueText.enabled = true;
        if (suitText != null) suitText.enabled = true;
        if (suitImage != null) suitImage.enabled = true;
        if (backgroundImage != null) backgroundImage.enabled = true;
        if (cardBackImage != null) cardBackImage.enabled = false;
    }
    
    public void HideCard()
    {
        isRevealed = false;
        
        if (valueText != null) valueText.enabled = false;
        if (suitText != null) suitText.enabled = false;
        if (suitImage != null) suitImage.enabled = false;
        if (backgroundImage != null) backgroundImage.enabled = false;
        if (cardBackImage != null) cardBackImage.enabled = true;
    }
    
    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void SetPosition(Vector3 position)
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = position;
        }
    }
}
