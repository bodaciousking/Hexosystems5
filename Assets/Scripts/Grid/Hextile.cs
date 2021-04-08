using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hextile : MonoBehaviour
{
    public Vector2 tileLocation;
    
    public bool isCity;
    public bool shielded;
    public bool blocked;
    public GameObject myCity;
    public GameObject fogOfWar;
    public GameObject myShield;
    public GameObject myFire;
    public GameObject floor; 
    public City containingCity;
    public int owningPlayerID;
    public int permanentShields;
    public int decayShields;
    public int health;
    public int myRowLength;

    public bool visible = false; 

    private void Start()
    {
        //floor.GetComponent<Renderer>().material.color = Color.gray;

        health = 2; 
        myShield.transform.localScale = new Vector3(myShield.transform.localScale.x, myCity.transform.localScale.y + myCity.transform.localScale.y/8f, myShield.transform.localScale.z);
    }
    private void Update()
    {
        
        if (owningPlayerID == 1)
        {
            
            if (visible == false)
            {
                myCity.SetActive(false);
                fogOfWar.SetActive(true);
                
            }
            else
            {
                fogOfWar.SetActive(false);
                if (isCity)
                {
                    myCity.SetActive(true);
                    if (health <= 0)
                    {
                        myFire.SetActive(true);
                    }
                }

            }

        }
        else if (isCity)
        {
            myCity.SetActive(true);
            if (health <= 0)
            {
                myFire.SetActive(true);
            }
        }
        if (shielded)
        {
            myShield.SetActive(true);
        }
        else
        {
            myShield.SetActive(false);
        }


       
    }

    public void TakeDamage(int dmg)
    {
        if (isCity)
        {
            if(permanentShields > 0)
            {
                permanentShields -= dmg;
            }
            else
                health = health - dmg;

            if (health <= 0) 
            { 
                Explode(); 
            }
        }
        myFire.SetActive(true);
    }

    public void Explode()
    {
        if (containingCity!=null)
        {
            containingCity.cityTiles.Remove(this);
            containingCity.CheckDestroyed();
        }

        GameObject hextileObject = gameObject;
        Transform gfx = hextileObject.transform.Find("Main");
        FloorGfx fgfx = gfx.GetComponent<FloorGfx>();
        fgfx.myColor = Color.red;
        Renderer hextileRenderer = gfx.GetComponent<Renderer>();
        if(visible || owningPlayerID == 0)
        hextileRenderer.material.color = Color.red;
    }

 
   
}

