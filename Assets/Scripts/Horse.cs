using UnityEngine;

public class Horse : MonoBehaviour
{
    public string suit; // Suit associated with the horse (hearts, diamonds, clubs, spades)
    public float moveDistance = 1.0f; // Distance to move forward

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialization code here
    }

    // Update is called once per frame
    void Update()
    {
        // Update code here
    }

    // Method to move the horse forward
    public void MoveForward()
    {
        transform.Translate(Vector3.forward * moveDistance);
    }
}