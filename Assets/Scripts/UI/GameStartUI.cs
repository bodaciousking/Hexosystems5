using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    public List<GameObject> citySelectOptions = new List<GameObject>();

    public void SelectObject(int a)
    {
        citySelectOptions[a].SetActive(true);
    }
}
