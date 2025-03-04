using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListMonitor : MonoBehaviour
{
    [Header("Live Card Data (Read Only)")]
    [SerializeField] private List<string> deckCardNames = new List<string>();
    [SerializeField] private List<string> playedCardNames = new List<string>();
    
    private GameManager gameManager;
    private Card lastRevealedCard;
    
    void Start()
    {
        gameManager = GameManager.gm;
        UpdateCardLists();
    }
    
    void Update()
    {
        // Only update when the current revealed card changes
        if (gameManager != null && gameManager.currentRevealedCard != lastRevealedCard)
        {
            lastRevealedCard = gameManager.currentRevealedCard;
            UpdateCardLists();
        }
    }
    
    void UpdateCardLists()
    {
        if (gameManager == null) return;
        
        // Update deck list
        deckCardNames.Clear();
        foreach (Card card in gameManager.deck)
        {
            deckCardNames.Add($"{GetValueName(card.value)} of {card.suit}");
        }
        
        // Update played cards list (extract from revealed card)
        if (gameManager.currentRevealedCard != null)
        {
            Card card = gameManager.currentRevealedCard;
            playedCardNames.Add($"{GetValueName(card.value)} of {card.suit}");
        }
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