using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadwasFadeManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] string[] msgs;
    [SerializeField] Image img;
    [SerializeField] TMP_Text txt;
    [SerializeField] int i = 0;
    [SerializeField] bool fading;

    private void Start()
    {
        img.sprite = sprites[0];
        txt.text = msgs[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (i < msgs.Length - 1 && fading == false)
            {
                StartCoroutine(FadeOutInImgTxt(img, 1f));
            }
            else
               if (i == msgs.Length - 1 && fading == false) CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.TutorialScene);
        }
    }

    public IEnumerator FadeOutInImgTxt(Image panel1, float time)
    {
        i++;
        fading = true;
        panel1.CrossFadeAlpha(0f, time, true);
        txt.CrossFadeAlpha(0f, time, true);
        yield return new WaitForSecondsRealtime(time);
        txt.text = msgs[i];
        panel1.sprite = sprites[i];
        yield return new WaitForSecondsRealtime(0.1f);
        panel1.CrossFadeAlpha(1f, time, true);
        txt.CrossFadeAlpha(1f, time, true);
        yield return new WaitForSecondsRealtime(1f);
        fading = false;

    }
}
