using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [HideInInspector]
    public Transform pos0, pos1, posN;

    private void Start()
    {
        pos0 = GameObject.Find("CamPos0").transform;
        pos1 = GameObject.Find("CamPos1").transform;
        posN = GameObject.Find("CamPosN").transform;
    }
    public void SwitchPos(Transform designation)
    {
        StopCoroutine(MoveCam(designation));
        StartCoroutine(MoveCam(designation));
    }

    IEnumerator MoveCam(Transform designation)
    {
        int frameCount = 20;

        for (int i = 0; i < frameCount; i++)
        {
            transform.position = Vector3.Lerp(transform.position, designation.position, (float)i / (float)frameCount);
            transform.rotation = Quaternion.Lerp(transform.rotation, designation.rotation, (float)i / (float)frameCount);
            yield return null;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many turn structure scripts!");
            return;
        }
        instance = this;
    } //Singleton loop
}
