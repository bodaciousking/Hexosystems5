using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICities : MonoBehaviour
{
    public int city3, city4, city7;
    public int intendedSize;
    public int numCities;
    public bool donePlacing;
    GameObject player1Planet;
    Planet planet;
    GameStartPhases gSP;
    TurnStructure tS;
    List<Hextile> possibleCity = new List<Hextile>();
    List<Hextile> possibleBlockedArea = new List<Hextile>();
    public List<GameObject> AICityPlaceObjects = new List<GameObject>();
    public static AICities instance;

    public List<City> aiCities = new List<City>();


    private void Update()
    {
        numCities = aiCities.Count;
    }
    public void DestroyCity(City destroyedCity)
    {
        aiCities.Remove(destroyedCity);
        numCities = aiCities.Count;
        if (numCities <= 0)
        {
            Debug.Log("Game over! Player Wins");
        }
        //else
           // Debug.Log("cities remaining: " + numCities);
    }
    public int DetermineEnergyGeneratedByCities()
    {
        int allCityEnergy = 0;
        for (int i = 0; i < aiCities.Count; i++)
        {
            allCityEnergy += aiCities[i].cityEnergy;
        }

        return allCityEnergy;
    }
    public void SelectRandomTemplate()
    {
        int randomInt = Random.Range(0, 4);
        switch (randomInt)
        {
            case 0:
                SetCities(0, 0, 3);
                break;
            case 1:
                SetCities(1, 1, 2);
                break;
            case 2:
                SetCities(3, 3, 0);
                break;
            case 3:
                SetCities(7, 0, 0);
                break;
            case 4:
                SetCities(2, 2, 1);
                break;
        }
    }

    public void SetCities(int c3, int c4, int c7)
    {
        city3 = c3;
        city4 = c4;
        city7 = c7;

        PlacePrep();
    }

    public void DonePlacingCities()
    {
        donePlacing = true; 
        if(gSP.currentPhase == GameStartPhases.initPhase.Prepare)
        {
            tS.NextPhase();
        }
    }

    public void PlacePrep()
    {
        player1Planet = GameObject.Find("Player 1 Map");
        planet = player1Planet.transform.Find("Planet(Clone)").GetComponent<Planet>();
        StartCoroutine(PlaceCityLoop());
    }


    public IEnumerator PlaceCityLoop()
    {
        while (city3 > 0 || city4 > 0 || city7 > 0)
        {
            possibleBlockedArea.Clear();
            possibleCity.Clear();
            if (city3 > 0)
            {
                intendedSize = 3; 
                CheckCity(0);
                yield return new WaitForSeconds(0.1f);
                if (PlaceCity())
                    city3--;
            }
            else
          if (city4 > 0)
            {
                intendedSize = 4; 
                CheckCity(1);
                yield return new WaitForSeconds(0.1f);
                if (PlaceCity())
                    city4--;
            }
            else
          if (city7 > 0)
            {
                intendedSize = 7;
                CheckCity(2);
                yield return new WaitForSeconds(0.1f);
                if(PlaceCity())
                    city7--;
            
            }
        yield return new WaitForSeconds(0.5f);
        }
        DonePlacingCities();
    } 

    public void CheckCity(int citySize)
    {
        int randomNumber = Random.Range(0, planet.hextileList.Count);
        Hextile randomHex = planet.hextileList[randomNumber];

        GameObject newCPObj = AICityPlaceObjects[citySize];
        newCPObj.SetActive(true);
        newCPObj.transform.position = randomHex.transform.position;

    }

    public bool PlaceCity()
    {
        bool success = false;
        if (possibleCity.Count < intendedSize)
        {
            possibleBlockedArea.Clear();
            possibleCity.Clear();
            return success;
        }
        else
        {
            City newCity = new City();
            List<Hextile> newCityTiles = new List<Hextile>(possibleCity);
            newCity.cityTiles = newCityTiles;
            for (int i = 0; i < possibleCity.Count; i++)
            {
                Hextile tileScript = possibleCity[i];
                tileScript.isCity = !tileScript.isCity;
                tileScript.transform.Find("Main").GetComponent<Renderer>().material.color = Color.gray;
                tileScript.containingCity = newCity;
                tileScript.health = 2;
                FloorGfx fgfx = tileScript.transform.Find("Main").GetComponent<FloorGfx>();
                fgfx.myColor = Color.gray;
            }
            newCity.cityEnergy = newCity.DetermineCityEnergy();
            newCity.owner = 1;
            aiCities.Add(newCity);
            success = true;

            possibleCity.Clear();

            for (int i = 0; i < possibleBlockedArea.Count; i++)
            {
                Hextile tileScript = possibleBlockedArea[i];
                tileScript.blocked = !tileScript.blocked;
                FloorGfx fgfx = tileScript.transform.Find("Main").GetComponent<FloorGfx>();
                tileScript.transform.Find("Main").GetComponent<Renderer>().material.color = fgfx.myColor;
            }
            possibleBlockedArea.Clear();
        }

        return success;
    }
    public void AddTileToCity(Hextile tile)
    {
        if (!possibleCity.Contains(tile))
        {
            possibleCity.Add(tile);
        }
    }
    public void AddTileToBlockedArea(Hextile tile)
    {
        if (!possibleBlockedArea.Contains(tile))
        {
            possibleBlockedArea.Add(tile);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many AICity Placements!");
            return;
        }
        else
            instance = this;
    }

    private void Start()
    {
        gSP = GameStartPhases.instance;
        tS = TurnStructure.instance;
    }
}
