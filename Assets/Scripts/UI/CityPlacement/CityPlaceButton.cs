using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlaceButton : MonoBehaviour
{
    public int citySize;
    public int actualSize;
    CityPlacement targettingScript;

    private void Start()
    {
        targettingScript = CityPlacement.instance;
    }
    public void SelectSize()
    {
        targettingScript.ClearCity();
        targettingScript.EnableCityPlacementPrefab(citySize);
        targettingScript.intendedSize = actualSize;
    }
}
