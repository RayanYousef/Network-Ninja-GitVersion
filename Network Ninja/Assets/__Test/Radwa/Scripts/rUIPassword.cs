using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class rUIPassword : MonoBehaviour
{
    [Header("Login Panel")]
    [SerializeField] private GameObject loginPanel;

    [Header("Password Panel")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordIF;

    string username = "Daiavoloz";
    string birthDate = "21102000";

    public UnityEvent<string> OnTakePassword;

    void Start()
    {
        //Time.timeScale = 0f;
        //loginPanel.SetActive(true);
        passwordPanel.SetActive(false);
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void LoginBtnClicked()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("birthDate", birthDate);
        loginPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowPasswordPanel()
    {
        Time.timeScale = 0f;
        passwordPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
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

        // raise event for check strength to get called
        OnTakePassword?.Invoke(passwordIF.text);

        passwordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}

