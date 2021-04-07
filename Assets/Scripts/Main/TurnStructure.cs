using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnStructure : MonoBehaviour
{
    public static TurnStructure instance;
    MsgDisplay msgD;
    CityHandler cH;
    Decks decks;
    ResolutionPhase rP;
    DeckHandUI deckHandUI;
    AIInfo aiI;
    AICities aiC;
    ResolutionUI rUI;
    CameraMovement cM;
    public GameObject energyUI, nextPhaseButton;
    public turnPhase currentPhase = turnPhase.Standby;
    public int numTurns = 0;

    public bool testing;


    public float standbyPhaseTime = 2f;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many turn structure scripts!");
            return;
        }
        instance = this;
    } //Singleton loop

    private void Start()
    {
        msgD = MsgDisplay.instance;
        cH = GetComponent<CityHandler>();
        decks = GetComponent<Decks>();
        deckHandUI = GameObject.Find("UIScripts").GetComponent<DeckHandUI>();
        rP = GetComponent<ResolutionPhase>();
        aiI = GetComponent<AIInfo>();
        aiC = GetComponent<AICities>();
        cM = CameraMovement.instance;
        rUI = GetComponent<ResolutionUI>();
    }

    public enum turnPhase
    {
        Standby,
        TurnTitle,
        EnergyGen,
        DrawPhase,
        Strategy,
        Recalibrate,
        Resolution,
        Discard
    }

    public void NextPhase()
    {
        switch (currentPhase)
        {
            case turnPhase.Standby:
                currentPhase = turnPhase.TurnTitle;
                BeginPhaseTitle();
                break;
            case turnPhase.TurnTitle:
                currentPhase = turnPhase.EnergyGen;
                BeginPhaseEnergy();
                break;
            case turnPhase.EnergyGen:
                currentPhase = turnPhase.DrawPhase;
                BeginPhaseDraw();
                break;
            case turnPhase.Recalibrate:
                currentPhase = turnPhase.DrawPhase;
                BeginPhaseDraw();
                break;
            case turnPhase.DrawPhase:
                currentPhase = turnPhase.Strategy;
                BeginPhaseStrategy();
                break;
            case turnPhase.Strategy:
                currentPhase = turnPhase.Resolution;
                BeginPhaseResolution();
                break;
            case turnPhase.Resolution:
                currentPhase = turnPhase.Discard;
                BeginPhaseDiscard();
                break;
            case turnPhase.Discard:
                currentPhase = turnPhase.TurnTitle;
                BeginPhaseTitle();
                break;
        }
        //Debug.Log(currentPhase);
    }
    public void BeginPhaseStandby()
    {
        SetNextTurnTimer(standbyPhaseTime);
    }
    public void BeginPhaseTitle()
    {
        numTurns++;
        msgD.DisplayMessage("Eon " + numTurns, 1f);
        nextPhaseButton.SetActive(true);
    }
    public void BeginPhaseEnergy()
    {
        cH.generatedEnergy = cH.DetermineEnergyGeneratedByCities() + 4 /* 4 = baseEnergy */;
        msgD.DisplayMessage("Energy Generated: " + cH.generatedEnergy, 1f);
        aiI.aiEnergy = aiC.DetermineEnergyGeneratedByCities();
        energyUI.SetActive(true);
        aiI.ResetPriorities();
        aiI.DeterminePriorities();
    }
    public void BeginPhaseRecalibrate()
    {
        msgD.DisplayMessage("Recalibrate Phase", 1f);
        //Code for regenerating/decaying shields, etc. goes here.
    }
    public void BeginPhaseDraw()
    {
        msgD.DisplayMessage("Draw Phase", 1f);

        decks.PrepareDecks();
        deckHandUI.EnableDeckUI();
        deckHandUI.EnableHandUI();
        deckHandUI.EnableAIHandUI();

        aiI.AIDrawCard();
        aiI.AIDrawCard();
        aiI.AIDrawCard();
        aiI.AIDrawCard();
        aiI.AIDrawCard();
    }
    public void BeginPhaseStrategy()
    {
        deckHandUI.DrawRevealedHandUI();
        msgD.DisplayMessage("Strategy Phase", 1f);
        deckHandUI.DisableDeckUI();
        StartCoroutine(aiI.AiPlayCards());
    }
    public void BeginPhaseResolution()
    {
        deckHandUI.DisableHandUI();
        nextPhaseButton.SetActive(false);
        energyUI.SetActive(false);
        rUI.playerActions.Clear();
        rUI.DisplayActionButtons();
        msgD.DisplayMessage("Resolution Phase", 1f);
        rP.BeginPlayActions();
    }
    public void BeginPhaseDiscard()
    {
        msgD.DisplayMessage("Discard Phase", 1f);
        //Code for discarding cards goes here.
    }

    public void SetNextTurnTimer(float time)
    {
        StopCoroutine(TimeNextTurn(time));
        StartCoroutine(TimeNextTurn(time));
    }

    IEnumerator TimeNextTurn(float time)
    {
        yield return new WaitForSeconds(time);
        NextPhase();
    }

    public void ToggleTest()
    {
        testing = !testing;
    }
}