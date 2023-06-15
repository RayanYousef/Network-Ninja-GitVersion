using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rFaceExpressionManager : MonoBehaviour
{
    static rFaceExpressionManager instance;
    public enum FacialExp { Idle, Angry, Serious, Afraid };
    [Header("Character Facial Expressions")]
    [SerializeField] Image characterFace;
    [SerializeField] float facialExpFadeDuration = 1;

    [SerializeField] Sprite idle;
    [SerializeField] Sprite angry;
    [SerializeField] Sprite serious;
    [SerializeField] Sprite afraid;
    List<Sprite> facialExpImgs;

    public static rFaceExpressionManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        facialExpImgs = new List<Sprite> { idle, angry, serious, afraid };
    }

    #region Fade Out/In Sprite
    public IEnumerator FadeOutInImg(Sprite anotherExp, float time)
    {
        if (anotherExp == null)
        {
            Debug.LogWarning("Image2 is null. Stopping the coroutine.");
            yield break;
        }

        characterFace.CrossFadeAlpha(0f, time, true);
        yield return new WaitForSecondsRealtime(time);
        characterFace.sprite = anotherExp;
        characterFace.CrossFadeAlpha(1f, time, true);
    }
    #endregion

    public void ChangeFacialExp(FacialExp exp)
    {
        StartCoroutine(FadeOutInImg(facialExpImgs[(int)exp], facialExpFadeDuration));
    }

}
