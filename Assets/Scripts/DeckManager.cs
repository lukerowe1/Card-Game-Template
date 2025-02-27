using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent;
    public float cardSpacing = 100f; // Increased spacing for better visibility
    public Vector2 revealedCardPosition = new Vector2(800, 0);
    public float revealedCardSpacing = 50f;
    
    private List<Card> deck = new List<Card>();
    private List<Card> revealedCards = new List<Card>();
    
    private string[] suits = { "hearts", "diamonds", "clubs", "spades" };
    private int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
    
    void Start()
    {
        if (ValidateSetup())
        {
            CreateDeck();
            ShuffleDeck();
            LayoutDeck();
        }
    }
    
    bool ValidateSetup()
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card prefab not assigned in DeckManager!");
            return false;
        }
        
        if (cardParent == null)
        {
            Debug.LogWarning("Card parent not assigned, using DeckManager as parent.");
            cardParent = transform;
        }
        
        return true;
    }
    
    void Update()
    {
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
        int cardsPerRow = 13;
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i] != null)
            {
                RectTransform rt = deck[i].GetComponent<RectTransform>();
                if (rt != null)
                {
                    int row = i / cardsPerRow;
                    int col = i % cardsPerRow;
                    rt.anchoredPosition = new Vector2(col * cardSpacing, -row * cardSpacing);
                }
            }
        }
    }
    
    void RevealNextCard()
    {
        foreach (Card card in deck)
        {
            if (card != null && !card.IsRevealed())
            {
                // Move card to revealed position with offset based on number of revealed cards
                RectTransform rt = card.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Vector2 position = revealedCardPosition + new Vector2(revealedCards.Count * revealedCardSpacing, 0);
                    rt.anchoredPosition = position;
                }
                
                card.RevealCard();
                revealedCards.Add(card);
                return;
            }
        }
    }
} 