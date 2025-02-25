using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent; // Where to place the cards
    
    private List<Card> deck = new List<Card>();
    private List<Card> revealedCards = new List<Card>();
    
    private string[] suits = { "hearts", "diamonds", "clubs", "spades" };
    private int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
    
    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        LayoutDeck();
    }
    
    void Update()
    {
        // Reveal a card when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RevealNextCard();
        }
    }
    
    void CreateDeck()
    {
        foreach (string suit in suits)
        {
            foreach (int value in values)
            {
                GameObject cardObj = Instantiate(cardPrefab, cardParent);
                Card card = cardObj.GetComponent<Card>();
                card.SetCard(suit, value);
                deck.Add(card);
            }
        }
    }
    
    void ShuffleDeck()
    {
        // Fisher-Yates shuffle algorithm
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    
    void LayoutDeck()
    {
        // Position cards in a grid or stack as needed
        // This is a simple layout - adjust for your specific needs
        float spacing = 0.5f;
        for (int i = 0; i < deck.Count; i++)
        {
            Vector3 position = new Vector3(i % 13 * spacing, -(i / 13) * spacing, 0);
            deck[i].transform.localPosition = position;
        }
    }
    
    void RevealNextCard()
    {
        // Find the first card that hasn't been revealed yet
        foreach (Card card in deck)
        {
            if (!card.IsRevealed())
            {
                card.RevealCard();
                revealedCards.Add(card);
                break;
            }
        }
    }
} 