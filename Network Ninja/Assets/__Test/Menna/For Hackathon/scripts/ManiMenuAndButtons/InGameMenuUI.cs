using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuUI : MonoBehaviour
{
    [Header("Winning Panel")]
    [SerializeField] GameObject winningPanel;

    [Header("Options")]
    [SerializeField] Slider volumeSlider;
    public string MainMenu;

    #region Winning Panel
    public void WinningUI()
    {
        rPasswordManager.Instance.CurrentArea.MeshColourChanger.ChangeToColour(rPasswordManager.Instance.MaxHealth);
        winningPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        //Time.timeScale = 0;
    }
    #endregion

    private void Start()
    {
        winningPanel.SetActive(false);

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
