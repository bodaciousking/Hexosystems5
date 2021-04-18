using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionUI : MonoBehaviour
{
    ResolutionPhase rP;
    public GameObject actionButton;
    public List<Color> colors = new List<Color>();
    public List<CardAction> playerActions = new List<CardAction>();
    public static ResolutionUI instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too man resoUI!");
            return;
        }
        else instance = this;
    }
    private void Start()
    {
        rP = ResolutionPhase.instance;
    }
    public void DisplayActionButtons()
    {
        HideActionButtons();
        string actionButtonHolder = "ActionsList";
        Transform actionsHolder = GameObject.Find(actionButtonHolder).transform;

        for (int i = 0; i < playerActions.Count; i++)
        {
            GameObject newButton = Instantiate(actionButton, actionsHolder);
            newButton.GetComponent<Image>().color = colors[playerActions[i].actionType];
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = playerActions[i].actionName;
            ResoActionButton rAP = newButton.GetComponent<ResoActionButton>();
            rAP.myAction = playerActions[i];
        }
    }
    public void HideActionButtons()
    {
        string actionButtonHolder = "ActionsList";
        Transform actionsHolder = GameObject.Find(actionButtonHolder).transform;

        foreach (Transform item in actionsHolder)
        {
            Destroy(item.gameObject);
        }
    }
}
