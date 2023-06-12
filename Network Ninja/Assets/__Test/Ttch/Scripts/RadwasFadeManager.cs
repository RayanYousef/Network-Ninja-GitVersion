using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadwasFadeManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image img;
    [SerializeField] TMP_Text txt;
    [SerializeField] string[] msgs;
    int i = 1;

    private void Start()
    {
        img.sprite = sprites[0];
        txt.text = msgs[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (i < msgs.Length)
            {
                i++;
                StartCoroutine(FadeOutPanelFadeInAnotherPanel(img, 1f));
            }
        }
    }

    public IEnumerator FadeOutPanelFadeInAnotherPanel(Image panel1, float time)
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
