using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class rUIManager : MonoBehaviour
{
    [Header("Login Panel")]
    [SerializeField] private GameObject loginPanel;

    [Header("Password Panel")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordIF;
    [SerializeField] public TMP_Text debugTxt;

    [Header("Events")]
    public rGameEvent OnPasswordEntered;


    [SerializeField] GameObject passwordManager;
    rPassword Password;

    string username = "Daiavoloz";
    string birthDate = "21102000";


    void Start()
    {
        Time.timeScale = 0f;
        loginPanel.SetActive(true);
        passwordPanel.SetActive(false);

        Password = passwordManager.GetComponent<rPassword>();
    }

    public void LoginBtnClicked()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("birthDate", birthDate);

        loginPanel.SetActive(false);
    }

    public void ShowPasswordPanel()
    {
        Time.timeScale = 0f;
        passwordPanel.SetActive(true);
    }

    /**
     * TakePassword() is called when Form Army button is clicked 
     */
    public void TakePassword()
    {
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            return;
        }
        debugTxt.text = passwordIF.text;

        //OnPasswordEntered.Raise(passwordIF.text);


        if (Password != null)
        {
            /* this should be replaced by event
            * clicking Form Army btn will raise an event
            * CheckStrength func. will listen to that event
            * rPassword will listen to this event
            * so the rPassword doesn't know anything about the UI
            * also the UI doesn't know anything about checking the password
            */
            Password.CheckStrength(passwordIF.text);
        }
        passwordPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
