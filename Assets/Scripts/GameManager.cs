using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public GameObject horseHearts;
    public GameObject horseDiamonds;
    public GameObject horseClubs;
    public GameObject horseSpades;

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
        // Initialize the deck with cards (this is a placeholder, you need to implement the actual card initialization)
    }

    void ShuffleDeck()
    {
        // Shuffle the deck (this is a placeholder, you need to implement the actual shuffle logic)
    }

    void FlipCard()
    {
        if (deck.Count > 0)
        {
            Card flippedCard = deck[0];
            deck.RemoveAt(0);
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

using UnityEngine;

public class Horse
{
    public string Suit { get; private set; }
    public int Position { get; private set; }

    public Horse(string suit)
    {
        Suit = suit;
        Position = 0;
    }

    public void MoveForward()
    {
        Position++;
    }
}
