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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
