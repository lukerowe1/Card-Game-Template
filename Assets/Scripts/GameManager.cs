using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public GameObject blankCardPrefab; // Reference to the blank card prefab
    public GameObject horseHearts;
    public GameObject horseDiamonds;
    public GameObject horseClubs;
    public GameObject horseSpades;
    public Transform deckParent; // Reference to the Deck GameObject
    
    // Add these new variables
    public Vector2 deckPosition = new Vector2(0, 0); // Position of the deck
    public Vector2 revealPosition = new Vector2(800, 0); // Position where the flipped card appears
    public float cardSpacing = 100f; // Spacing between cards in the deck

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
        LayoutDeck(); // Add this line to position the cards
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipCard();
        }
    }

    void InitializeDeck()
    {
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                GameObject cardObject = Instantiate(blankCardPrefab, deckParent);
                Card card = cardObject.GetComponent<Card>();
                card.SetCard(suits[i], j);
                cardObject.name = j + " of " + suits[i];
                deck.Add(card);
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // Add this new method to layout the deck
    void LayoutDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i] != null)
            {
                // Stack cards with a small offset
                Vector2 position = deckPosition + new Vector2(0, -i * 0.1f);
                deck[i].SetPosition(position);
                deck[i].HideCard(); // Make sure all cards start face down
            }
        }
    }

    void FlipCard()
    {
        if (deck.Count > 0)
        {
            Card flippedCard = deck[0];
            deck.RemoveAt(0);
            
            // Move the card to the reveal position and show it
            RectTransform rt = flippedCard.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = revealPosition;
            }
            
            flippedCard.RevealCard();
            MoveHorse(flippedCard.suit);
        }
    }

    void MoveHorse(string suit)
    {
        switch (suit)
        {
            case "hearts":
                horseHearts.GetComponent<Horse>().MoveForward();
                break;
            case "diamonds":
                horseDiamonds.GetComponent<Horse>().MoveForward();
                break;
            case "clubs":
                horseClubs.GetComponent<Horse>().MoveForward();
                break;
            case "spades":
                horseSpades.GetComponent<Horse>().MoveForward();
                break;
        }
    }
}