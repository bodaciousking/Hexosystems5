using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

    public class Cards : MonoBehaviour
    {
    public static Cards instance;
    public List<Sprite> cardImages = new List<Sprite>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Too many Cards scripts!");
            return;
        }
        instance = this;
    }
}

    public class Card
{
    public int imageReference;
    public int cardID;
    public string cardName;
    public string cardDescr;
    public string energyCostText;
    public int cardType;
    public int energyCost;
    public CardAction myAction;
    public bool aIPlay;

    public TargetType targetType; 
    public int targetSize;
    public int numTargets;

    public virtual void PlayCard(bool playedByAI)
    {
        aIPlay = playedByAI;
        DeckHandUI dHUI = DeckHandUI.instance;
        CityHandler cH = CityHandler.instance;
        ResolutionUI rUI = ResolutionUI.instance;
        if (!playedByAI)
        {
            if (cH.generatedEnergy < energyCost)
            {
                Debug.Log("Not enough Energy.");
                return;
            }
            else if (cH.generatedEnergy >= energyCost)
            {
                Debug.Log("Playing " + cardName + "...");
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

    public List<Hextile> SelectMultiHextile(bool playedByAI)
    {
        GameObject clientMaster = GameObject.Find("ClientMaster");
        List<Hextile> tar = clientMaster.GetComponent<Targetting>().targets;
       
       
        return tar;
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
        imageReference = 0;
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

    public class LaserStrike : AttackCard
{
    public LaserStrike()
    {
        cardID = 003;
        cardType = 0;
        imageReference = 1;
        cardName = "Laser Strike";
        cardDescr = "Target 3 tiles in a line and apply 1 damage to each one of them. Shield Piercing(Keyword: The attack ignores shields)";
        energyCostText = "4";
        energyCost = 4;
        targetType = TargetType.selectTarget;
        numTargets = 3;
        damageDealt = 1;

    }

    public override void PlayCard(bool playedByAI)
    {
        LaserStrikeAction lsa = new LaserStrikeAction();
        lsa.actionName = "Laser Strike";
        lsa.targets = null;
        lsa.damage = damageDealt;
        lsa.actionType = 0;
        lsa.representedCard = this;
        myAction = lsa;

        ResolutionPhase rP = ResolutionPhase.instance;
        base.PlayCard(playedByAI);
        Targetting targetting = Targetting.instance;

        if (!playedByAI)
        {
            targetting.SelectObjectAoE(1);
            targetting.intendedNumTargets = 3;
            targetting.currentCondition = Targetting.TargetCondition.isEnemyTile;

            rP.storedAttackAction = lsa;
        }
        else
        {
            rP.AIStoredAction = lsa;
            if (AIInfo.instance.enemyTiles.Count > 0)
            {
                targetting.StartFindLine(AIInfo.instance.enemyTiles[0], 0); 
            }
            else
            { 
                targetting.StartFindLine(SelectRandomHextile(true)[0], 0); 
            }


            rP.attackActions.Add(lsa);
        }


    }
} 

    public class GaussCannon : AttackCard
    {
        public GaussCannon()
        {
            cardID = 002;
        imageReference = 2;
        cardType = 0;
            cardName = "Gauss Cannon";
            cardDescr = "Target 1 tile and apply 1 damage.";
            energyCostText = "3";
            energyCost = 3;
            targetType = TargetType.selectTarget;
            numTargets = 1;
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
            Targetting targetting = Targetting.instance;
            targetting.SelectObjectAoE(0);
            targetting.currentCondition = Targetting.TargetCondition.isEnemyTile;

            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedAttackAction = gca;
        } 
        else
        {
            Debug.Log("Ai playing Gauss Cannon."); 
            if (AIInfo.instance.enemyTiles.Count > 0)
                gca.target = AIInfo.instance.enemyTiles[0];
            else
                gca.target = SelectRandomHextile(true)[0];
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
        imageReference = 3;
        cardName = "Emergency Shield";
        cardDescr = "Apply 1 Shield point to a target hex. Shield lasts for one turn or until destroyed.";
        energyCost = 1;
        energyCostText = "1";
        targetType = TargetType.selectTarget;
        numTargets = 1;
        shieldType = 0;
        shieldsRestored = 2;
    }
    public override void PlayCard(bool playedByAI)
    {
        base.PlayCard(playedByAI);

        EmergencyShieldAction esa = new EmergencyShieldAction();
        esa.actionName = "Emergency Shield";
        esa.actionType = 1;
        esa.effectedCity = null;
        esa.shieldStrength = shieldsRestored;
        esa.representedCard = this;
        myAction = esa;

        if (!playedByAI)
        {
            Targetting targetting = Targetting.instance;
            targetting.SelectObjectAoE(0);
            targetting.currentCondition = Targetting.TargetCondition.isFriendlyCity;


            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedDefenceAction = esa;
        }

        else
        {
            Debug.Log("Ai playing Defensive Shield.");
            if (AIInfo.instance.endangeredTiles.Count > 0)
                esa.target = AIInfo.instance.endangeredTiles[0];
            else
                esa.target = AICities.instance.aiCities[0].cityTiles[0];

            ResolutionPhase rP = ResolutionPhase.instance;
            rP.defenceActions.Add(esa);
        }
    } 
}

    public class InstalledShield : DefenceCard
    {
        public InstalledShield()
        {
            cardID = 101;
            cardType = 1;
        imageReference = 4;
        cardName = "Installed Shield";
            cardDescr = "Apply 1 Shield point to a target hex.";
            energyCost = 3;
            energyCostText = "3";
            targetType = TargetType.selectTarget;
            numTargets = 1;
            shieldType = 1;
            shieldsRestored = 2;
        }
    public override void PlayCard(bool playedByAI)
    {
        InstalledShieldAction installedShieldAction = new InstalledShieldAction();
        installedShieldAction.actionName = "Installed Shield";
        installedShieldAction.actionType = 1;
        installedShieldAction.effectedCity = null;
        installedShieldAction.shieldStrength = shieldsRestored;
        installedShieldAction.representedCard = this;
        myAction = installedShieldAction;


        if (!playedByAI)
        {
            Targetting targetting = Targetting.instance;
            targetting.SelectObjectAoE(0);
            targetting.currentCondition = Targetting.TargetCondition.isFriendlyCity;

            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedDefenceAction = installedShieldAction;
        }

        base.PlayCard(playedByAI);
    
    }
}

    public class MetropolitanDefenseSystem : DefenceCard
    {
        public MetropolitanDefenseSystem()
        {
            cardID = 101;
            cardType = 1;
        imageReference = 5;
        cardName = "Metropolitan Defense System";
            cardDescr = "Target a city and apply 1 shield to each of its urban tiles.";
            energyCost = 5;
            energyCostText = "5";
            targetType = TargetType.selectTarget;
            numTargets = 1;
            shieldType = 1;
            shieldsRestored = 2;
        }
        public override void PlayCard(bool playedByAI)
    {
        MetropolitanDefenseSystemAction metropolitanDefenseSystemAction = new MetropolitanDefenseSystemAction();
        metropolitanDefenseSystemAction.actionName = "Metropolitan Defense System";
        metropolitanDefenseSystemAction.actionType = 1;
        metropolitanDefenseSystemAction.effectedCity = null;
        metropolitanDefenseSystemAction.shieldStrength = shieldsRestored;
        metropolitanDefenseSystemAction.representedCard = this;
        myAction = metropolitanDefenseSystemAction;

        base.PlayCard(playedByAI);

        if (!playedByAI)
        {
            Targetting targetting = Targetting.instance;
            targetting.SelectObjectAoE(0);
            targetting.currentCondition = Targetting.TargetCondition.isFriendlyCity;



            ResolutionPhase rP = ResolutionPhase.instance;
            rP.storedDefenceAction = metropolitanDefenseSystemAction;
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
        imageReference = 6;
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

        Debug.Log("A");

            ResolutionPhase rP = ResolutionPhase.instance;
            rP.reconActions.Add(bea);
        }

    }

public class ScoutingDrone : ReconCard
{
    public ScoutingDrone()
    {
        cardID = 201;
        cardType = 2;
        imageReference = 7;
        cardName = "Scouting Drone";
        cardDescr = "Recon one tile and 2 additional tiles adjacent to it.";
        energyCost = 3;
        energyCostText = "3";
        targetType = TargetType.selectTarget;
        numTargets = 1;
        visionDuration = 1;
    }
    public override void PlayCard(bool playedByAI)
    {
        ScoutingDroneAction sda = new ScoutingDroneAction(playedByAI);
        sda.actionName = "Scouting Drone";
        sda.target = null;
        sda.actionType = 2;
        sda.representedCard = this;
        myAction = sda;

        base.PlayCard(playedByAI);


        for (int x = 0; x <= 4; x++)
        {
            if (!playedByAI)
            {
                Targetting targetting = Targetting.instance;
                targetting.SelectObjectAoE(0);
                targetting.recon = true;
                targetting.currentCondition = Targetting.TargetCondition.isEnemyTile;

                ResolutionPhase rP = ResolutionPhase.instance;
                rP.storedReconAction = sda;

            }
            else
            {
                Debug.Log("Ai playing Scouting Drone.");
                sda.target = SelectTarget();
                ResolutionPhase rP = ResolutionPhase.instance;
                rP.reconActions.Add(sda);
            }
        }


    } 

}

    public class ShapeshifterInfiltrator : ReconCard
    {
    public ShapeshifterInfiltrator()
    {
        cardID = 201;
        cardType = 2;
        imageReference = 8;
        cardName = "Shapeshifter Infiltrator";
        cardDescr = "Get vision of a random enemy city tile that has not been reconned yet";
        energyCost = 3;
        energyCostText = "3";
        targetType = TargetType.random;
        numTargets = 1;
        visionDuration = 1;
    }

    public override void PlayCard(bool playedByAI)
    {

        ShapeshifterInfiltratorAction sia = new ShapeshifterInfiltratorAction(playedByAI);
        sia.targets = SelectRandomHextile(playedByAI);
        sia.actionName = "Shapeshifter Infiltrator";
        sia.actionType = 2;
        sia.representedCard = this;
        myAction = sia;

        base.PlayCard(playedByAI);
        DeckHandUI dhUI = DeckHandUI.instance;
        dhUI.EnableHandUI();


        ResolutionPhase rP = ResolutionPhase.instance;
        rP.reconActions.Add(sia);
    }


}