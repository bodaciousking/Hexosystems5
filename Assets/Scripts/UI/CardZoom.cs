using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardZoom : MonoBehaviour
{
    private bool isHeld = false;
    public Vector3 currentPos; 

    // Start is called before the first frame update
    void Start()
    {
        currentPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {



    }

    public void onPointerEnterFunction()
    {

        this.gameObject.transform.localScale = new Vector3(3, 3, 3);
        this.gameObject.transform.localPosition += new Vector3(0, 150, 0);
        this.gameObject.layer = 8;
    }
        public void onPointerExitFunction()
    {
        this.gameObject.layer = 5;
        this.gameObject.transform.localScale = new Vector3(0.5876192f, 0.5164623f, 0.5751259f);
            this.gameObject.transform.localPosition -= new Vector3(0, 150, 0);
        }
}
