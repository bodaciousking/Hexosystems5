using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualResolution : MonoBehaviour
{
    public static VisualResolution instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many VisualResolutions!");
            return;
        }
        instance = this;
    }

}
