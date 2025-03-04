using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledDeckManager : MonoBehaviour
{
    void Awake()
    {
        // Disable this GameObject completely
        gameObject.SetActive(false);
    }
} 