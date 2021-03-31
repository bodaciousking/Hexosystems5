using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Targetting : MonoBehaviour
{
    public GameObject selectedTargettingObject;
    public List<GameObject> targettingObjects = new List<GameObject>();
    public List<Hextile> targets = new List<Hextile>();
    public Hextile singleTarget;
    public static Targetting instance;

    CityHandler cH;

    public TargetCondition currentCondition;

    public enum TargetCondition
    {
        isFriendlyCity, isEnemyTile, isFogged
    }
    ResolutionPhase rP;
    DeckHandUI dHUI;

    Ray ray;
    RaycastHit hit;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Too many Targetting scripts!");
                return;
        }
        instance = this;
    }

    private void Start()
    {
        rP = ResolutionPhase.instance;
        dHUI = DeckHandUI.instance;
        cH = GetComponent<CityHandler>();
    }
    public void SelectObjectAoE(int sizeIndex)
    {
        if (selectedTargettingObject)
            Destroy(selectedTargettingObject);

        GameObject newObj = Instantiate(targettingObjects[sizeIndex]);
        selectedTargettingObject = newObj;
        selectedTargettingObject.SetActive(true);
    }

    public void DisableObject()
    {
        if (selectedTargettingObject)
            Destroy(selectedTargettingObject);
    }

    public void AddTileToTargets(Hextile tile)
    {
        if (!targets.Contains(tile))
        {
            targets.Add(tile);
        }
    }
    public void ResetColors()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GameObject hextileObject = targets[i].gameObject;
            Transform gfx = hextileObject.transform.Find("Main");
            Renderer hextileRenderer = gfx.GetComponent<Renderer>();
            FloorGfx hextileGfx = gfx.GetComponent<FloorGfx>();
            hextileRenderer.material.color = hextileGfx.myColor;
        }
    }
    public void ClearTargets()
    {
        targets.Clear();
        singleTarget = null;
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, 5000, layerMask))
        {
            if (selectedTargettingObject)
                selectedTargettingObject.SetActive(true);

            if (selectedTargettingObject)
                selectedTargettingObject.transform.position = hit.transform.position;
        }
        else if (selectedTargettingObject)
        {
            selectedTargettingObject.SetActive(false);
            ResetColors();
        }
        if (selectedTargettingObject)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                selectedTargettingObject.transform.Rotate(0, 60, 0);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    ConfirmTarget();
            }
        }
    }

    public void ConfirmTarget()
    {
        bool validTarget = false;
        if(currentCondition == TargetCondition.isFriendlyCity)
        {
            if (singleTarget.isCity)
            {
                validTarget = true;
            }
            if (validTarget)
            {
                selectedTargettingObject.SetActive(false);

                rP.storedDefenceAction.effectedCity = singleTarget.containingCity;
                rP.storedDefenceAction.target = singleTarget;
                rP.CompleteAction();
                rP.BeginPlayActions(); // For testing only. if not testing, comment this line out
                dHUI.EnableHandUI();
                ResetColors();
                DisableObject();
                ClearTargets();
            }
            else
                Debug.Log("Invalid Target");
        }
    }
}
