using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlaceObject : MonoBehaviour
{
    CityPlacement cityPlacementScript;
    AICities AICityPlacementScript;
    public bool blocker, AICity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Main")
        {
            GameObject hextileObject = other.gameObject.transform.parent.gameObject;

            Hextile tileScript = hextileObject.GetComponent<Hextile>();

            if (!tileScript.isCity && !tileScript.blocked)
            {
                Renderer hextileRenderer = hextileObject.transform.Find("Main").GetComponent<Renderer>();

                if (AICity)
                {
                    if (!blocker)
                    { 
                        AICityPlacementScript.AddTileToCity(tileScript);
                    }
                    else
                        AICityPlacementScript.AddTileToBlockedArea(tileScript);

                }
                else
                {
                    if (!blocker)
                    {
                        hextileRenderer.material.color = Color.blue;
                        cityPlacementScript.AddTileToCity(tileScript);
                    }
                    else
                    {
                        hextileRenderer.material.color = Color.yellow;
                        cityPlacementScript.AddTileToBlockedArea(tileScript);
                    }
                }
            }


        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Main")
        {
            GameObject hextileObject = other.gameObject.transform.parent.gameObject;

            Hextile tileScript = hextileObject.GetComponent<Hextile>();

            if (!tileScript.isCity && !tileScript.blocked)
            {
                Renderer hextileRenderer = hextileObject.transform.Find("Main").GetComponent<Renderer>();

                if (AICity)
                {
                    if (!blocker)
                        AICityPlacementScript.AddTileToCity(tileScript);
                    else
                        AICityPlacementScript.AddTileToBlockedArea(tileScript);

                }
                else
                {
                    if (!blocker)
                    {
                        hextileRenderer.material.color = Color.blue;
                        cityPlacementScript.AddTileToCity(tileScript);
                    }
                    else
                    {
                        hextileRenderer.material.color = Color.yellow;
                        cityPlacementScript.AddTileToBlockedArea(tileScript);
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main")
        {
            GameObject hextileObject = other.gameObject.transform.parent.gameObject;

            Hextile tileScript = hextileObject.GetComponent<Hextile>();

            if (!tileScript.isCity && !tileScript.blocked)
            {
                cityPlacementScript.ResetColors();
                cityPlacementScript.ClearCity();
            }
        }
    }

    private void Update()
    {
    }

    private void Start()
    {
        cityPlacementScript = CityPlacement.instance;
        AICityPlacementScript = AICities.instance; 
    }
}
