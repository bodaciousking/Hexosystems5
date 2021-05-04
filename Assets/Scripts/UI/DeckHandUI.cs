using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckHandUI : MonoBehaviour
{
    public GameObject deckUI;
    public Transform handHolder;
    public Transform enemyHandHolder;
    public GameObject handCardButton;
    Decks decksScript;
    Hands playerHand;
    Cards cards;
    public static DeckHandUI instance;
    public Sprite aBack, dBack, Rback;

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
        Debug.Log("Hiding hand UI.");
        handHolder.gameObject.SetActive(false);
    }
    
    public void EnableAIHandUI()
    {
        handHolder.gameObject.SetActive(true);
    }
    public void DisableAIHandUI()
    {
        handHolder.gameObject.SetActive(false);
    }

    public void DrawHiddenHandUI()
    {
        GridLayoutGroup gLG = handHolder.GetComponent<GridLayoutGroup>();
        gLG.enabled = true;
        foreach (Transform item in handHolder)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerHand.hand.Count; i++)
        {
            Card cardToDraw = playerHand.hand[i];
            GameObject newCardButton = Instantiate(handCardButton);
            newCardButton.transform.SetParent(handHolder);
            newCardButton.transform.localScale = Vector3.one;
            Image cardImage = newCardButton.GetComponent<Image>();
            TextMeshProUGUI[] text = newCardButton.GetComponentsInChildren<TextMeshProUGUI>();
            for (int j = 0; j < text.Length; j++)
            {
                text[j].gameObject.SetActive(false);
            }
            switch (cardToDraw.cardType)
            {
                case 0:
                    cardImage.sprite = aBack;
                    break;
                case 1:
                    cardImage.sprite = dBack;
                    break;
                case 2:
                    cardImage.sprite = Rback;
                    break;
            }
            Button actualButton = newCardButton.GetComponent<Button>();
            actualButton.onClick.AddListener(() => ReturnCard(cardToDraw));
        }
    }
    public void ReturnCard(Card cardToReturn)
    {
        playerHand.hand.Remove(cardToReturn); 
        switch (cardToReturn.cardType)
        {
            case 0:
                decksScript.attackDeck.Enqueue(cardToReturn);
                break;
            case 1:
                decksScript.defenceDeck.Enqueue(cardToReturn);
                break;
            case 2:
                decksScript.reconDeck.Enqueue(cardToReturn);
                break;
        }
        DrawHiddenHandUI();
    }

    public void DrawRevealedHandUI()
    {
        GridLayoutGroup gLG = handHolder.GetComponent<GridLayoutGroup>();
        gLG.enabled = true;

        foreach (Transform item in handHolder)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerHand.hand.Count; i++)
        {
            Card cardToDraw = playerHand.hand[i];
            if (CityHandler.instance.generatedEnergy > cardToDraw.energyCost)
            {
                GameObject newCardButton = Instantiate(handCardButton);
                newCardButton.transform.SetParent(handHolder);
                newCardButton.transform.localScale = Vector3.one;
                CardZoom cZ = newCardButton.GetComponent<CardZoom>();
                cZ.revealed = true;
                Image cardImage = newCardButton.GetComponent<Image>();
                //cardImage.sprite = cards.cardImages[cardToDraw.imageReference]; // For drawing Card Fronts!!
                TextMeshProUGUI[] text = newCardButton.GetComponentsInChildren<TextMeshProUGUI>();
                text[0].text = cardToDraw.cardName;
                text[0].color = Color.white;
                text[1].text = cardToDraw.cardDescr;
                text[1].color = Color.white;
                text[2].text = cardToDraw.energyCostText;
                text[2].color = Color.white;

                Button actualButton = newCardButton.GetComponent<Button>();
                actualButton.onClick.AddListener(() => cardToDraw.PlayCard(false));
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHand = GameObject.Find("ClientMaster").GetComponent<Hands>();
        cards = Cards.instance;
        decksScript = GameObject.Find("ClientMaster").GetComponent<Decks>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
