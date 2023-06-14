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
    [SerializeField] Button optionsBtn;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider mouseSensitivitySlider;

    [SerializeField] Button camFarBtn;
    [SerializeField] Button camMidBtn;
    [SerializeField] Button camNearBtn;
    [SerializeField] Button closeBtn;

    public GameObject MenuPanel { get => menuPanel; set => menuPanel = value; }

    private void Start()
    {
        if(menuPanel!= null)
            menuPanel.SetActive(false);
        rUIManager.Instance.InteractivePanels.Add(menuPanel);
        rUIManager.Instance.InteractivePanels.Add(optionsPanel);

        rUIManager.Instance.IndependantUIElements.Add(prompt.gameObject);

        optionsBtn.onClick.AddListener(OnClkOptions);

        //mouseSensitivitySlider.onValueChanged.AddListener(delegate { (); });

        //camFarBtn.onClick.AddListener(GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToFar);
        //camMidBtn.onClick.AddListener(GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToMid);
        //camNearBtn.onClick.AddListener(GameObjectsManager.Instance.Player.GetComponent<CS_CameraManager>().SetCameraToClose);

        closeBtn.onClick.AddListener(OnCloseClicked);
       
        //    #region Volume Slider
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
        //    #endregion
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
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        if (MainMenu != null)
        {
            SceneManager.LoadScene(MainMenu);
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

    public void OnCloseClicked()
    {
        optionsPanel.SetActive(false);
        rUIManager.Instance.SetInteractivePanelState = false;
    }
}
