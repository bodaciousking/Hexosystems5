using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFront : MonoBehaviour
{

    public Sprite frontAttack;
    public Sprite frontDefence;
    public Sprite frontRecon;

    public GameObject cardType1;

    
    
    int num = 0; 

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(cardType1.GetComponent<Card>().returnType());

        if (num == 0)
        {
            //this.gameObject.GetComponent<Button>().image.overrideSprite = frontAttack;
        }
        if (num == 1)
        {
           // this.gameObject.GetComponent<SpriteRenderer>().sprite = frontAttack;
        }
        if (num == 2)
        {
           // this.gameObject.GetComponent<SpriteRenderer>().sprite = frontAttack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
