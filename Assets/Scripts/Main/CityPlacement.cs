using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CityPlacement : MonoBehaviour
{
    public GameObject target;
    public GameObject selectedCityObject;
    public List<GameObject> cityPlaceObjects = new List<GameObject>();
    public static CityPlacement instance;
    TemplateSelection tS;
    CityHandler cH;
    public int intendedSize;
    List<Hextile> possibleCity = new List<Hextile>();
    List<Hextile> possibleBlockedArea = new List<Hextile>();

    Ray ray;
    RaycastHit hit;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many targetting scripts!");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        tS = TemplateSelection.instance;
        cH = GetComponent<CityHandler>();

        for (int i = 0; i < cityPlaceObjects.Count; i++)
        {
            GameObject newObject = Instantiate(cityPlaceObjects[i]);
            cityPlaceObjects[i] = newObject;
            if (target)
            {
                cityPlaceObjects[i].transform.position = target.transform.position;
            }
            cityPlaceObjects[i].SetActive(false);
        }
    }
    public void Update()
    {
        if (tS.size3 > 0 || tS.size4 > 0 || tS.size7 > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, 5000, layerMask))
            {
                if (hit.collider.transform.parent.GetComponent<Hextile>().owningPlayerID == 0)
                {
                    if (tS.size3 > 0 || tS.size4 > 0 || tS.size7 > 0)
                        selectedCityObject.SetActive(true);
                    if (selectedCityObject)
                        selectedCityObject.transform.position = hit.transform.position;
                }
            }
            else if (selectedCityObject)
            {
                selectedCityObject.SetActive(false);
                ResetColors();
            }
            if (selectedCityObject)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    selectedCityObject.transform.Rotate(0, 60, 0);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                        CreateCity();
                }
            }
        }
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

    public void CreateCity()
    {
        if (possibleCity.Count < intendedSize)
        {
            Debug.Log("City too small!");
            return;
        }

        City newCity = new City();
        List<Hextile> newCityTiles = new List<Hextile>(possibleCity);
        newCity.cityTiles = newCityTiles;
        newCity.cityEnergy = newCity.DetermineCityEnergy();
        newCity.owner = 0;
        cH.myCities.Add(newCity);

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


        possibleCity.Clear();

        for (int i = 0; i < possibleBlockedArea.Count; i++)
        {
            Hextile tileScript = possibleBlockedArea[i];
            tileScript.blocked = !tileScript.blocked;
            FloorGfx fgfx = tileScript.transform.Find("Main").GetComponent<FloorGfx>();
            tileScript.transform.Find("Main").GetComponent<Renderer>().material.color = fgfx.myColor;
        }
        possibleBlockedArea.Clear();

        tS.DecrementRemainingCities(intendedSize);
    }
    public void ResetColors()
    {
        for (int i = 0; i < possibleCity.Count; i++)
        {
            GameObject hextileObject = possibleCity[i].gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            FloorGfx hextileGfx = gfx.GetComponent<FloorGfx>();
            hextileRenderer.material.color = hextileGfx.myColor;
        }
        for (int i = 0; i < possibleBlockedArea.Count; i++)
        {
            GameObject hextileObject = possibleBlockedArea[i].gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            FloorGfx hextileGfx = gfx.GetComponent<FloorGfx>();
            hextileRenderer.material.color = hextileGfx.myColor;
        }
    }
    public void ClearCity()
    {
        possibleCity.Clear();

        possibleBlockedArea.Clear();
    }
    public void EnableCityPlacementPrefab(int listIndex)
    {
        switch (listIndex)
        {
            case 0:
                intendedSize = 7;
                break;
            case 1:
                intendedSize = 4;
                break;
            case 2:
                intendedSize = 3;
                break;
            default:
                Debug.Log("invalid city size recieved! Size recieved: " + listIndex);
                break;
        }
        if (selectedCityObject)
        {
            selectedCityObject.SetActive(false);
        }
        cityPlaceObjects[listIndex].SetActive(true); 
        selectedCityObject = cityPlaceObjects[listIndex];
    }

    public void DisableCityPlacementPrefab()
    {
        selectedCityObject.SetActive(false);
        ClearCity();
    }
}
