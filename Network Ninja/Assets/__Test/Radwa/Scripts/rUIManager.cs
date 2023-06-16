using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rUIManager : MonoBehaviour
{
    public static rUIManager instance;

    [SerializeField] private rUIPassword uiPassword;

    [SerializeField] private m_InGameUI inGameUI;

    [Header("Panels Lists")]
    [SerializeField] List<GameObject> interactivePanels;
    [SerializeField] bool setInteractivePanelState;

    [SerializeField] List<GameObject> independantUIElements;

    public static rUIManager Instance { get => instance; }
    public rUIPassword UiPassword { get => uiPassword; set => uiPassword = value; }
    public m_InGameUI InGameUI { get => inGameUI; set => inGameUI = value; }

    public List<GameObject> InteractivePanels { get => interactivePanels; set => interactivePanels = value; }
    public bool SetInteractivePanelState
    {
        get => setInteractivePanelState;
        set
        {
            setInteractivePanelState = value;

            foreach(GameObject p in interactivePanels)
            {
                if(p.activeSelf)
                {
                    GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(false);
                    Cursor.lockState = CursorLockMode.Confined;
                    return;
                }
            }

            GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(true);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
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

        SetInteractivePanelState = false;
    }

    public IEnumerator FadeInPanel(CanvasGroup panelToFade, float time, bool stopTimeScale = true)
    {
        panelToFade.gameObject.SetActive(true);

        SetInteractivePanelState = true;

        float elapsedTime = 0f;

        while (elapsedTime < time + 0.1)
        {
            if (!panelToFade.gameObject.activeSelf)
                yield break;

            panelToFade.alpha = Mathf.Lerp(0, 1, (elapsedTime / time));
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
        panelToFade.alpha = 1;
        if (stopTimeScale)
            Time.timeScale = 0;
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
