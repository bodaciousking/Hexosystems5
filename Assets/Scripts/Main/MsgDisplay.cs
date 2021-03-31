using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MsgDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public Image displayImage;
    public static MsgDisplay instance;
    public Color textColor;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many message display scripts!");
            return;
        }
        instance = this;
    } //singleton
    public void DisplayMessage(string messageText, float duration)
    {
        StopCoroutine(FadeOutRoutine(duration));
        displayText.gameObject.SetActive(true);
        displayText.text = messageText;
        displayText.color = textColor;
        displayText.alpha = 255;
        FadeOut(duration);
    }

    //Fade time in seconds
    float fadeOutTime = 0.8f;
    public void FadeOut(float duration)
    {
        StopCoroutine(FadeOutRoutine(duration));
        StartCoroutine(FadeOutRoutine(duration));
    }
    private IEnumerator FadeOutRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            displayText.color = Color.Lerp(textColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        displayText.gameObject.SetActive(false);

    }
    void Start()
    {
        textColor = displayText.color;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
