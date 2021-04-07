using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionPhase : MonoBehaviour
{


    public List<AttackAction> attackActions = new List<AttackAction>();
    public List<DefenceAction> defenceActions = new List<DefenceAction>();
    public List<ReconAction> reconActions = new List<ReconAction>();

    public AttackAction storedAttackAction;
    public DefenceAction storedDefenceAction;
    public ReconAction storedReconAction;

    public static ResolutionPhase instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many ResolutionPhase scripts!");
            return;
        }
        instance = this;
    }

    public void CompleteAction()
    {
        if (storedAttackAction!= null) 
        {
            Debug.Log("Finishing an Attack action: " +storedAttackAction.actionName);
            attackActions.Add(storedAttackAction);
            storedAttackAction = null;
        }
        if (storedDefenceAction!= null) 
        {
            Debug.Log("Finishing a Defence action.");
            defenceActions.Add(storedDefenceAction);
            storedDefenceAction = null;
        }
        if (storedReconAction!= null) 
        {
            Debug.Log("Finishing a Recon action.");
            reconActions.Add(storedReconAction);
            storedReconAction = null;
        }
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
    }

    public IEnumerator ActionLoop()
    {
        for (int i = 0; i < attackActions.Count; i++)
        {
            AttackAction aA = attackActions[i];
            aA.ExecuteAction();
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < defenceActions.Count; i++)
        {
            DefenceAction dA = defenceActions[i];
            dA.ExecuteAction();
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < reconActions.Count; i++)
        {
            ReconAction rA = reconActions[i];
            rA.ExecuteAction();
        }
        ClearActions();
    }
}

public class CardAction
{
    public int actionType; // 0 = Attack, 1 = Defence, 2 = Scouting
    public string actionName;
    public Hextile target;
    public List<Hextile> targets; //= new List<Hextile>();

    public virtual void ExecuteAction()
    {
        Debug.Log("Executing " + actionName + ".");
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
            if (hextileObject.isCity)
            {
                hextileObject.TakeDamage(damage);
            }
        }
    }
}

public class LaserStrikeAction : AttackAction
{
    public override void ExecuteAction()
    {

        base.ExecuteAction();

       
        //Debug.Log(clientMaster.GetComponent<Targetting>().targets);
        //targets[0]

        //for (int i = 0; i < targets.Count; i++)
        //{
        //    Hextile hextileObject = targets[i];

        //    if (hextileObject.isCity)
        //    {
 
        //        Transform gfx = hextileObject.transform.Find("Main");
        //        Renderer hextileRenderer = gfx.GetComponent<Renderer>();
        //        hextileRenderer.material.color = Color.red;

        //        hextileObject.TakeDamage(damage);
        //        hextileObject.visible = true;
        //    }
        //}


       // target.TakeDamage(damage);
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
            GameObject hextileObject = item.gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            hextileRenderer.material.color = Color.blue;

            item.visible = true;
            if (playedByAI)
            {
                if (item.isCity)
                {
                    Debug.Log("Added a city tile to known player tiles.");
                    GameObject cM = GameObject.Find("ClientMaster");
                    AIInfo aiI = cM.GetComponent<AIInfo>();
                    aiI.enemyTiles.Add(item);
                }
            }
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
        Transform gfx = hextileObject.transform.Find("Main");
        Renderer hextileRenderer = gfx.GetComponent<Renderer>();
        hextileRenderer.material.color = Color.blue;

        int mid = 0;
        int top = 0;
        int bottom = 0;
        int left = 0;
        int right = 0; 

        target.visible = true;
        if (!playedByAI)
        {
            List<Hextile> hextileList = map2.transform.GetChild(0).GetComponent<Planet>().hextileList;

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (target.tileLocation == hextileList[x].GetComponent<Hextile>().tileLocation)
                {
                    //mid = x;
                    //top = x - 1;
                    //bottom = x + 1;
                    //left = top / 2;
                    //Debug.Log("mid: " + mid + " Top: " + top + " bottom: " + bottom + " left: " + left);
                    
                    hextileObject = hextileList[x + 1].gameObject;
                    gfx = hextileObject.transform.Find("Main");
                    hextileRenderer = gfx.GetComponent<Renderer>();
                    hextileRenderer.material.color = Color.blue;

                    hextileObject = hextileList[x - 1].gameObject;
                    gfx = hextileObject.transform.Find("Main");
                    hextileRenderer = gfx.GetComponent<Renderer>();
                    hextileRenderer.material.color = Color.blue;


                }
            }
        }
        else
        {
            List<Hextile> hextileList = map1.transform.GetChild(0).GetComponent<Planet>().hextileList;

            for (int x = 0; x < hextileList.Count; x++)
            {
                if (target.tileLocation == hextileList[x].GetComponent<Hextile>().tileLocation)
                {
                    




                    hextileObject = hextileList[x + 1].gameObject;
                    gfx = hextileObject.transform.Find("Main");
                    hextileRenderer = gfx.GetComponent<Renderer>();
                    hextileRenderer.material.color = Color.blue;

                    hextileObject = hextileList[x - 1].gameObject;
                    gfx = hextileObject.transform.Find("Main");
                    hextileRenderer = gfx.GetComponent<Renderer>();
                    hextileRenderer.material.color = Color.blue;

                    hextileList[x + 1].visible = true;
                    hextileList[x - 1].visible = true;

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
            List<Hextile> hextileList = map2.transform.GetChild(0).GetComponent<Planet>().hextileList;
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
            int rand = Random.Range(0, cityList.Count);

            GameObject hextileObject = cityList[rand].gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            hextileRenderer.material.color = Color.blue;

            cityList[rand].visible = true;
            
           
        }
        else
        {
            List<Hextile> hextileList = map1.transform.GetChild(0).GetComponent<Planet>().hextileList;
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
            int rand = Random.Range(0, cityList.Count);

            GameObject hextileObject = cityList[rand].gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            hextileRenderer.material.color = Color.blue;

            cityList[rand].visible = true;
        }

    }
}





