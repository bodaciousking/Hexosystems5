using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateSelection : MonoBehaviour
{
    public int size7, size4, size3;
    public static TemplateSelection instance;
    public GameObject button7, button4, button3, choiceWndow;
    CityPlacement targettingScript;
    GameStartPhases gSP;
    MsgDisplay msgD;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("too many template selection scripts!");
            return;
        }
        instance = this;
    }

    public void SetCities(int s7, int s4, int s3)
    {
        size7 = s7;
        size4 = s4;
        size3 = s3;
        choiceWndow.SetActive(false);

        if (size7 > 0)
        {
            button7.SetActive(true);
            targettingScript.EnableCityPlacementPrefab(0);
        }
        if (size4 > 0)
        {
            targettingScript.EnableCityPlacementPrefab(1);
            button4.SetActive(true);
        }
        if (size3 > 0)
        {
            targettingScript.EnableCityPlacementPrefab(2);
            button3.SetActive(true);
        }

        msgD.DisplayMessage("Place Your Cities", 0.5f);
    }

    private void Update()
    {
    }

    public void DecrementRemainingCities(int citySize)
    {
        switch (citySize)
        {
            case 7:
                size7--;
                break;
            case 4:
                size4--;
                break;
            case 3:
                size3--;
                break;
            default:
                Debug.Log("invalid city size recieved! Size recieved: " + citySize);
                break;
        }

        if (size7 == 0 && size4 == 0 && size3 == 0)
        {
            targettingScript.DisableCityPlacementPrefab();
            button7.SetActive(false);
            button4.SetActive(false);
            button3.SetActive(false);
            gSP.PreparePhase();
            return;
        }

        if (size7 == 0)
        {
            button7.SetActive(false);
            if(targettingScript.intendedSize == 7)
            {
                if (size4 > 0)
                    targettingScript.EnableCityPlacementPrefab(1);
                else if (size3 > 0)
                    targettingScript.EnableCityPlacementPrefab(2);
            }
        }
        if (size4 == 0)
        {
            button4.SetActive(false);
            if (targettingScript.intendedSize == 4)
            {
                if(size3 > 0)
                    targettingScript.EnableCityPlacementPrefab(2);
                else if(size7 > 0)
                    targettingScript.EnableCityPlacementPrefab(0);
            }
        }

        if (size3 == 0)
        {
            button3.SetActive(false);
            if (targettingScript.intendedSize == 3)
            {
                if (size7 > 0)
                    targettingScript.EnableCityPlacementPrefab(0);
                else if (size4 > 0)
                    targettingScript.EnableCityPlacementPrefab(1);
            }
        }
    }

    public void FadeInImage()
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float duration = 1.4f;
        choiceWndow.SetActive(true);

        for (float t = 0.01f; t < duration; t += Time.deltaTime)
        {
            choiceWndow.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, Mathf.Min(1, t / duration));
            yield return null;
        }
    }

    private void Start()
    {
        targettingScript = CityPlacement.instance;
        gSP = GameStartPhases.instance;
        msgD = MsgDisplay.instance;
    }
}
