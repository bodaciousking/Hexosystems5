using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckHandUI : MonoBehaviour
{
    public GameObject deckUI;
    public Transform handHolder;
    public GameObject handCardButton;
    Hands playerHand;
    public static DeckHandUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many DeckHandUI scripts!");
            return;
        }
        instance = this;
    } //Singleton

    public void EnableDeckUI()
    {
        deckUI.SetActive(true);
    }
    public void DisableDeckUI()
    {
        deckUI.SetActive(false);
    } 
    public void EnableHandUI()
    {
        handHolder.gameObject.SetActive(true);
    }
    public void DisableHandUI()
    {
        handHolder.gameObject.SetActive(false);
    }

    public void DrawHandUI()
    {
        foreach (Transform item in handHolder)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerHand.hand.Count; i++)
        {
            Card cardToDraw = playerHand.hand[i];
            GameObject newCardButton = Instantiate(handCardButton);
            newCardButton.transform.parent = handHolder;
            TextMeshProUGUI text = newCardButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = cardToDraw.cardName;

            Button actualButton = newCardButton.GetComponent<Button>();
            actualButton.onClick.AddListener(() => cardToDraw.PlayCard(false));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHand = GameObject.Find("ClientMaster").GetComponent<Hands>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
