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

    public GameObject blank;

    TextMeshProUGUI valueText;
    TextMeshProUGUI suitText;
    

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        print("Card Awake");
        valueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        suitText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    public void SetCard(string suit, int value)
    {
        this.suit = suit;
        this.value = value;
        valueText.text = value.ToString();
        suitText.text = suit;
        UpdateArtwork();
    }

    void UpdateArtwork()

    {
        switch (suit)
        {
            case "hearts":
                transform.GetChild(3).GetComponent<Image>().sprite = heartsSprite;
                break;
            case "diamonds":
                transform.GetChild(3).GetComponent<Image>().sprite = diamondsSprite;
                break;
            case "clubs":
                transform.GetChild(3).GetComponent<Image>().sprite = clubsSprite;
                break;
            case "spades":
                transform.GetChild(3).GetComponent<Image>().sprite = spadesSprite;
                break;
        }
    }
}
