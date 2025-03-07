using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI winnerText; // Drag your WinnerText object here in Inspector
    private bool gameOver = false;

    // Add these variables to the GameManager class
    public int playerMoney = 100;
    public int currentBet = 0;
    public string betHorse = ""; // Which horse the player bet on
    public TextMesh moneyText; // Will show the player's money
    public bool canPlaceBet = true; // Whether betting is allowed (before a round starts)

    private void Awake()
    {
        if (gm != null && gm != this)
            Destroy(gameObject);
        else
            gm = this;
        
        // Make sure winner text starts unchilded (not in canvas)
        if (winnerText != null)
        {
            winnerText.gameObject.SetActive(false); // Hide it initially
            winnerText.transform.SetParent(null);   // Unchild from the canvas
        }
    }

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        PositionDeck();
        
        // Create money display
        GameObject moneyObj = new GameObject("MoneyDisplay");
        moneyText = moneyObj.AddComponent<TextMesh>();
        moneyText.text = $"Money: ${playerMoney}\nBet: $0";
        moneyText.fontSize = 90;
        moneyText.color = Color.white;
        
        // Position in corner of screen
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Vector3 pos = mainCam.ViewportToWorldPoint(new Vector3(0.1f, 0.9f, 10));
            moneyObj.transform.position = pos;
            moneyObj.transform.forward = mainCam.transform.forward;
            moneyObj.transform.localScale = Vector3.one * 0.15f;
        }
    }

    void Update()
    {
        // Flip card with Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipNextCard();
        }
        
        // Reset game with R - MOVED OUTSIDE CONDITION SO IT ALWAYS WORKS
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed - attempting to reset game");
            ResetGame();
        }
        
        // Add these betting controls
        if (canPlaceBet && !gameOver)
        {
            // Bet on Hearts with 1
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlaceBet("hearts");
            }
            // Bet on Diamonds with 2
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                PlaceBet("diamonds");
            }
            // Bet on Clubs with 3
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                PlaceBet("clubs");
            }
            // Bet on Spades with 4
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                PlaceBet("spades");
            }
        }
    }

    void CreateDeck()
    {
        // Make sure deckParent is a child of a Canvas
        if (deckParent == null || deckParent.GetComponentInParent<Canvas>() == null)
        {
            // Use FindAnyObjectByType instead
            Canvas canvas = FindAnyObjectByType<Canvas>();
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
        if (gameOver) return;
        
        // No more bets once cards start flipping
        canPlaceBet = false;
        
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

    public void DeclareWinner(string suit)
    {
        // Get camera reference once at the beginning
        Camera mainCam = Camera.main;
        
        // Process bet payout
        if (currentBet > 0)
        {
            if (suit.ToLower() == betHorse.ToLower())
            {
                // Player won - pays 4:1
                int winnings = currentBet * 4;
                playerMoney += (winnings + currentBet);
                
                // Create win message
                GameObject betWin = new GameObject("BET_WIN");
                TextMesh betText = betWin.AddComponent<TextMesh>();
                betText.text = $"YOU WON ${winnings}!";
                betText.color = Color.green;
                betText.fontSize = 40;
                
                // Position below winner text (using the mainCam declared at top)
                if (mainCam != null)
                {
                    betWin.transform.position = mainCam.transform.position + mainCam.transform.forward * 5 + Vector3.down * 1;
                    betWin.transform.forward = mainCam.transform.forward;
                    betWin.transform.localScale = Vector3.one * 0.1f;
                }
            }
            else
            {
                // Player lost
                GameObject betLose = new GameObject("BET_LOSE");
                TextMesh betText = betLose.AddComponent<TextMesh>();
                betText.text = $"YOU LOST ${currentBet}!";
                betText.color = Color.red;
                betText.fontSize = 40;
                
                // Using the mainCam from above
                if (mainCam != null)
                {
                    betLose.transform.position = mainCam.transform.position + mainCam.transform.forward * 5 + Vector3.down * 1;
                    betLose.transform.forward = mainCam.transform.forward;
                    betLose.transform.localScale = Vector3.one * 0.1f;
                }
            }
            
            // Reset current bet
            currentBet = 0;
            betHorse = "";
            
            // Update money display
            UpdateMoneyDisplay();
        }
        
        // Create winner text (using mainCam declared at top)
        GameObject winnerObj = new GameObject("WINNER");
        TextMesh textMesh = winnerObj.AddComponent<TextMesh>();
        
        textMesh.text = suit.ToUpper() + " WINS!!!";
        textMesh.fontSize = 80;
        textMesh.color = Color.yellow;
        
        if (mainCam != null)
        {
            winnerObj.transform.position = mainCam.transform.position + mainCam.transform.forward * 5;
            winnerObj.transform.forward = mainCam.transform.forward;
            winnerObj.transform.localScale = Vector3.one * 0.2f;
        }
        else
        {
            winnerObj.transform.position = new Vector3(0, 2, 0);
        }
        
        Debug.Log(suit + " WINS! Winner text created.");
        
        // Stop the game
        gameOver = true;
        canPlaceBet = false;
    }

    public void ResetGame()
    {
        Debug.Log("==== RESET GAME STARTED ====");
        gameOver = false;
        canPlaceBet = true;
        
        // Destroy any winner or bet result text objects
        GameObject winnerText = GameObject.Find("WINNER");
        if (winnerText != null)
        {
            Debug.Log("Destroying winner text");
            Destroy(winnerText);
        }
        
        GameObject betWinText = GameObject.Find("BET_WIN");
        if (betWinText != null)
        {
            Debug.Log("Destroying bet win text");
            Destroy(betWinText);
        }
        
        GameObject betLoseText = GameObject.Find("BET_LOSE");
        if (betLoseText != null)
        {
            Debug.Log("Destroying bet lose text");
            Destroy(betLoseText);
        }
        
        // If any bet was active, reset it
        if (currentBet > 0)
        {
            Debug.Log($"Returning active bet: ${currentBet}");
            playerMoney += currentBet;
            currentBet = 0;
            betHorse = "";
        }
        
        // Update display
        UpdateMoneyDisplay();
        
        // Reset all horses
        Debug.Log("Starting to reset horses...");
        
        if (horseHearts != null) 
        {
            Debug.Log("Resetting Hearts horse");
            horseHearts.GetComponent<Horse>().ResetHorse();
        }
        else
        {
            Debug.LogError("Hearts horse reference is null!");
        }
        
        if (horseDiamonds != null) 
        {
            Debug.Log("Resetting Diamonds horse");
            horseDiamonds.GetComponent<Horse>().ResetHorse();
        }
        else
        {
            Debug.LogError("Diamonds horse reference is null!");
        }
        
        if (horseClubs != null) 
        {
            Debug.Log("Resetting Clubs horse");
            horseClubs.GetComponent<Horse>().ResetHorse();
        }
        else
        {
            Debug.LogError("Clubs horse reference is null!");
        }
        
        if (horseSpades != null) 
        {
            Debug.Log("Resetting Spades horse");
            horseSpades.GetComponent<Horse>().ResetHorse();
        }
        else
        {
            Debug.LogError("Spades horse reference is null!");
        }
        
        Debug.Log("Finished resetting horses");
        
        // Recreate the deck
        deck.Clear();
        CreateDeck();
        ShuffleDeck();
        PositionDeck();
        
        Debug.Log("==== RESET GAME FINISHED ====");
    }

    // Add this method to update the money display
    void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Money: ${playerMoney}\nBet: ${currentBet} on {betHorse}";
        }
    }

    // Add this method to handle placing bets
    public void PlaceBet(string horseSuit)
    {
        // Can only bet between rounds
        if (!canPlaceBet) return;
        if (gameOver) return;
        
        // If betting on a new horse, need to cancel previous bet
        if (currentBet > 0 && betHorse != horseSuit)
        {
            // Return previous bet if switching horses
            playerMoney += currentBet;
            currentBet = 0;
            betHorse = "";
        }
        
        // Add $5 to bet (either new bet or increasing existing bet)
        if (playerMoney >= 5)
        {
            currentBet += 5;
            playerMoney -= 5;
            betHorse = horseSuit;
            Debug.Log($"Bet $5 on {horseSuit}, total bet: ${currentBet}");
        }
        else
        {
            Debug.Log("Not enough money to bet!");
        }
        
        // Update money display
        UpdateMoneyDisplay();
    }
}