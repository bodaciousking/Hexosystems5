using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateButton : MonoBehaviour
{
    public int cities7, cities4, cities3;
    TemplateSelection tS;

    void Start()
    {
        tS = TemplateSelection.instance;
    }

    public void selectMe()
    {
        tS.SetCities(cities7, cities4, cities3);
    }
}
