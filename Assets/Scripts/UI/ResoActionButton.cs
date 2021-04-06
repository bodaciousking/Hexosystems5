using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResoActionButton : MonoBehaviour
{
    public CardAction myAction;
    ResolutionPhase rP;
    ResolutionUI rUI;
    Hands hands;
    DeckHandUI dHUI;

    public void CancelAction()
    {
        Debug.Log("Cancelling");
        rP = ResolutionPhase.instance;
        rUI = ResolutionUI.instance;
        hands = Hands.instance;
        dHUI = DeckHandUI.instance;
        rUI.playerActions.Remove(myAction);
        if (rP.attackActions.Contains(myAction))
        {
            rP.attackActions.Remove(myAction);
            Debug.Log("asd");
        }
        else if (rP.defenceActions.Contains(myAction))
        {
            rP.defenceActions.Remove(myAction);
        }
        else if (rP.reconActions.Contains(myAction))
        {
            rP.reconActions.Remove(myAction);
        }

        hands.hand.Add(myAction.representedCard);
        dHUI.DrawRevealedHandUI();
        rUI.DisplayActionButtons();
    }
}
