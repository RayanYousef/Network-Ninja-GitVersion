using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CS_AudioLerp : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioMixer mixer;

    [SerializeField] bool lerping, inCombat;
    [SerializeField] float song1, max_1, min_1, song2, min_2, max_2, slow, fast;

    public bool InCombat { get => inCombat; set => inCombat = value; }

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("Song2", min_2);
    }

    private void Update()
    {
        if (InCombat && !lerping)
        {
            float Song1Volume, Song2Volume;
            mixer.GetFloat("Song1", out Song1Volume);
            mixer.GetFloat("Song2", out Song2Volume);
            if (Song1Volume > -1 && Song2Volume < -14)
            {
                StartCoroutine(LerpFunction("Song1", song1, max_1, min_1, slow));
                StartCoroutine(LerpFunction("Song2", song2, min_2, max_2, fast));
            }
            //lerp = false;
        }
        else if (!InCombat && !lerping)
        {
            float Song1Volume, Song2Volume;
            mixer.GetFloat("Song1", out Song1Volume);
            mixer.GetFloat("Song2", out Song2Volume);
            if (Song1Volume < -14 && Song2Volume > 3)
            {
                StartCoroutine(LerpFunction("Song2", song2, max_2, min_2, slow));
                StartCoroutine(LerpFunction("Song1", song1, min_1, max_1, fast));
            }
            //lerp = true;
        }
    }



    IEnumerator LerpFunction(string name, float variable, float startValue, float endValue, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            variable = Mathf.Lerp(startValue, endValue, time / duration);
            mixer.SetFloat(name, variable);
            time += Time.deltaTime;
            lerping = true;
            yield return null;
        }
        variable = endValue;
        lerping = false;

    }

}
