using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



[AddComponentMenu("")]
public class GameNetworkManager : NetworkManager
{

    public Transform firstPlayerSpawn;
    public Transform secondPlayerSpawn;
    public GameObject cam;

    public override void OnServerAddPlayer(NetworkConnection conn)//overrides networkmanager to add more players to the server 
    {

        Transform start = numPlayers == 0 ? firstPlayerSpawn : secondPlayerSpawn;//check number of players 
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);//creates players and sets position 
        NetworkServer.AddPlayerForConnection(conn, player);//adds players to the server. all objs need to be added to the server 

        Debug.Log("Player: " + numPlayers + " Postion " + start.name);
        Debug.Log("Player: " + numPlayers + " has joined the server!");
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //NetworkServer.Destroy(obj);
        //add code to end game and destroy objs 

        base.OnServerDisconnect(conn);
    }


}


