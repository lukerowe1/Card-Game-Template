using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardParent; // Where to place the cards
    public float cardSpacing = 0.5f; // Spacing between cards
    public bool layoutInGrid = true; // Whether to lay out cards in a grid or stack
    
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
        
        if (!cardPrefab.GetComponent<Card>())
        {
            Debug.LogError("Card prefab does not have a Card component!");
            return false;
        }
        
        return true;
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
        
        Debug.Log($"Created deck with {deck.Count} cards");
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
        
        Debug.Log("Deck shuffled");
    }
    
    void LayoutDeck()
    {
        if (layoutInGrid)
        {
            // Layout in grid
            int cardsPerRow = 13;
            for (int i = 0; i < deck.Count; i++)
            {
                int row = i / cardsPerRow;
                int col = i % cardsPerRow;
                
                if (deck[i] != null && deck[i].transform != null)
                {
                    RectTransform rt = deck[i].GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        // For UI cards
                        rt.anchoredPosition = new Vector2(col * cardSpacing, -row * cardSpacing);
                    }
                    else
                    {
                        // For non-UI cards
                        deck[i].transform.localPosition = new Vector3(col * cardSpacing, -row * cardSpacing, 0);
                    }
                }
            }
        }
        else
        {
            // Stack the cards
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i] != null && deck[i].transform != null)
                {
                    RectTransform rt = deck[i].GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        // For UI cards
                        rt.anchoredPosition = new Vector2(0, -i * 0.01f);
                    }
                    else
                    {
                        // For non-UI cards
                        deck[i].transform.localPosition = new Vector3(0, 0, -i * 0.01f);
                    }
                }
            }
        }
        
        Debug.Log("Cards laid out in scene");
    }
    
    void RevealNextCard()
    {
        // Find the first card that hasn't been revealed yet
        foreach (Card card in deck)
        {
            if (card != null && !card.IsRevealed())
            {
                card.RevealCard();
                revealedCards.Add(card);
                Debug.Log($"Revealed card: {card.suit} {card.value}");
                return;
            }
        }
        
        Debug.Log("No more cards to reveal!");
    }
} 