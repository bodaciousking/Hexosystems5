using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CityHandler : MonoBehaviour
{
    public int myID;
    public List<City> myCities = new List<City>();
    public int generatedEnergy = 0;
    public int numCities;
    public static CityHandler instance;
    public TextMeshProUGUI energyText;

    public void RevealAll()
    {
        HexoPlanet planet = GameObject.Find("Player 1 Map").GetComponentInChildren<HexoPlanet>();
        for (int i = 0; i < planet.hextileList.Count; i++)
        {
            planet.hextileList[i].visible = true;
        }
    }
    public int DetermineEnergyGeneratedByCities()
    {
        int allCityEnergy = 0;
        for (int i = 0; i < myCities.Count; i++)
        {
            allCityEnergy += myCities[i].cityEnergy;
        }

        return allCityEnergy;
    }
    private void Start()
    {
    }
    private void Update()
    {
        numCities = myCities.Count;
        if (energyText)
            energyText.text = generatedEnergy.ToString();
    }

    public void DestroyCity(City destroyedCity)
    {
        for (int i = 0; i < destroyedCity.cityTiles.Count; i++)
        {
            destroyedCity.cityTiles[i].Explode();
        }
        myCities.Remove(destroyedCity);
        numCities = myCities.Count;
        if (numCities <= 0)
        {
            Debug.Log("Game over! AI Wins");
            EndGameHandler eGH = GetComponent<EndGameHandler>();
            eGH.DisplayImage(eGH.dImage);
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Too many city handlers!");
            return;
        }
        instance = this;
    }
}

public class City
{
    public List<Hextile> cityTiles;
    public int cityEnergy;
    public int owner;
    public int size;
    public bool destroyed;

    public int DetermineCityEnergy()
    {
        int energy = 0;
        size = cityTiles.Count;
        switch (cityTiles.Count)
        {
            case 3:
                energy = 1;
                break;
            case 4:
                energy = 2;
                break;
            case 7:
                energy = 3;
                break;
        }
        return energy;
    }

    public void ShieldCity()
    {

        for(int x = 0; x <= cityTiles.Count-1; x++)
        {
            cityTiles[x].shielded = true;
        }
        
    }


    public void CheckDestroyed()
    {
        AICities aic = AICities.instance;
        CityHandler cH = CityHandler.instance;

        if (!destroyed)
        {
            switch (size)
            {
                case 3:
                    if (cityTiles.Count != 3)
                    {
                        Debug.Log("City Destroyed!");
                        destroyed = true;
                        if (owner == 0)
                        {
                            cH.DestroyCity(this);
                        }
                        else
                            aic.DestroyCity(this);
                    }
                    break;
                case 4:
                    if (cityTiles.Count <= 2)
                    {
                        Debug.Log("City Destroyed!");
                        destroyed = true;
                        if (owner == 0)
                        {
                            cH.DestroyCity(this);
                        }
                        else
                            aic.DestroyCity(this);
                    }
                    break;
                case 7:
                    if (cityTiles.Count <= 3)
                    {
                        Debug.Log(cityTiles.Count);
                        Debug.Log("City Destroyed!");
                        destroyed = true;
                        if (owner == 0)
                        {
                            cH.DestroyCity(this);
                        }
                        else
                            aic.DestroyCity(this);
                    }
                    break;
            }
        }
    }
}