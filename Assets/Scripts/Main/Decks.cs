using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decks : MonoBehaviour
{
    public Queue<Card> attackDeck;
    public Queue<Card> defenceDeck;
    public Queue<Card> reconDeck;

    public Queue<Card> aiAttackDeck;
    public Queue<Card> aiDefenceDeck;
    public Queue<Card> aiReconDeck;

    public List<Card> allAttackCards = new List<Card>();
    public List<Card> allDefenceCards = new List<Card>();
    public List<Card> allReconCards = new List<Card>();

    Hands playerHand;
    DeckHandUI dhUI;
    AIInfo aiInfo;

    private void Start()
    {
        PopulateAttackDeck();
        PopulateDefenceDeck();
        PopulateReconDeck();

        playerHand = GetComponent<Hands>();
        dhUI = GameObject.Find("UIScripts").GetComponent<DeckHandUI>();
        aiInfo = GetComponent<AIInfo>();

        PrepareDecks();
    }

    public void PopulateAttackDeck()
    {
        allAttackCards.Add(new ScatterShot());
        allAttackCards.Add(new ScatterShot());
        allAttackCards.Add(new ScatterShot());
        allAttackCards.Add(new ScatterShot());
        allAttackCards.Add(new ScatterShot());
        allAttackCards.Add(new ScatterShot());
    }
    public void PopulateDefenceDeck()
    {
        allDefenceCards.Add(new EmergencyShield());
    }
    public void PopulateReconDeck()
    {
        allReconCards.Add(new BraveExplorers());
        allReconCards.Add(new BraveExplorers());
        allReconCards.Add(new BraveExplorers());
        allReconCards.Add(new BraveExplorers());
        allReconCards.Add(new BraveExplorers());
    }

    public Card[] ShuffleDeck(List<Card> deck)
    {
        Card[] deckArray = deck.ToArray();
        Card[] shuffledDeck;
        shuffledDeck = Utility.ShuffleArray<Card>(deckArray, Random.Range(1,100));
        return shuffledDeck;
    }
    public void PrepareDecks()
    {
        attackDeck = new Queue<Card>(ShuffleDeck(allAttackCards));
        defenceDeck = new Queue<Card>(ShuffleDeck(allDefenceCards));
        reconDeck = new Queue<Card>(ShuffleDeck(allReconCards));

        aiAttackDeck = new Queue<Card>(ShuffleDeck(allAttackCards));
        aiDefenceDeck = new Queue<Card>(ShuffleDeck(allDefenceCards));
        aiReconDeck = new Queue<Card>(ShuffleDeck(allReconCards));
    }
    public Card aiDrawCard(Queue<Card> deck)
    {
        Card drawnCard;
        if (deck.Count > 0)
        {
            drawnCard = deck.Dequeue();

            if (aiInfo.aiHand.Count < 5)
            {
                aiInfo.aiHand.Add(drawnCard);
                return drawnCard;
            }
            else
            {
                Debug.Log("Hand full.");
                return null;
            }
        }
        else
        {
            Debug.Log("Deck empty.");
            return null;
        }
    }


    public Card DrawCard(Queue<Card> deck)
    {
        Card drawnCard;
        if (deck.Count > 0)
        {
            drawnCard = deck.Dequeue();

            if (playerHand.hand.Count < 5)
            { 
                playerHand.hand.Add(drawnCard);
                dhUI.DrawHandUI();
                return drawnCard;
            }
            else
            {
                Debug.Log("Hand full.");
                return null;
            }
        }
        else
        {
            Debug.Log("Deck empty.");
            return null;
        }
    }

    public void DrawAttackCard()
    {
        DrawCard(attackDeck);
    }
    public void DrawDefenceCard()
    {
        DrawCard(defenceDeck);
    }
    public void DrawReconCard()
    {
        DrawCard(reconDeck);
    }

    /*
    private GameObject[] AttackDeckArr;
    public List<GameObject> AttackDeck;
    public List<GameObject> AttackDiscard;

    private GameObject[] DefenceDeckArr;
    public List<GameObject> DefenceDeck;
    public List<GameObject> DefenceDiscard;

    private GameObject[] ReconDeckArr;
    public List<GameObject> ReconDeck;
    public List<GameObject> ReconDiscard;

    private GameObject temp;

    private int deckSizeAtt;
    private int deckSizeDef;
    private int deckSizeRec;


    // Start is called before the first frame update
    void Start()
    {
        AttackDeckArr = Resources.LoadAll<GameObject>("AttackDeck");
        DefenceDeckArr = Resources.LoadAll<GameObject>("DefenceDeck");
        ReconDeckArr = Resources.LoadAll<GameObject>("ReconDeck");
        loadDecks(AttackDeckArr, AttackDeck);
        loadDecks(DefenceDeckArr, DefenceDeck);
        loadDecks(ReconDeckArr, ReconDeck);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void loadDecks(GameObject[] tempArr, List<GameObject> tempList)
    {
        for (int c = 0; c <= 8; c++)
        {
            for (int i = 0; i <= tempArr.Length - 1; i++)
            {
                temp = tempArr[i];
                tempList.Add(temp);

            }
        }
    }

    public void RemoveLast(string type)
    {
        switch (type)
        {
            case "Attack":
                deckSizeAtt = AttackDeck.Count; 
                AttackDeck.RemoveAt(deckSizeAtt - 1);
                deckSizeAtt--; 



                Debug.Log("Drawing from the attack deck");
                break;

            case "Defence":
                deckSizeDef = DefenceDeck.Count;
                DefenceDeck.RemoveAt(deckSizeDef - 1);
                deckSizeDef--;



                Debug.Log("Drawing from the attack deck");
                break;
            case "recon":
                deckSizeRec = ReconDeck.Count;
                ReconDeck.RemoveAt(deckSizeRec - 1);
                deckSizeRec--;



                Debug.Log("Drawing from the attack deck");
                break;
        }
    }

    */
}
