using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorialMenu;
    public GameObject[] slides;
    public GameObject button;
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
        
        if (index < 3)
        {
            slides[index].SetActive(false);
            index = index + 1;
            slides[index].SetActive(true);       
        }
        if(index == 3)
        {
            button.SetActive(false);
        }

    }
    public void BackSlide()
    {
        button.SetActive(true);
        if (index == 0)
        {
            mainMenu.SetActive(true);
            tutorialMenu.SetActive(false);
        }
        if (index > 0)
        {
            slides[index].SetActive(false);
            index = index - 1;
            slides[index].SetActive(true);
        }


    }

}