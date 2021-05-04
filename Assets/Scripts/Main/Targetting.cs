using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Targetting : MonoBehaviour
{
    public GameObject selectedTargettingObject;
    public List<GameObject> targettingObjects = new List<GameObject>();
    public List<GameObject> AItargettingObjects = new List<GameObject>();
    public List<Hextile> targets = new List<Hextile>();
    public List<Hextile> AItargets = new List<Hextile>();
    public Hextile singleTarget;
    public static Targetting instance;
    public bool recon = false;
    public int intendedNumTargets;

    CameraMovement cM;
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
        cM = CameraMovement.instance;
    }
    public void SelectObjectAoE(int sizeIndex)
    {
        if (selectedTargettingObject)
            Destroy(selectedTargettingObject);

        ResolutionUI.instance.HideActionButtons();

        GameObject newObj = Instantiate(targettingObjects[sizeIndex], transform.position, Quaternion.Euler(-25f, 16f, 2.27f));
        selectedTargettingObject = newObj;
        selectedTargettingObject.SetActive(true);

        if (currentCondition == TargetCondition.isEnemyTile || currentCondition == TargetCondition.isFogged)
            cM.SwitchPos(cM.pos1);
        else if (currentCondition == TargetCondition.isFriendlyCity)
            cM.SwitchPos(cM.pos0);
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
    public void AddTileToAITargets(Hextile tile)
    {
        if (!AItargets.Contains(tile))
        {
            AItargets.Add(tile);
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

            if (selectedTargettingObject)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    selectedTargettingObject.transform.Rotate(0, -60, 0);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                        ConfirmTarget();
                }
            }
        }
        else if (selectedTargettingObject)
        {
            selectedTargettingObject.SetActive(false);
            ResetColors();
        }
    }

    public void ConfirmTarget()
    {
        Debug.Log("confirming target");
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
                dHUI.EnableHandUI();
                ResetColors();
                DisableObject();
                ClearTargets();
            }
            else
                Debug.Log("Invalid Target");
        }
        else if(currentCondition == TargetCondition.isEnemyTile)
        {
            Debug.Log(singleTarget);
            if (singleTarget.owningPlayerID != 0)
            {
                validTarget = true;
            }
            else if (targets.Count == intendedNumTargets)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].owningPlayerID != 1)
                    {
                        validTarget = false;
                        break;
                    }
                    else
                        validTarget = true;
                }
            }

                if (validTarget)
                {
                    selectedTargettingObject.SetActive(false);

                    if (recon)
                    {
                        rP.storedReconAction.target = singleTarget;
                        rP.CompleteAction();
                        dHUI.EnableHandUI();
                        ResetColors();
                        DisableObject();
                        ClearTargets();
                        recon = false;

                    }
                    else
                    {
                        rP.storedAttackAction.target = singleTarget;
                        rP.storedAttackAction.targets = new List<Hextile>(targets);
                        rP.CompleteAction();
                        dHUI.EnableHandUI();
                        ResetColors();
                        DisableObject();
                        ClearTargets();
                    }

                }
            
        }
    }

    public void ConfirmAITargets()
    {
        rP = ResolutionPhase.instance;
        Debug.Log(rP.AIStoredAction);
        List<Hextile> targets = new List<Hextile>(AItargets);
        rP.AIStoredAction.targets = targets;
        rP.CompleteAIAction();
    }
    public void StartFindLine(Hextile start, int objectsListIndex)
    {
        StartCoroutine(FindLine(start, objectsListIndex));
    }
    public IEnumerator FindLine(Hextile start, int objectsListIndex)
    {
        GameObject obj = Instantiate(AItargettingObjects[objectsListIndex]);
        obj.transform.position = start.transform.position;

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 3; i++)
        {
            if (AItargets.Count != 3)
            {
                obj.transform.localEulerAngles += new Vector3(0, 60, 0);
                AItargets.Clear();
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log(AItargets.Count);
        Destroy(obj); 
        ConfirmAITargets();
        AItargets.Clear();
    }
}
