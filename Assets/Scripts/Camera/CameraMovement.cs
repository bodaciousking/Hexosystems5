using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [HideInInspector]
    public Transform pos0, pos1, posN;
    bool moving;

    private void Start()
    {
        pos0 = GameObject.Find("CamPos0").transform;
        pos1 = GameObject.Find("CamPos1").transform;
        posN = GameObject.Find("CamPosN").transform;
        SwitchPos(pos0);
    }

    public void SwapPos()
    {
        if (!moving)
        {
            if (Camera.main.transform.position == pos0.position)
            {
                SwitchPos(pos1);
            }
            else
                SwitchPos(pos0);
        }
    }
    public void SwitchPos(Transform designation)
    {
        if (!moving)
        {
            StopCoroutine(MoveCam(designation));
            StartCoroutine(MoveCam(designation));
        }
    }

    IEnumerator MoveCam(Transform designation)
    {
        moving = true;
        int frameCount = 100;
        Debug.Log("");
        Vector3 currentPos;
        currentPos = Camera.main.transform.position;
        if (designation.position != currentPos)
        {
            if (currentPos != posN.position)
            {
                for (int i = 0; i < frameCount; i++)
                {
                    transform.position = Vector3.Lerp(transform.position, posN.position, (float)i / (float)frameCount);
                    transform.rotation = Quaternion.Lerp(transform.rotation, posN.rotation, (float)i / (float)frameCount);
                    yield return null;
                }
            }
            for (int i = 0; i < frameCount; i++)
            {
                transform.position = Vector3.Lerp(transform.position, designation.position, (float)i / (float)frameCount);
                transform.rotation = Quaternion.Lerp(transform.rotation, designation.rotation, (float)i / (float)frameCount);
                yield return null;
            }
        }
        moving = false;
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
