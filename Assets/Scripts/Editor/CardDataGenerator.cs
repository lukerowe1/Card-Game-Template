using UnityEngine;
using UnityEditor;
using System.IO;

public class CardDataGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Card Data")]
    static void GenerateCardData()
    {
        // Make sure we have the required folders
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Cards"))
            AssetDatabase.CreateFolder("Assets/Resources", "Cards");

        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        
        foreach (string suit in suits)
        {
            for (int value = 1; value <= 13; value++)
            {
                // Create card data asset
                Card_data cardData = ScriptableObject.CreateInstance<Card_data>();
                cardData.suit = suit;
                cardData.value = value;
                
                // Try to find the corresponding sprite (adjust path as needed)
                string spritePath = $"Assets/Sprites/Cards/{suit}_{value}";
                cardData.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + ".png");
                
                // Save the asset
                string assetPath = $"Assets/Resources/Cards/{suit}_{value}.asset";
                AssetDatabase.CreateAsset(cardData, assetPath);
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Card data generation complete!");
    }
} 