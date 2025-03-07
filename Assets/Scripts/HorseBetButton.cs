using UnityEngine;

public class HorseBetButton : MonoBehaviour
{
    public string horseSuit; // Set this in the Inspector (hearts, diamonds, etc.)
    
    void OnMouseDown()
    {
        if (GameManager.gm != null)
        {
            GameManager.gm.PlaceBet(horseSuit);
        }
    }
} 