using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CityInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject Hex; 


    public int CityHp = 0;
    public bool CityShield = false;
    public bool CityDestroyed = false;

    TextMeshProUGUI[] CityHpText;
   


    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("CityInfoPanel");
        CityHpText = infoPanel.GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
       
        

    }

    private void OnMouseOver()
    {

        CityHp = Hex.GetComponent<Hextile>().health;

        CityHpText[0].text = "City Health: " + CityHp;

        CityShield = Hex.GetComponent<Hextile>().shielded;

        if (CityShield == false)
        {
            CityHpText[1].text = "City Shield: Inactive";
            CityHpText[3].text = "Shield HP: 0"; 
        }
        else
        {
            CityHpText[1].text = "City Shield: Active";
            CityHpText[3].text = "Shield HP: " + Hex.GetComponent<Hextile>().permanentShields;
        }

        if (CityDestroyed == false)
        {
            CityHpText[2].text = "City Status: Alive";
        }
        else
        {
            CityHpText[2].text = "City Status: Destroyed";
        }

        Vector3 mousePos = Input.mousePosition;
        infoPanel.transform.position = mousePos;

        if(mousePos.y >= 800.0f)
        {
            infoPanel.transform.position += new Vector3(150, -80, 0);

        }
        else if(mousePos.y <= 100.0f) 
        {
          
            infoPanel.transform.position += new Vector3(150, 100, 0);
        }

        
       
    }

    private void OnMouseExit()
    {
        infoPanel.transform.position = new Vector3(-500, -500, -500);
    }


}