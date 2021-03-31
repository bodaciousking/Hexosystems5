using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public List<Card> aiHand = new List<Card>();

    public static Hands instance;
    
    public void ClearHand()
    {
        hand.Clear();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many Hands scripts!");
            return;
        }
        instance = this;
    }
}
