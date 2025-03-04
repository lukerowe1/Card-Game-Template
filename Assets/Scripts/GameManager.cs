using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public GameObject blankCardPrefab;
    public GameObject horseHearts;
    public GameObject horseDiamonds;
    public GameObject horseClubs;
    public GameObject horseSpades;
    public Transform deckParent;
    
    // Position for the revealed card
    Vector2 deckPosition = new Vector2(450, 400);     // Where the deck appears
    Vector2 revealPosition = new Vector2(150, 400);   // Where the revealed card appears
    
    // Keep track of revealed card
    public Card currentRevealedCard;
    public GameObject canvas;


    private void Awake()
    {
        if (gm != null && gm != this)
            Destroy(gameObject);
        else
            gm = this;
    }

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        PositionDeck();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipNextCard();
        }
    }

    void CreateDeck()
    {
        // Make sure deckParent is a child of a Canvas
        if (deckParent == null || deckParent.GetComponentInParent<Canvas>() == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = new GameObject("Card Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<CanvasScaler>();
                canvas.gameObject.AddComponent<GraphicRaycaster>();
            }
            
            GameObject deckObj = new GameObject("Card Deck");
            deckObj.transform.SetParent(canvas.transform, false);
            deckParent = deckObj.transform;
        }
        
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        
        // Remove Canvas references since we're using world-space
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                // Create card as child of deckParent
                GameObject cardObject = Instantiate(blankCardPrefab, deckParent);
                
                Card card = cardObject.GetComponent<Card>();
                if (card == null)
                {
                    Debug.LogError("Card script missing from prefab!");
                    continue;
                }
                
                // Set the card data
                card.SetCard(suits[i], j);
                card.HideCard();
                
                // Rename the card
                cardObject.name = GetCardName(j, suits[i]);
                
                deck.Add(card);
            }
        }
        
        Debug.Log($"Created deck with {deck.Count} cards");
    }

    // Helper method to get nice card names
    private string GetCardName(int value, string suit)
    {
        string valueName;
        switch (value)
        {
            case 1: valueName = "Ace"; break;
            case 11: valueName = "Jack"; break;
            case 12: valueName = "Queen"; break;
            case 13: valueName = "King"; break;
            default: valueName = value.ToString(); break;
        }
        
        return $"{valueName} of {suit}";
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
    
    void PositionDeck()
    {
        // Convert UI coordinates to world coordinates
        Vector3 worldDeckPos = new Vector3(deckPosition.x / 100, deckPosition.y / 100, 0);
        
        // Position all cards in a stack
        for (int i = 0; i < deck.Count; i++)
        {
            // Add slight z-offset to stack cards visually
            Vector3 cardPos = worldDeckPos + new Vector3(0, 0, -0.01f * i);
            deck[i].SetPosition(cardPos);
        }
    }

    void FlipNextCard()
    {
        if (deck.Count > 0)
        {
            // Get the top card
            Card nextCard = deck[0];
            deck.RemoveAt(0);
            
            // Move the old revealed card back to the deck if needed
            if (currentRevealedCard != null)
            {
                Destroy(currentRevealedCard.gameObject);
            }
            
            // Set as current revealed card
            currentRevealedCard = nextCard;
            
            // Convert UI coordinates to world coordinates
            Vector3 worldRevealPos = new Vector3(revealPosition.x / 100, revealPosition.y / 100, -1);
            
            // Move it to the reveal position and show it
            try {
                nextCard.transform.SetParent(canvas.transform); // Unparent to position independently
                nextCard.SetPosition(worldRevealPos);
                nextCard.RevealCard();
                
                Debug.Log($"Card flipped: {nextCard.value} of {nextCard.suit}");
            } 
            catch (System.Exception e) {
                Debug.LogError($"Error positioning card: {e.Message}");
            }
            
            // Move the corresponding horse
            MoveHorse(nextCard.suit);
        }
        else
        {
            Debug.Log("No more cards in the deck!");
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