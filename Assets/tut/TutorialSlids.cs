using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSlids : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorialMenu;
    public GameObject[] slides;
    public int index = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextSlide()
    {
        if(index < 3)
        {
            slides[index].SetActive(false);
            index = index + 1;
            slides[index].SetActive(true);
        }
 
    }
    public void BackSlide()
    {
        if(index == 0)
        {
            mainMenu.SetActive(true);
            tutorialMenu.SetActive(false);
        }
        if(index > 0)
        {
            slides[index].SetActive(false);
            index = index - 1;
            slides[index].SetActive(true);
        }
       

    }

}
