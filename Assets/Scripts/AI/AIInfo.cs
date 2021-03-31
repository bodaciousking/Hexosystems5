using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInfo : MonoBehaviour
{
    public List<Hextile> enemyTiles = new List<Hextile>();
    public List<Hextile> endangeredTiles = new List<Hextile>();
    public List<Card> aiHand = new List<Card>();

    public static AIInfo instance;
    public int aiEnergy;

    [Header("                                                     atk                   def                recon")]
    public Vector3 priorities; // x = atk, y = def, z = recon
    public Vector3 bias; // x = atk, y = def, z = recon

    Decks decks;

    private void Start()
    {
        decks = GetComponent<Decks>();
    }
    public void DeterminePriorities()
    {
        ResetPriorities();
        foreach (Hextile hextile in enemyTiles) // the more enemy tiles we know of, the more we need to attack
        {
            priorities.x+=3;
            priorities.z--;
        }
        foreach (Hextile hex in endangeredTiles) // the more cities we have exposed, the more we need to prioritize defence
        {
            priorities.x--;
            priorities.z--;
            priorities.y++;
        }
        if(enemyTiles.Count == 0) //If we dont see any enemy cities, we need to prioritize recon
        {
            priorities.z += 5;
        }
    }

    public IEnumerator AiPlayCards()
    {
        for (int i = 0; i < aiHand.Count; i++)
        {
            Card card = aiHand[i];
            if (card.energyCost > aiEnergy)
            {
                Debug.Log(card.cardName + " costed too much.");
                continue;
            }
            else
            {
                card.PlayCard(true);
                aiEnergy -= card.energyCost;
            }
            yield return new WaitForSeconds(0.01f);
        }
        aiHand.Clear();
    }

    public void StartPlayCards()
    {
        StopCoroutine(AiPlayCards());
        StartCoroutine(AiPlayCards());
    }


    public void AIDrawCard()
    {
        if(priorities.z > priorities.x && priorities.z > priorities.y)
        {
            Debug.Log("Drawing from recon.");
            decks.aiDrawCard(decks.aiReconDeck);
            priorities.z--;
        }
        else if(priorities.x > priorities.z && priorities.x > priorities.y)
        {
            Debug.Log("Drawing from attack.");
            decks.aiDrawCard(decks.aiAttackDeck);
            priorities.x--;
        }
        else if(priorities.y > priorities.x && priorities.y > priorities.z)
        {
            Debug.Log("Drawing from defence.");
            decks.aiDrawCard(decks.aiDefenceDeck);
            priorities.y--;
        }
        else //There's a tie. draw from a random deck
        {
            Debug.Log("Drew random card.");
            int randomNum = Random.Range(1, 3);
            if(randomNum == 1)
                decks.aiDrawCard(decks.aiReconDeck);
            if(randomNum == 2)
                decks.aiDrawCard(decks.aiAttackDeck);
            if(randomNum == 3)
                decks.aiDrawCard(decks.aiDefenceDeck);
        }

        int nA = 0; int nD = 0; int nR=0;
        for (int i = 0; i < aiHand.Count; i++)
        {
            if (aiHand[i].cardType == 0)
                nA++;
            if (aiHand[i].cardType == 1)
                nD++;
            if (aiHand[i].cardType == 2)
                nR++;
        }
        //Debug.Log(nA + " a cards, " + nD + " d cards, " + nR + " r cards for a total of " + aiHand.Count + " cards");
    }

    public void ResetPriorities()
    {
        priorities.x = bias.x;
        priorities.y = bias.y;
        priorities.z = bias.z;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Too many AIInfo scripts!");
            return;
        }
        instance = this;
    }
}
