using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public List<Hextile> hextileList;
    public int owningPlayerID;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0.1f, 0));
    }
}
