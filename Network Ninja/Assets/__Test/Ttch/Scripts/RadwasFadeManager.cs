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

    private void Start()
    {
        img.sprite = sprites[0];
        txt.text = msgs[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            i++;
            if (i < msgs.Length)
            {
                StartCoroutine(FadeOutInImgTxt(img, 1f));
            }
        }
    }

    public IEnumerator FadeOutInImgTxt(Image panel1, float time)
    {
        panel1.CrossFadeAlpha(0f, time, true);
        txt.CrossFadeAlpha(0f, time, true);
        yield return new WaitForSecondsRealtime(time);
        txt.text = msgs[i];
        panel1.sprite = sprites[i];
        yield return new WaitForSecondsRealtime(0.1f);
        panel1.CrossFadeAlpha(1f, time, true);
        txt.CrossFadeAlpha(1f, time, true);

    }
}
