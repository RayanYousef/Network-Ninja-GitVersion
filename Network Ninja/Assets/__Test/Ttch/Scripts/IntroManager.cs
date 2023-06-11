using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{

    public Image[] images;
    public TextMeshProUGUI[] text;

    public int index = 0;
    void Start()
    {
        images[index].gameObject.SetActive(true);
        text[index].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        PlayIntroWithNoFade();
    }

    void PlayIntroWithNoFade()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            index++;


            images[index].gameObject.SetActive(true);
            text[index].gameObject.SetActive(true);

            images[index - 1].gameObject.SetActive(false);
            text[index - 1].gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);


            if (index == images.Length)
            {
                MoveToTutorialScene();
            }
        }
    }
    void PlayIntro()
    {
        index++;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (images[index].color.a == 0)
            {
                FadeInImageAndText(images[index], text[index], 0.5f);
                FadeOutImageAndText(images[index - 1], text[index - 1], 0.5f);
            }
            index++;
        }
        if (index == images.Length)
        {
            MoveToTutorialScene();
        }

    }
    void MoveToTutorialScene()
    {
        //SceneManager.LoadScene("TutorialScene");
    }

    public IEnumerator FadeOutImageAndText(Image imageToFade, TextMeshProUGUI textToFade, float time)
    {
        float elapsedTime = 0f;
        Color imgColor = imageToFade.color;
        Color textColor = textToFade.color;

        while (elapsedTime < time)
        {
            textColor.a = Mathf.Lerp(1, 0, (elapsedTime / time));
            textToFade.color = textColor;

            imgColor.a = Mathf.Lerp(1, 0, (elapsedTime / time));
            imageToFade.color = imgColor;
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        //textToFade.gameObject.SetActive(false);
        imageToFade.gameObject.SetActive(false);
    }

    public IEnumerator FadeInImageAndText(Image imageToFade, TextMeshProUGUI textToFade, float time)
    {
        float elapsedTime = 0f;

        Color imgColor = imageToFade.color;
        Color textColor = textToFade.color;

        while (elapsedTime < time)
        {
            textColor.a = Mathf.Lerp(0, 1, (elapsedTime / time));
            textToFade.color = textColor;

            imgColor.a = Mathf.Lerp(0, 1, (elapsedTime / time));
            imageToFade.color = imgColor;


            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
