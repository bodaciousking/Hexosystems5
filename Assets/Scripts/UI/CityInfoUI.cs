using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CityInfoUI : MonoBehaviour
{
    public GameObject onScreen; 
    public GameObject infoPanel;
    public GameObject Hex; 


    public int CityHp = 0;
    public bool CityShield = false;
    public bool CityDestroyed = false;

    public bool over = false; 

    TextMeshProUGUI[] CityHpText;



    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("CityInfoPanel");
        onScreen = GameObject.Find("onScreen");
        CityHpText = infoPanel.GetComponentsInChildren<TextMeshProUGUI>();

    }


    // Update is called once per frame
    void Update()
    {
       
        

    }

    private void OnMouseOver()
    {
        
        if (over == false)
        {
            infoPanel.transform.position = onScreen.transform.position;
        }
        over = true;

        CityHp = Hex.GetComponent<Hextile>().health;
        CityHpText[0].text = "City Health: " + CityHp;

        CityShield = Hex.GetComponent<Hextile>().shielded;

        if (CityShield == false)
        {   
            CityHpText[1].text = "Shield HP: 0"; 
        }
        else
        {
            CityHpText[1].text = "Shield HP: " + Hex.GetComponent<Hextile>().permanentShields;
        }

        if (CityDestroyed == false)
        {
            CityHpText[2].text = "City Status: Alive";
        }
        else
        {
            CityHpText[2].text = "City Status: Destroyed";
        }
    }

    private void OnMouseExit()
    {
        over = false;
        infoPanel.transform.position -= new Vector3(400, 0, 0);
        CityHpText[0].text = "City Health: 0";
        CityHpText[1].text = "Shield HP: 0";
    }


}