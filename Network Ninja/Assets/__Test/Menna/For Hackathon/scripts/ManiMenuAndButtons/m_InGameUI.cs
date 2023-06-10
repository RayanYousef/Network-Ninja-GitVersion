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
    [SerializeField] Button optionsBtn;

    [Header("Main Menu")]
    public string MainMenu;

    [Header("Transition")]
    public TextMeshProUGUI prompt;

    [Header("Options Panel")]
    [SerializeField] GameObject optionsPanel;
    [SerializeField] Slider volumeSlider;

    public GameObject MenuPanel { get => menuPanel; set => menuPanel = value; }

    private void Start()
    {

        if(menuPanel!= null)
            menuPanel.SetActive(false);
        rUIManager.instance.InteractivePanels.Add(menuPanel);
        rUIManager.instance.InteractivePanels.Add(optionsPanel);

        rUIManager.instance.IndependantUIElements.Add(prompt.gameObject);

        optionsBtn.onClick.AddListener(OnClkOptions);

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
            ShowHideMenu();
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

    void ShowHideMenu()
    {
        switch (menuPanel.activeSelf)
        {
            case true:
                menuPanel.SetActive(false);
                rUIManager.Instance.IsSideMenuActive = false;
                //rUIManager.Instance.IsAnyInteractivePanelEnabled = false;
                break;

            case false:
                menuPanel.SetActive(true);
                rUIManager.Instance.IsSideMenuActive = true;
                //rUIManager.Instance.IsAnyInteractivePanelEnabled = true;
                //rUIManager.Instance.HideAllIndependantUIElementsExceptLast(menuPanel);
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
        optionsPanel.SetActive(true);
        rUIManager.Instance.IsOptionsPanelActive = true;
        rUIManager.Instance.IsSideMenuActive = false;
    }

    public void OnCancelClicked()
    {
        optionsPanel.SetActive(false);
        rUIManager.Instance.IsOptionsPanelActive = false;
    }
}
