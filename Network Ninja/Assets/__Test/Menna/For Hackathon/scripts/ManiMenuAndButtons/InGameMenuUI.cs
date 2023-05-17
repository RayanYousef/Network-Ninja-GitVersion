using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameMenuUI : MonoBehaviour
{
    [Header("Winning Panel")]
    [SerializeField] GameObject winningLosingPanel;
    [SerializeField] GameObject menuPanel;

    [Header("Options")]
    [SerializeField] Slider volumeSlider;
    public string MainMenu;

    #region Winning/Losing Panel
    public void WinningUI()
    {
        rPasswordManager.Instance.CurrentArea.MeshColourChanger.ChangeToColour(rPasswordManager.Instance.MaxHealth);
        winningLosingPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        //Time.timeScale = 0;
    }

    public void LosingUI()
    {
        winningLosingPanel.SetActive(true);
        winningLosingPanel.GetComponentInChildren<TMP_Text>().text = "You Lost!";
        Cursor.lockState = CursorLockMode.Confined;
    }
    #endregion

    private void Start()
    {
        winningLosingPanel.SetActive(false);
        menuPanel.SetActive(false);

        #region Volume Slider
        volumeSlider.value = 1;

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.GetFloat("musicVolume", 0.5f);
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
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case false:
                menuPanel.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
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
