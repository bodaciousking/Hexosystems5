using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cards : MonoBehaviour
{
    
}

public class Card
{
    public int cardID;
    public string cardName;
    public int cardType;
    public int energyCost;

    public TargetType targetType; 
    public int targetSize;
    public int numTargets;

    public virtual void PlayCard(bool playedByAI)
    {
        Debug.Log("Playing " + cardName + "...");
        DeckHandUI dHUI = DeckHandUI.instance;
        if (!playedByAI)
        {
            Hands hand = Hands.instance;
            hand.hand.Remove(this);
            dHUI.DrawHandUI();
            dHUI.DisableHandUI();
        }
    }

    public enum TargetType
    {
        random, selectTarget, noTarget
    }
}

public class AttackCard : Card
{
    public int damageDealt;
}

public class DefenceCard : Card
{
    public int shieldsRestored;
    public int shieldType; // 0 = Decaying shield. 1 = Permanent shield.
}

public class ReconCard : Card
{
    public int visionDuration;
}

/////// ATTACK CARD LIST 
public class ScatterShot : AttackCard
{
    public ScatterShot()
    {
        cardID = 001;
        cardType = 0;
        cardName = "Scatter Shot";
        energyCost = 2;
        targetType = TargetType.random;
        numTargets = 4;
        damageDealt = 1;
    }

    public override void PlayCard(bool playedByAI)
    {
        base.PlayCard(playedByAI);


        GameObject clientMaster = GameObject.Find("ClientMaster");
        GameObject enemyMapTransform;

        if (!playedByAI)
            enemyMapTransform = clientMaster.transform.Find("Player 1 Map").gameObject;
        else
            enemyMapTransform = clientMaster.transform.Find("Player 0 Map").gameObject;

        GameObject enemyPlanetTransform = enemyMapTransform.transform.Find("Planet(Clone)").gameObject;

        Planet enemyPlanet = enemyPlanetTransform.GetComponent<Planet>();

        List<Hextile> enemyTileList = enemyPlanet.hextileList;
        List<Hextile> selectedTargets = new List<Hextile>();

        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, enemyTileList.Count).OrderBy(x => rnd.Next()).Take(4).ToList();


        for (int i = 0; i < randomNumbers.Count; i++)
        {
            selectedTargets.Add(enemyTileList[randomNumbers[i]]);
        }

        DeckHandUI dhUI = DeckHandUI.instance;
        dhUI.EnableHandUI();

        ScatterShotAction ssa = new ScatterShotAction();
        ssa.targets = selectedTargets;
        ssa.actionName = "Scatter Shot";
        ssa.damage = damageDealt;
        ssa.actionType = 0;

        ResolutionPhase rP = ResolutionPhase.instance;
        rP.attackActions.Add(ssa); 
        
    }
}

/////// DEFENCE CARD LIST
public class EmergencyShield : DefenceCard
{
    public EmergencyShield()
    {
        cardID = 101;
        cardType = 1;
        cardName = "Emergency Shield";
        energyCost = 3;
        targetType = TargetType.selectTarget;
        numTargets = 1;
        shieldType = 0;
        shieldsRestored = 2;
    }
    public override void PlayCard(bool playedByAI)
    {
        base.PlayCard(playedByAI);

        if (!playedByAI)
        {
            Targetting targetting = Targetting.instance;
            targetting.SelectObjectAoE(0);
            targetting.currentCondition = Targetting.TargetCondition.isFriendlyCity;

            EmergencyShieldAction emergencyShieldAction = new EmergencyShieldAction();
            emergencyShieldAction.actionName = "Emergency Shield";
            emergencyShieldAction.actionType = 1;
            emergencyShieldAction.effectedCity = null;
            emergencyShieldAction.shieldStrength = shieldsRestored;

            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedDefenceAction = emergencyShieldAction;
        }
    }
}

    /////// RECON CARD LIST
    public class BraveExplorers : ReconCard
{
    public BraveExplorers()
    {
        cardID = 201;
        cardType = 2;
        cardName = "Brave Explorers";
        energyCost = 2;
        targetType = TargetType.selectTarget;
        numTargets = 4;
        visionDuration = 1;
    }

    public override void PlayCard(bool playedByAI)
    {
        base.PlayCard(playedByAI);

        GameObject clientMaster = GameObject.Find("ClientMaster");
        GameObject enemyMapTransform;
        if (!playedByAI)
            enemyMapTransform = clientMaster.transform.Find("Player 1 Map").gameObject;
        else
            enemyMapTransform = clientMaster.transform.Find("Player 0 Map").gameObject;

        GameObject enemyPlanetTransform = enemyMapTransform.transform.Find("Planet(Clone)").gameObject;
        Planet enemyPlanet = enemyPlanetTransform.GetComponent<Planet>();

        List<Hextile> enemyTileList = enemyPlanet.hextileList;
        List<Hextile> selectedTargets = new List<Hextile>();

        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, enemyTileList.Count).OrderBy(x => rnd.Next()).Take(4).ToList();

        Debug.Log(randomNumbers[0]);
        for (int i = 0; i < randomNumbers.Count; i++)
        {
            selectedTargets.Add(enemyTileList[randomNumbers[i]]);
        }

        DeckHandUI dhUI = DeckHandUI.instance;
        dhUI.EnableHandUI();


        BraveExplorersAction bea = new BraveExplorersAction(playedByAI);
        bea.targets = selectedTargets;
        bea.actionName = "Brave Explorers";
        bea.actionType = 2;

        ResolutionPhase rP = ResolutionPhase.instance;
        rP.reconActions.Add(bea);
    }
}

