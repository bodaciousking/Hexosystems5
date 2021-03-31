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
    public GameObject myShield;
    public GameObject myFire;
    public City containingCity;
    public int owningPlayerID;
    public int permanentShields;
    public int decayShields;
    public int health;

    public bool visible = false; //Edit by Erik 

    private void Start()
    {
        //tileLocation = gameObject.transform.position;
        myShield.transform.localScale = new Vector3(myShield.transform.localScale.x, myCity.transform.localScale.y + myCity.transform.localScale.y/8f, myShield.transform.localScale.z);
    }
    private void Update()
    {
        if (isCity)
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
        health = health - dmg;
        if(health <= 0) { Explode(); }
    }

    public void Explode()
    {
        containingCity.cityTiles.Remove(this);
        containingCity.CheckDestroyed();

        GameObject hextileObject = gameObject;
        Transform gfx = hextileObject.transform.Find("Main");
        FloorGfx fgfx = gfx.GetComponent<FloorGfx>();
        fgfx.myColor = Color.red;
        Renderer hextileRenderer = gfx.GetComponent<Renderer>();
        hextileRenderer.material.color = Color.red;
    }
}
