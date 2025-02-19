using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public GameObject cardPrefab; // Reference to the card prefab
    public GameObject horseHearts;
    public GameObject horseDiamonds;
    public GameObject horseClubs;
    public GameObject horseSpades;
    public Transform deckParent; // Reference to the Deck GameObject

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
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                GameObject cardObject = Instantiate(cardPrefab, deckParent);
                //cardObject.transform.SetParent(canvas.transform);
                Card card = cardObject.GetComponent<Card>();
                //card.suit = suits[i];
               // card.value = j;
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