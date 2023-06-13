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
    public Image currentImg;

    public int index = 0;
    void Start()
    {
        images[index].CrossFadeAlpha(255, 1f, true);
        text[index].CrossFadeAlpha(255, 1f, true);
    }

    // Update is called once per frame
    void Update()
    {
        currentImg = images[index];
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayIntro();
        }
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

        StartCoroutine(CrossFadeImgAndTxt(1f));



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

    public IEnumerator CrossFadeImgAndTxt (float time)
    {


        images[index].CrossFadeAlpha(255, time, true);
        text[index].CrossFadeAlpha(255, time, true);
        yield return new WaitForSecondsRealtime(time);
        images[index - 1].CrossFadeAlpha(0, time, true);
        text[index - 1].CrossFadeAlpha(0, time, true);

        
    }
}
