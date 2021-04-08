using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameHandler : MonoBehaviour
{
    public GameObject vImage, dImage;
    public GameObject rematchButton;


    public void DisplayImage(GameObject image)
    {
        StartCoroutine(FadeInImage(image));
    }

    public IEnumerator FadeInImage(GameObject image)
    {
        image.SetActive(true);
            
        float fadeInTime = 5f;

            for (float t = 0.01f; t < fadeInTime; t += Time.deltaTime)
            {
                image.GetComponent<Image>().material.color = Color.Lerp(Color.clear, Color.white, Mathf.Min(1, t / fadeInTime));
                yield return null;
            }
    }

    public void Rematch()
    {

    }

    private void Start()
    {
    }
}
