using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardZoom : MonoBehaviour
{
    DeckHandUI dhUI;
    public bool revealed;
    public Vector3 mySize;
    public int offset = 0;

    public void onPointerEnterFunction()
    {
        if (revealed)
        {  
           
            mySize = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(2, 2, 2);
            gameObject.transform.localPosition += new Vector3(0, offset, 0);
            gameObject.transform.SetAsLastSibling();
            GridLayoutGroup gLG = transform.parent.GetComponent<GridLayoutGroup>();
            if (gLG)
                gLG.enabled = false;
        }
    }
    public void onPointerExitFunction()
    {
        if (revealed)
        {
           
            gameObject.transform.localScale = mySize;
            gameObject.transform.localPosition -= new Vector3(0, offset, 0);
        }
    } 
}
