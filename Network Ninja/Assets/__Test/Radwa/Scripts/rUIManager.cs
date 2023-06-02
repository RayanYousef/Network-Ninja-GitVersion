using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rUIManager : MonoBehaviour
{
    public static rUIManager instance;

    [SerializeField] private rUIPassword uiPassword;

    [SerializeField] private m_InGameUI inGameUI;


    [Header("Interactive Panels")]
    [SerializeField] GameObject[] interactivePanels;
    [SerializeField] bool isAnyInteractivePanelEnabled;

    [Header("Fade effect")]
    [SerializeField] private Color transparentColor;
    [SerializeField] private Color color;

    public static rUIManager Instance { get => instance; }
    public rUIPassword UiPassword { get => uiPassword; set => uiPassword = value; }
    public m_InGameUI InGameUI { get => inGameUI; set => inGameUI = value; }

    public GameObject[] InteractivePanels { get => interactivePanels; set => interactivePanels = value; }
    public bool IsAnyInteractivePanelEnabled
    {
        get => isAnyInteractivePanelEnabled;
        set
        {
            isAnyInteractivePanelEnabled = value;
            switch(isAnyInteractivePanelEnabled)
            {
                case true:
                    if (Time.timeScale != 0)
                    {
                        GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(false);
                        Cursor.lockState = CursorLockMode.Confined;
                        Time.timeScale = 0;
                        return;
                    }
                    break;
                case false:
                    if(Time.timeScale != 1)
                    {
                        GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(true);
                        Cursor.lockState = CursorLockMode.Locked;
                        Time.timeScale = 1;
                    }
                    break;
            }
        }
    }

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
        color = new Color(0f, 0f, 0f, 1f);
        transparentColor = new Color(0f, 0f, 0f, 0f);
    }

    public IEnumerator FadePanel(CanvasGroup panelToFade, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            panelToFade.alpha = Mathf.Lerp(1, 0, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

}
