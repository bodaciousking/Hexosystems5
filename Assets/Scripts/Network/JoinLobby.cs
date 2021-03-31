using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class JoinLobby : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private InputField ipAddressInputField = null;//TMP_
    [SerializeField] private Button joinButton = null;


    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;

    }


    public void JoinLobbyScript()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress; 
        networkManager.StartClient();

        joinButton.interactable = false; 

    }


    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);

    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }


   
}
