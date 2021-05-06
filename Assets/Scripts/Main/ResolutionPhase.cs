using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionPhase : MonoBehaviour
{
    public List<CardAction> attackActions = new List<CardAction>();
    public List<CardAction> defenceActions = new List<CardAction>();
    public List<CardAction> reconActions = new List<CardAction>();

    public AttackAction storedAttackAction;
    public DefenceAction storedDefenceAction;
    public ReconAction storedReconAction;

    public CardAction AIStoredAction;

    public static ResolutionPhase instance;


    ResolutionUI rUI;
    TurnStructure tS;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many ResolutionPhase scripts!");
            return;
        }
        instance = this;
    }
    private void Start()
    {
        rUI = ResolutionUI.instance;
        tS = TurnStructure.instance;
    }

    public void CompleteAIAction()
    {
        CardAction completedAction = new CardAction();
        completedAction = AIStoredAction;
        switch (AIStoredAction.actionType)
        {
            case 0:
                attackActions.Add(completedAction);
                break;
            case 1:
                defenceActions.Add(completedAction);
                break;
            case 2:
                reconActions.Add(completedAction);
                break;
        }
        AIStoredAction = null;
        Debug.Log("Finishing an AI action: " + completedAction.actionName);
    }
    public void CompleteAction()
    {
        CardAction completedAction = new CardAction();
        if (storedAttackAction!= null) 
        {
            completedAction = storedAttackAction;
            Debug.Log("Finishing an Attack action: " + completedAction.actionName);
            attackActions.Add(completedAction);
            storedAttackAction = null;
        }
        if (storedDefenceAction!= null) 
        {
            completedAction = storedDefenceAction;
            Debug.Log("Finishing a Defence action.");
            defenceActions.Add(completedAction);
            storedDefenceAction = null;
        }
        if (storedReconAction!= null)
        {
            completedAction = storedReconAction;
            Debug.Log("Finishing a Recon action.");
            reconActions.Add(completedAction);
            storedReconAction = null;
        }

        CameraMovement cM = CameraMovement.instance;
        //cM.SwitchPos(cM.posN);

        rUI.playerActions.Add(completedAction);
        completedAction = null;
        rUI.DisplayActionButtons();
    }
    public void BeginPlayActions()
    {
        StopCoroutine(ActionLoop());
        StartCoroutine(ActionLoop());
    }

    public void ClearActions()
    {
        attackActions.Clear();
        defenceActions.Clear();
        reconActions.Clear();
        rUI.DisplayActionButtons();
    }

    public IEnumerator ActionLoop()
    {
        Debug.Log(attackActions.Count+ " "+ defenceActions.Count + " " + reconActions.Count);
        for (int i = 0; i < reconActions.Count; i++)
        {
            CardAction rA = reconActions[i];
            rA.ExecuteAction();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < defenceActions.Count; i++)
        {
            CardAction dA = defenceActions[i];
            dA.ExecuteAction();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < attackActions.Count; i++)
        {
            CardAction aA = attackActions[i];
            aA.ExecuteAction();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);

        ClearActions();
        tS.NextPhase();
    }
}

public class CardAction
{
    public int actionType; // 0 = Attack, 1 = Defence, 2 = Scouting
    public string actionName;
    public Card representedCard;
    public Hextile target;
    public List<Hextile> targets; //= new List<Hextile>();

    public virtual void ExecuteAction()
    {
        Debug.Log("Executing " + actionName + ".");
    }

    public void RevealTile(Hextile hex)
    {
        AIInfo aII;
        aII = AIInfo.instance;
        hex.visible = true;
        if (hex.isCity)
        {
            if (hex.owningPlayerID == 0) // Player tile revealed
            {
                if (!aII.enemyTiles.Contains(hex))
                    aII.enemyTiles.Add(hex);
            }
            else if (hex.owningPlayerID == 1) //AI tile revealed
            {
                if (!aII.endangeredTiles.Contains(hex))
                    aII.endangeredTiles.Add(hex);
            }
        }
    }
}

public class AttackAction : CardAction
{
    public int damage;
}

public class DefenceAction : CardAction
{
    public int shieldStrength;
    public City effectedCity;
}

public class ReconAction : CardAction
{
    public GameObject map1;
    public GameObject map2;
}
// ATTACK ACTIONS

public class ScatterShotAction : AttackAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        //Card Resolution

        for (int i = 0; i < targets.Count; i++)
        {
            Hextile hextileObject = targets[i];
            hextileObject.TakeDamage(damage);
        }
    }
}

public class LaserStrikeAction : AttackAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        for (int i = 0; i < targets.Count; i++)
        {
            Hextile hextileObject = targets[i];
            hextileObject.TakeDamage(damage);
            //hextileObject.visible = true;
        }

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(damage);
        }
    }
}//needs work;

