using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexoPlanet : MonoBehaviour
{
    public List<Hextile> hextileList = new List<Hextile>();
    public int owningPlayerID;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0.1f, 0));
    }
}
