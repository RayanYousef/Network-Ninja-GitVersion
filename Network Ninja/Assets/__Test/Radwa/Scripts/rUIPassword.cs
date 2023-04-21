//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;
//using TMPro;

//public class rUIPassword : MonoBehaviour
//{
//    [Header("Login Panel")]
//    [SerializeField] private GameObject loginPanel;

//    [Header("Password Panel")]
//    [SerializeField] private GameObject createPasswordPanel;
//    [SerializeField] private TMP_InputField newPasswordIF;
//    [SerializeField] private GameObject checkPasswordPanel;
//    [SerializeField] private TMP_InputField preSetPasswordIF;


//    string username = "Daiavoloz";
//    string birthDate = "21102000";

//    public UnityEvent<string> OnTakeNewPassword;
//    public UnityEvent<string> OnTakePreSetPassword;

//    void Start()
//    {
//        //Time.timeScale = 0f;
//        //loginPanel.SetActive(true);
//        createPasswordPanel.SetActive(false);
//        checkPasswordPanel.SetActive(false);
//        //Cursor.lockState = CursorLockMode.Confined;
//    }

//    public void LoginBtnClicked()
//    {
//        Time.timeScale = 1f;
//        PlayerPrefs.SetString("username", username);
//        PlayerPrefs.SetString("birthDate", birthDate);
//        loginPanel.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    public void ShowCreatePasswordPanel()
//    {
//        Time.timeScale = 0f;
//        createPasswordPanel.SetActive(true);
//        Cursor.lockState = CursorLockMode.Confined;
//    }

//    public void ShowCheckPasswordPanel()
//    {
//        Time.timeScale = 0f;
//        checkPasswordPanel.SetActive(true);
//        Cursor.lockState = CursorLockMode.Confined;
//    }

//    /**
//     * TakePassword() is called when Form Army button is clicked 
//     */
//    public void TakeNewPassword()
//    {
//        if (string.IsNullOrEmpty(newPasswordIF.text))
//        {
//            return;
//        }

//        // raise event for check strength to get called
//        OnTakeNewPassword?.Invoke(newPasswordIF.text);

//        createPasswordPanel.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;
//        Time.timeScale = 1f;
//    }

//    public void TakePreSetPassword()
//    {
//        if (string.IsNullOrEmpty(newPasswordIF.text))
//        {
//            return;
//        }

//        // raise event for check strength to get called
//        OnTakePreSetPassword?.Invoke(preSetPasswordIF.text);

//        createPasswordPanel.SetActive(false);
//        Cursor.lockState = CursorLockMode.Locked;
//        Time.timeScale = 1f;
//    }
//}