public class GuassCannonAction : AttackAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        target.TakeDamage(damage);
    }
}


// DEFENSE ACTIONS
public class EmergencyShieldAction : DefenceAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        target.shielded = true;
        target.decayShields += shieldStrength;
    }
}

public class InstalledShieldAction : DefenceAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        target.shielded = true;
        target.decayShields += shieldStrength;
    }
}

public class MetropolitanDefenseSystemAction : DefenceAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        target.shielded = true;
        target.decayShields += shieldStrength;
        target.containingCity.ShieldCity();
    }
}



//RECON ACTIONS

public class BraveExplorersAction : ReconAction
{
    bool playedByAI;
    public BraveExplorersAction(bool _playedByAI)
    {
        playedByAI = _playedByAI;
    }

    public override void ExecuteAction()
    {
        base.ExecuteAction();

        foreach (Hextile item in targets)
        {

            Hextile hextileObject = item;
            Transform gfx = hextileObject.transform.Find("Main");
            FloorGfx fgfx = gfx.GetComponent<FloorGfx>();
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            if (hextileObject.visible || hextileObject.owningPlayerID == 0)
            {
                fgfx.myColor = Color.blue;
                hextileRenderer.material.color = Color.blue;
            }
               

            RevealTile(item);
        }
    }
}

public class ScoutingDroneAction : ReconAction
{
    bool playedByAI;
    public ScoutingDroneAction(bool _playedByAI)
    {
        playedByAI = _playedByAI;
    }

    public override void ExecuteAction()
    {
        map1 = GameObject.Find("Player 0 Map");
        map2 = GameObject.Find("Player 1 Map");

        base.ExecuteAction();

        GameObject hextileObject = target.gameObject;
        Hextile hex = hextileObject.GetComponent<Hextile>();
        RevealTile(hex);

        
       


        target.visible = true;
        if (!playedByAI)
        {
            List<Hextile> hextileList = map2.transform.GetChild(0).GetComponent<HexoPlanet>().hextileList;

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (target.tileLocation == hextileList[x].GetComponent<Hextile>().tileLocation)
                {
                    hextileObject = hextileList[x + 1].gameObject;
                    hex = hextileObject.GetComponent<Hextile>();
                    RevealTile(hex);

                    hextileObject = hextileList[x - 1].gameObject;
                    hex = hextileObject.GetComponent<Hextile>();
                    hex.visible = true;
                    RevealTile(hex);

                }
            }
        }
        else
        {
    
            List<Hextile> hextileList = map1.transform.GetChild(0).GetComponent<HexoPlanet>().hextileList;

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (target.tileLocation == hextileList[x].GetComponent<Hextile>().tileLocation)
                {
                    RevealTile(hextileList[x + 1]);
                    RevealTile(hextileList[x - 1]); 
                }
            }
        }
        

    }
}

public class ShapeshifterInfiltratorAction : ReconAction
{
    bool playedByAI;
    public ShapeshifterInfiltratorAction(bool _playedByAI)
    {
        playedByAI = _playedByAI;
    }

    public override void ExecuteAction()
    {
        map1 = GameObject.Find("Player 0 Map");
        map2 = GameObject.Find("Player 1 Map");

        base.ExecuteAction();

        if (!playedByAI)
        {
            List<Hextile> hextileList = map2.transform.GetChild(0).GetComponent<HexoPlanet>().hextileList;
            List<Hextile> cityList = new List<Hextile>();

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (hextileList[x].GetComponent<Hextile>().isCity)
                {
                    if(hextileList[x].GetComponent<Hextile>().visible == false)
                    {
                        cityList.Add(hextileList[x]);
                    }
                }
            }
            int rand = Random.Range(0, cityList.Count-1);

            Debug.Log(rand);
            RevealTile(cityList[rand]);

           
        }
        else
        {
            List<Hextile> hextileList = map1.transform.GetChild(0).GetComponent<HexoPlanet>().hextileList;
            List<Hextile> cityList = new List<Hextile>();

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (hextileList[x].GetComponent<Hextile>().isCity)
                {
                    if (hextileList[x].GetComponent<Hextile>().visible == false)
                    {
                        cityList.Add(hextileList[x]);
                    }

                }
            }
            int rand = Random.Range(0, cityList.Count-1);
            Debug.Log(rand);
            RevealTile(cityList[rand]);

            Hextile hextileObject = cityList[rand];
            Transform gfx = hextileObject.transform.Find("Main");
            FloorGfx fgfx = gfx.GetComponent<FloorGfx>();
            fgfx.myColor = Color.blue;
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            if (hextileObject.visible || hextileObject.owningPlayerID == 0)
                hextileRenderer.material.color = Color.blue;
        }

    }
}





