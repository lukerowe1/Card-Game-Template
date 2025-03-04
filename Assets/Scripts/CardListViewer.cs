using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardListViewer : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI deckListText;
    public TextMeshProUGUI playedCardsText;
    
    [Header("Settings")]
    public bool showFullDeck = true;
    public int maxCardsToShow = 10; // Limit display length
    
    private GameManager gameManager;
    private List<Card> lastFlippedCards = new List<Card>();
    
    void Start()
    {
        gameManager = GameManager.gm;
        if (gameManager == null)
        {
            Debug.LogError("CardListViewer: GameManager not found!");
            enabled = false;
            return;
        }
        
        // Create UI elements if not assigned
        if (deckListText == null)
        {
            Debug.LogWarning("Deck List Text not assigned - viewer may not function correctly");
        }
        
        if (playedCardsText == null)
        {
            Debug.LogWarning("Played Cards Text not assigned - viewer may not function correctly");
        }
        
        // Initialize the lists
        UpdateCardLists();
    }
    
    void Update()
    {
        // Check if a new card was flipped
        if (gameManager.currentRevealedCard != null && 
            (lastFlippedCards.Count == 0 || gameManager.currentRevealedCard != lastFlippedCards[lastFlippedCards.Count-1]))
        {
            // Add it to our tracking list
            lastFlippedCards.Add(gameManager.currentRevealedCard);
            
            // Update the displayed lists
            UpdateCardLists();
        }
    }
    
    void UpdateCardLists()
    {
        if (deckListText != null)
        {
            // Build deck list text
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Remaining Deck:");
            
            int count = 0;
            foreach (Card card in gameManager.deck)
            {
                count++;
                if (showFullDeck || count <= maxCardsToShow)
                {
                    sb.AppendLine($"• {GetCardName(card)}");
                }
                
                if (!showFullDeck && count == maxCardsToShow && gameManager.deck.Count > maxCardsToShow)
                {
                    sb.AppendLine($"• ...and {gameManager.deck.Count - maxCardsToShow} more cards");
                    break;
                }
            }
            
            deckListText.text = sb.ToString();
        }
        
        if (playedCardsText != null)
        {
            // Build played cards list text
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Played Cards:");
            
            if (lastFlippedCards.Count == 0)
            {
                sb.AppendLine("• No cards played yet");
            }
            else
            {
                foreach (Card card in lastFlippedCards)
                {
                    sb.AppendLine($"• {GetCardName(card)}");
                }
            }
            
            playedCardsText.text = sb.ToString();
        }
    }
    
    private string GetCardName(Card card)
    {
        return $"{GetValueName(card.value)} of {card.suit}";
    }
    
    private string GetValueName(int value)
    {
        switch (value)
        {
            case 1: return "Ace";
            case 11: return "Jack";
            case 12: return "Queen";
            case 13: return "King";
            default: return value.ToString();
        }
    }
} 