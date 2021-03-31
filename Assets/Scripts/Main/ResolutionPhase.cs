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
            Debug.Log("Finishing an Attack action.");
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
    public List<Hextile> targets = new List<Hextile>();

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
                hextileObject.TakeDamage(2);
            }
        }
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
                    Debug.Log("Added a city tile to AI Enmy tiles.");
                    GameObject cM = GameObject.Find("ClientMaster");
                    AIInfo aiI = cM.GetComponent<AIInfo>();
                    aiI.enemyTiles.Add(item);
                }
            }
        }
    }
}
