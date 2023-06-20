using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class m_InGameUI : MonoBehaviour
{
    [Header("Winning Panel")]
    [SerializeField] GameObject menuPanel;

    [Header("Main Menu")]
    public string MainMenu;

    [Header("Transition")]
    public TextMeshProUGUI prompt;

    [Header("Options Panel")]
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject controlsPage1;
    [SerializeField] GameObject controlsPage2;

    [SerializeField] GameObject comboMapPanel;
    [SerializeField] GameObject comboPage1;
    [SerializeField] GameObject comboPage2;

    [SerializeField] Button optionsBtn;
    [SerializeField] Button controlsBtn;
    [SerializeField] Button comboMapBtn;

    int controlsPageNumb = 1;
    int comboPageNumb = 1;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider mouseSensitivitySlider;

    //[SerializeField] Button camFarBtn;
    //[SerializeField] Button camMidBtn;
    //[SerializeField] Button camNearBtn;
    [SerializeField] Button closeBtn;

    public GameObject MenuPanel { get => menuPanel; set => menuPanel = value; }

    private void Start()
    {
        if(menuPanel!= null)
            menuPanel.SetActive(false);
        rUIManager.Instance.InteractivePanels.Add(menuPanel);
        rUIManager.Instance.InteractivePanels.Add(optionsPanel);
        rUIManager.Instance.InteractivePanels.Add(controlsPanel);
        rUIManager.Instance.InteractivePanels.Add(comboMapPanel);

        rUIManager.Instance.IndependantUIElements.Add(prompt.gameObject);

        optionsBtn.onClick.AddListener(OnClkOptions);
        controlsBtn.onClick.AddListener(OnClkControls);
        comboMapBtn.onClick.AddListener(OnClkComboMap);

        //mouseSensitivitySlider.onValueChanged.AddListener(delegate { (); });

        //camFarBtn.onClick.AddListener(() =>
        //{
        //    GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToFar();
        //});

        //camMidBtn.onClick.AddListener(() =>
        //{
        //    GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToMid();
        //});

        //camNearBtn.onClick.AddListener(delegate { GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToClose(); });

        closeBtn.onClick.AddListener(OnCloseClicked);
       
        #region Volume Slider
        //    volumeSlider.value = 1;

        //    if (!PlayerPrefs.HasKey("musicVolume"))
        //    {
        //        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        //        Load();
        //    }
        //    else
        //    {
        //        Load();
        //    } 
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(optionsPanel.activeSelf)
            {
                optionsPanel.SetActive(false);
                rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(optionsPanel.GetComponent<CanvasGroup>(), 0.2f));
            }
            else if (controlsPanel.activeSelf)
            {
                controlsPanel.SetActive(false);
                rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(controlsPanel.GetComponent<CanvasGroup>(), 0.2f));
            }
            else if (comboMapPanel.activeSelf)
            {
                comboMapPanel.SetActive(false);
                rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(comboMapPanel.GetComponent<CanvasGroup>(), 0.2f));
            }
            else
            {
                ShowHideSideMenu();
            }
        }
    }

    #region Volume
    public void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }


    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        save();
    }

    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    #endregion

    void ShowHideSideMenu()
    {
        switch (menuPanel.activeSelf)
        {
            case true:
                rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(menuPanel.GetComponent<CanvasGroup>(), 0.2f));
                break;

            case false:
                rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(menuPanel.GetComponent<CanvasGroup>(), 0.2f));
                break;
        }
    }
    public void BackToMainMenu()
    {
        if (MainMenu != null)
        {
            Time.timeScale = 1;
            CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.MainMenuScene);
        }
    }

    #region Pause
    public void pause()
    {
        Time.timeScale = 0;
    }
    public void unPause()
    {
        Time.timeScale = 1;
    }
    #endregion

    private void OnClkOptions()
    {
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(optionsPanel.GetComponent<CanvasGroup>(), 0.2f));

        rUIManager.Instance.SetInteractivePanelState = true;
    }

    #region Controls Panel
    private void OnClkControls()
    {
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(menuPanel.GetComponent<CanvasGroup>(), 0.2f));
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(controlsPanel.GetComponent<CanvasGroup>(), 0.2f));

        rUIManager.Instance.SetInteractivePanelState = true;
    }     
    private void OnClkComboMap()
    {
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(menuPanel.GetComponent<CanvasGroup>(), 0.2f));
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(comboMapPanel.GetComponent<CanvasGroup>(), 0.2f));

        rUIManager.Instance.SetInteractivePanelState = true;
    } 

    public void OnClkNextControls()
    {
        if (controlsPageNumb == 2)
            return;

        controlsPage1.SetActive(false);
        controlsPage2.SetActive(true);
        controlsPageNumb = 2;
    }
    public void OnClkPreviousControls()
    {
        if (controlsPageNumb == 1)
            return;

        controlsPage1.SetActive(true);
        controlsPage2.SetActive(false);
        controlsPageNumb = 1;
    }
    public void OnClkNextComboMap()
    {
        if (comboPageNumb == 2)
            return;

        comboPage1.SetActive(false);
        comboPage2.SetActive(true);
        comboPageNumb = 2;
    }
    public void OnClkPreviousComboMap()
    {
        if (comboPageNumb == 1)
            return;

        comboPage1.SetActive(true);
        comboPage2.SetActive(false);
        comboPageNumb = 1;
    }

    #endregion

    public void OnCloseClicked()
    {
        optionsPanel.SetActive(false);
        rUIManager.Instance.SetInteractivePanelState = false;
    }
}
