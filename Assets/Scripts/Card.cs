using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string suit; // Suit of the card (hearts, diamonds, clubs, spades)
    public int value; // Value of the card (e.g., 1 for Ace, 11 for Jack, etc.)
    public Sprite heartsSprite;
    public Sprite diamondsSprite;
    public Sprite clubsSprite;
    public Sprite spadesSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCard(string suit, int value)
    {
        this.suit = suit;
        this.value = value;
        UpdateArtwork();
    }

    void UpdateArtwork()
    {
        switch (suit)
        {
            case "hearts":
                spriteRenderer.sprite = heartsSprite;
                break;
            case "diamonds":
                spriteRenderer.sprite = diamondsSprite;
                break;
            case "clubs":
                spriteRenderer.sprite = clubsSprite;
                break;
            case "spades":
                spriteRenderer.sprite = spadesSprite;
                break;
        }
    }
}
