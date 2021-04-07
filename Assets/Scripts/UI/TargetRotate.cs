using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            // this.transform.localRotation = Quaternion.Euler(+10.0f, 0, 0);

            this.transform.localEulerAngles += new Vector3(0, 60, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            this.transform.localEulerAngles -= new Vector3(0, 60, 0);
        }
    }
}
