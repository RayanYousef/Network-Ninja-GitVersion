using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeManager : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TMP_Text txt;
    [SerializeField] string[] msgs = { "123",
                                       "456",
                                       "789" };
    int i = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(i < msgs.Length)
                StartCoroutine(FadeOutPanelFadeInAnotherPanel(img, 1f));
        }
    }

    public IEnumerator FadeOutPanelFadeInAnotherPanel(Image panel1, float time)
    {
        panel1.CrossFadeAlpha(0f, time, true);
        yield return new WaitForSecondsRealtime(time);
        txt.text = msgs[i++];
        yield return new WaitForSecondsRealtime(0.1f);
        panel1.CrossFadeAlpha(1f, time, true);
    }
}
