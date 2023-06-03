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
    [SerializeField] bool isAnyInteractivePanelActive;

    [SerializeField] List<GameObject> independantUIElements;


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
