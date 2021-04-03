using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardZoom : MonoBehaviour
{
    DeckHandUI dhUI;
    private bool isHeld = false;
    public Vector3 currentPos;
    public int offset = 175;

    void Start()
    {
        currentPos = gameObject.transform.position;
    }
    public void onPointerEnterFunction()
    {
        gameObject.transform.localScale = new Vector3(3, 3, 3);
        gameObject.transform.localPosition += new Vector3(0, offset, 0);
        gameObject.layer = 8;
    }
    public void onPointerExitFunction()
    {
        gameObject.layer = 5;
        gameObject.transform.localScale = new Vector3(0.5876192f, 0.5164623f, 0.5751259f);
        gameObject.transform.localPosition -= new Vector3(0, offset, 0);
    } 
}
