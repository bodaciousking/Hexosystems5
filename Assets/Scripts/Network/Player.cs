using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror.Example.Pong
{
    public class Player : NetworkBehaviour
    {
        public int playerID;

        public int energy = 0;
        public int cardsInHand = 0;
        public int maxHealth = 10; // change this later 
        public int currentHealth = 10;

        public GameObject PlayerHand;


        //other player stats


        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {

                if (Input.anyKey)
                {
                    
                }

                //checks if this player is the owner 
            }

        }
    }

}
