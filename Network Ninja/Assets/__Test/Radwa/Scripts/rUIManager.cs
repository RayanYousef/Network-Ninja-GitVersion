using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rUIManager : MonoBehaviour
{
    public static rUIManager instance;

    [SerializeField] private rUIPassword uiPassword;

    [SerializeField] private m_InGameUI inGameUI;

    [SerializeField] Image bg;

    [Header("Panels Lists")]
    [SerializeField] List<GameObject> interactivePanels;
    [SerializeField] bool isAnyInteractivePanelActive;

    [SerializeField] List<GameObject> independantUIElements;

    public enum FacialExp { Idle, Angry, Serious, Afraid };
    [Header("Character Facial Expressions")]
    [SerializeField] Image characterFace;
    [SerializeField] float facialExpFadeDuration = 1;
    Color nonTransparentColor = new Color(0, 0f, 0f, 1f);
    Color transparentColor = new Color(0f, 0f, 0f, 0f);

    [SerializeField] Sprite idle;
    [SerializeField] Sprite angry;
    [SerializeField] Sprite serious;
    [SerializeField] Sprite afraid;
    List<Sprite> facialExpImgs;

    public static rUIManager Instance { get => instance; }
    public rUIPassword UiPassword { get => uiPassword; set => uiPassword = value; }
    public m_InGameUI InGameUI { get => inGameUI; set => inGameUI = value; }

    public List<GameObject> InteractivePanels { get => interactivePanels; set => interactivePanels = value; }
    public bool IsAnyInteractivePanelEnabled
    {
        get => isAnyInteractivePanelActive;
        set
        {
            isAnyInteractivePanelActive = value;
            switch(isAnyInteractivePanelActive)
            {
                case true:
                    if (Time.timeScale != 0)
                    {
                        bg.gameObject.SetActive(true);
                        GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(false);
                        Cursor.lockState = CursorLockMode.Confined;
                        Time.timeScale = 0;
                        return;
                    }
                    break;
                case false:
                    if(Time.timeScale != 1)
                    {
                        bg.gameObject.SetActive(false);
                        GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(true);
                        Cursor.lockState = CursorLockMode.Locked;
                        Time.timeScale = 1;
                    }
                    break;
            }
        }
    }

    public List<GameObject> IndependantUIElements { get => independantUIElements; set => independantUIElements = value; }

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

    private void Start()
    {
        facialExpImgs = new List<Sprite> { idle, angry, serious, afraid };
    }

    #region Fade In/Out Panel
    public IEnumerator FadeOutPanel(CanvasGroup panelToFade, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            panelToFade.alpha = Mathf.Lerp(1, 0, (elapsedTime / time));
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        panelToFade.gameObject.SetActive(false);
    }

    public IEnumerator FadeInPanel(CanvasGroup panelToFade, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            panelToFade.alpha = Mathf.Lerp(0, 1, (elapsedTime / time));
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    } 
    #endregion

    #region Fade Out/In Sprite
    //public IEnumerator FadeOutImg(Image imgToFade, float time)
    //{
    //    float elapsedTime = 0f;

    //    while (elapsedTime < time)
    //    {
    //        imgToFade.color = Color.Lerp(nonTransparentColor, transparentColor, time);
    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }
    //}

    //public IEnumerator FadeInImg(Image imgToFade, float time)
    //{
    //    float elapsedTime = 0f;

    //    while (elapsedTime < time)
    //    {
    //        imgToFade.color = Color.Lerp(transparentColor, nonTransparentColor, time);
    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }
    //}

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

    #region Character Facial Expressions Functions
    public void ChangeFacialExp(FacialExp exp)
    {
        StartCoroutine(FadeOutInImg(facialExpImgs[(int)exp], facialExpFadeDuration));
    }
    #endregion

    public void HideAllIndependantUIElementsExceptLast(GameObject lastElementToAppear)
    {
        foreach(GameObject e in independantUIElements)
        {
            if (e == lastElementToAppear)
                continue;

            e.SetActive(false);
        }
    }

    
}
