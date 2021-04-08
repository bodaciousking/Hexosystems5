using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null; 


    public void HostLobby()
    {
        landingPagePanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    

}
