using UnityEngine;

public class Horse : MonoBehaviour
{
    public string suit; // Suit associated with the horse (hearts, diamonds, clubs, spades)
    public float moveDistance = 10.0f; // Distance to move
    
    // Add this to track movement count
    public int moveCount = 0;
    private bool hasWon = false;

    // Add this variable to store the starting position
    private Vector3 startingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure suit is initialized
        if (string.IsNullOrEmpty(suit))
        {
            Debug.LogWarning("Horse has no suit assigned! Setting to default.");
            suit = "unknown";
        }
        
        // Store the starting position when the game begins
        startingPosition = transform.position;
        Debug.Log($"Horse {gameObject.name} starting position: {startingPosition}");
    }

    // Update is called once per frame
    void Update()
    {
        // Update code here
    }

    // Method to move the horse forward
    public void MoveForward()
    {
        // Move the horse
        transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
        
        // Count the moves
        moveCount++;
        
        // Directly detect wins based on object name instead of suit property
        if (moveCount >= 5 && !hasWon)
        {
            hasWon = true;
            
            string horseSuit = "unknown";
            
            // Get suit from the GameObject name instead of the suit property
            if (gameObject.name.ToLower().Contains("hearts")) horseSuit = "Hearts";
            else if (gameObject.name.ToLower().Contains("diamonds")) horseSuit = "Diamonds";
            else if (gameObject.name.ToLower().Contains("clubs")) horseSuit = "Clubs";
            else if (gameObject.name.ToLower().Contains("spades")) horseSuit = "Spades";
            
            // Call the winner method
            GameManager.gm.DeclareWinner(horseSuit);
            
            Debug.Log($"Horse {gameObject.name} won the race!");
        }
    }

    // Method to reset horse for new game
    public void ResetHorse()
    {
        Debug.Log($"ResetHorse called on {gameObject.name}");
        Debug.Log($"  Before reset - Current position: {transform.position}");
        Debug.Log($"  Saved starting position: {startingPosition}");
        
        moveCount = 0;
        hasWon = false;
        
        // Reset the horse position to where it started
        transform.position = startingPosition;
        
        Debug.Log($"  After reset - New position: {transform.position}");
    }
}