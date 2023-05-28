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
    [SerializeField] GameObject winningPanel;
    [SerializeField] GameObject menuPanel;

    [Header("Options")]
    [SerializeField] Slider volumeSlider;
    public string MainMenu;

    [Header("Transition")]
    public TextMeshProUGUI prompt;


    #region Winning Panel
    public void WinningUI()
    {
        rAreasManager.Instance.CurrentArea.MeshColourChanger.ChangeToColour(rAreasManager.Instance.MaxHealth);
        winningPanel.SetActive(true);
        //Time.timeScale = 0;
    }
    #endregion

    private void Start()
    {
        if(winningPanel != null)
        winningPanel.SetActive(false);
        if(menuPanel!= null)
        menuPanel.SetActive(false);
        volumeSlider.value = 0.5f;


        #region Volume Slider
        volumeSlider.value = 1;

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
            Load();
        }
        else
        {
            Load();
        } 
        #endregion
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
                Time.timeScale = 1;
                break;
            case false:
                menuPanel.SetActive(true);
                Time.timeScale = 0;
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

}
