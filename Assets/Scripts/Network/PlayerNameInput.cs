using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private InputField nameInputField = null;//TMP_InputField
    [SerializeField] private Button continueButton = null; 


    public static string DisplayName { get; private set; }  // lets you get the name but not change it from another file
    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUpInputField();  // just call function on start 
    
    
    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {

            return;
        }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        // continueButton.interactable = !string.IsNullOrEmpty(name);//Can Press button when the name is vaild 
        continueButton.interactable = nameInputField.text.Length >= 4;
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

}
