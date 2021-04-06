using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
}

public class Card
{
    public int cardID;
    public string cardName;
    public string cardDescr;
    public string energyCostText;
    public int cardType;
    public int energyCost;
    public CardAction myAction;

    public TargetType targetType; 
    public int targetSize;
    public int numTargets;
    public bool aIPlay;

    public virtual void PlayCard(bool playedByAI)
    {
        aIPlay = playedByAI;
        Debug.Log("Playing " + cardName + "...");
        DeckHandUI dHUI = DeckHandUI.instance;
        CityHandler cH = CityHandler.instance;
        ResolutionUI rUI = ResolutionUI.instance;

        if (!playedByAI)
        {
            Hands hand = Hands.instance;
            hand.hand.Remove(this);
            dHUI.DrawRevealedHandUI();
            dHUI.DisableHandUI();


            if (targetType == TargetType.selectTarget)
            {
                Targetting targetting = Targetting.instance;
                switch (cardType)
                {
                    case 0:
                        targetting.currentCondition = Targetting.TargetCondition.isEnemyTile;
                        targetting.SelectObjectAoE(targetSize);
                        break;
                    case 1:
                        targetting.currentCondition = Targetting.TargetCondition.isFriendlyCity;
                        targetting.SelectObjectAoE(targetSize);
                        break;
                    case 2:
                        targetting.currentCondition = Targetting.TargetCondition.isEnemyTile;
                        targetting.SelectObjectAoE(targetSize);
                        break;
                }
            }
            else
            {
                rUI.playerActions.Add(myAction);
                rUI.DisplayActionButtons(); 
            }

            cH.generatedEnergy -= energyCost;
        }


    }

    public enum TargetType
    {
        random, selectTarget, noTarget
    }

    public List<Hextile> SelectRandomHextile(bool playedByAI)
    {
        Transform enemyMapTransform;
        GameObject clientMaster = GameObject.Find("ClientMaster");

        if (!playedByAI)
            enemyMapTransform = clientMaster.transform.Find("Player 1 Map");
        else
            enemyMapTransform = clientMaster.transform.Find("Player 0 Map");

        GameObject enemyPlanetTransform = enemyMapTransform.transform.Find("Planet(Clone)").gameObject;

        HexoPlanet enemyPlanet = enemyPlanetTransform.GetComponent<HexoPlanet>();

        List<Hextile> hextileList = enemyPlanet.hextileList;

        List<Hextile> selectedTargets = new List<Hextile>();

        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, hextileList.Count).OrderBy(x => rnd.Next()).Take(4).ToList();

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            selectedTargets.Add(hextileList[randomNumbers[i]]);
        }
        return selectedTargets;
    }

    public Hextile SelectTarget()
    {
        Hextile target;
        AIInfo aiInfo = GameObject.Find("ClientMaster").GetComponent<AIInfo>();
        if (aiInfo.enemyTiles.Count > 0)
        {
            target = aiInfo.enemyTiles[0];
        }
        else
        {
            target = SelectRandomHextile(true)[0];
        }
        return target;
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
        cardDescr = "Hit 3 random hexes and apply 1 damage.";
        energyCostText = "2";
        energyCost = 2;
        targetType = TargetType.random;
        numTargets = 4;
        damageDealt = 1;
    }


    public override void PlayCard(bool playedByAI)
    {
        ScatterShotAction ssa = new ScatterShotAction();
        ssa.targets = SelectRandomHextile(playedByAI);
        ssa.actionName = "Scatter Shot";
        ssa.damage = damageDealt;
        ssa.actionType = 0;
        ssa.representedCard = this;
        myAction = ssa;

        base.PlayCard(playedByAI);

        DeckHandUI dhUI = DeckHandUI.instance;
        dhUI.EnableHandUI();

        ResolutionPhase rP = ResolutionPhase.instance;
        rP.attackActions.Add(ssa);
    }
}

public class GaussCannon : AttackCard
{
    public GaussCannon()
    {
        cardID = 002;
        cardType = 0;
        cardName = "Gauss Cannon";
        cardDescr = "Target 1 tile and apply 1 damage.";
        energyCostText = "2";
        energyCost = 2;
        targetType = TargetType.selectTarget;
        numTargets = 1;
        targetSize = 0;
        damageDealt = 1;
    }

    public override void PlayCard(bool playedByAI)
    {
        GuassCannonAction gca = new GuassCannonAction();
        gca.actionName = "Gauss Cannon";
        gca.target = null;
        gca.damage = damageDealt;
        gca.actionType = 0;
        gca.representedCard = this;
        myAction = gca;

        base.PlayCard(playedByAI);

        if (!playedByAI)
        {
            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedAttackAction = gca;
        }
        else
        {
            Debug.Log("Ai playing Gauss Cannon.");
            gca.target = SelectTarget();
            ResolutionPhase rP = ResolutionPhase.instance;
            rP.attackActions.Add(gca);
        }
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
        cardDescr = "Apply 1 Shield point to a target hex. Shield lasts for one turn or until destroyed.";
        energyCost = 3;
        energyCostText = "3";
        targetType = TargetType.selectTarget;
        numTargets = 1;
        shieldType = 0;
        targetSize = 0;
        shieldsRestored = 2;
    }
    public override void PlayCard(bool playedByAI)
    {
        base.PlayCard(playedByAI);

        if (!playedByAI)
        {
            EmergencyShieldAction emergencyShieldAction = new EmergencyShieldAction();
            emergencyShieldAction.actionName = "Emergency Shield";
            emergencyShieldAction.actionType = 1;
            emergencyShieldAction.effectedCity = null;
            emergencyShieldAction.shieldStrength = shieldsRestored;
            emergencyShieldAction.representedCard = this;
            myAction = emergencyShieldAction;

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
        cardDescr = "Scout 3 random hexes. Vision lasts 1 turn.";
        energyCost = 2;
        energyCostText = "2";
        targetType = TargetType.random;
        numTargets = 4;
        visionDuration = 1;
    }

    public override void PlayCard(bool playedByAI)
    {
        BraveExplorersAction bea = new BraveExplorersAction(playedByAI);
        bea.targets = SelectRandomHextile(playedByAI);
        bea.actionName = "Brave Explorers";
        bea.actionType = 2;
        bea.representedCard = this;
        myAction = bea;

        base.PlayCard(playedByAI);
        DeckHandUI dhUI = DeckHandUI.instance;
        dhUI.EnableHandUI();

        ResolutionPhase rP = ResolutionPhase.instance;
        rP.reconActions.Add(bea);
    }
}

