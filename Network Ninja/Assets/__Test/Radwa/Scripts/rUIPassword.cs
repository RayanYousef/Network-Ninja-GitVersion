using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;


public class rUIPassword : MonoBehaviour
{
    [SerializeField] PlayerInput playerInputs;

    [Header("Create Password Panel")]
    [SerializeField] private GameObject createPasswordPanel;
    [SerializeField] TMP_InputField passwordIF;
    [SerializeField] Button passwordBtn;


    [Header("Check Password Panel")]
    [SerializeField] private GameObject checkPasswordPanel;
    [SerializeField] Button[] ansBtns;
    [SerializeField] string[] answers = new string[3];


    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] Button resetBtn;

    [Header("Message Panel")]
    [SerializeField] private GameObject msgPanel;
    [SerializeField] private TMP_Text msgTxt;

    void Start()
    {
        playerInputs = GameObjectsManager.Instance.Player.GetComponent<PlayerInput>();

        createPasswordPanel.SetActive(false);
        passwordIF = createPasswordPanel.GetComponentInChildren<TMP_InputField>();
        passwordIF.characterLimit = 18;
        passwordBtn = createPasswordPanel.GetComponentInChildren<Button>();
        passwordBtn.onClick.AddListener(OnClickFormArmyBasedOnPassword);


        checkPasswordPanel.SetActive(false);
        ansBtns = checkPasswordPanel.GetComponentsInChildren<Button>();
        ansBtns[0].onClick.AddListener(() => { TakeAns(ansBtns[0]); });
        ansBtns[1].onClick.AddListener(() => { TakeAns(ansBtns[1]); });
        ansBtns[2].onClick.AddListener(() => { TakeAns(ansBtns[2]); });

        menuPanel.SetActive(false);
        resetBtn = menuPanel.GetComponentInChildren<Button>();
        resetBtn.onClick.AddListener(ShowCreatePasswordPanel);
        resetBtn.onClick.AddListener(ShowHideMenu);

        if(msgPanel != null)
        {
            msgPanel.SetActive(false);
            msgTxt = msgPanel.GetComponentInChildren<TMP_Text>();
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ShowHideMenu();
        }

        /// prevent input filed from taking arabic text
        passwordIF.text = Regex.Replace(passwordIF.text, @"[^a-zA-Z0-9 !@#$%^&*()_+=\[{\]};:<>|./?,-]", "");

        if (createPasswordPanel.activeSelf || checkPasswordPanel.activeSelf || menuPanel.activeSelf)
        {
            if(Input.anyKeyDown && passwordIF.isFocused && passwordIF.text.Length != 0)
            {
                AudioManager.instance.PlayVariedPitcheAudio(AudioManager.instance.ClickClips);

                /// play clicks sounds only when inputfield is focused,
                /// play other sounds for clicking buttons and showing panels.
            }
        }
    }

    #region Menu Panel
    void ShowHideMenu()
    {

        switch (menuPanel.activeSelf)
        {
            case true:
                menuPanel.SetActive(false);
                if (createPasswordPanel.activeSelf)
                {
                    return;
                }
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case false:
                if (createPasswordPanel.activeSelf || checkPasswordPanel.activeSelf)
                {
                    return;
                }
                menuPanel.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }

    public void ResetPasswordButtonInteractbility(bool value)
    {
        resetBtn.interactable = value;
    }

    #endregion
    #region UI Panels

    public void OnEnteringAreaShowPannels()
    {

        switch (rPasswordManager.Instance.CurrentArea.Password != null)
        {
            case true:
                ShowCheckPasswordPanel();
                break;
            case false:
                ShowCreatePasswordPanel();
                break;
        }
    }
    public void ShowCreatePasswordPanel()
    {
        playerInputs.enabled = false;
        Time.timeScale = 0f;

        createPasswordPanel.SetActive(true);
        //passwordIF.Select();
        Cursor.lockState = CursorLockMode.Confined;
    }

     void ShowCheckPasswordPanel()
    {
        playerInputs.enabled = false;
        Time.timeScale = 0f;
        string correctAns = rPasswordManager.Instance.CurrentArea.Password;
        Debug.Log($"Correct Answer is {correctAns}");

        /// generate 2 answers shuffled from the correct answer
        /// and randomly set answers to buttons
        SetAnswersToButtons(correctAns);

        checkPasswordPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    #endregion

    #region On Button Clicked Do Functions
    public void OnClickFormArmyBasedOnPassword()
    {
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            return;
        }

        rPasswordManager.Instance.CurrentArea.Password = passwordIF.text;
        rPasswordManager.Instance.CheckCurrentAreaPasswordStrength();
        rPasswordManager.Instance.CurrentArea.AreaType = AreaType.Base;
        rPasswordManager.Instance.CurrentArea.GetComponent<Collider>().isTrigger = true;
        rPasswordManager.Instance.SetAreaHealthBasedOnPassword();
        rPasswordManager.Instance.CurrentArea.FormArmyBasedOnAreaHealth();
        rPasswordManager.Instance.AreasWithSamePasswordAsCurrent();

        createPasswordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        passwordIF.text = null;
        playerInputs.enabled = true;
    }

    public void ShowTips()
    {
        string str = "abc";
        if (!string.IsNullOrEmpty(str))
        {
            // lerp visibality
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = msgPanel.GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            msgPanel.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
    }

    public void TakeAns(Button selectedBtn)
    {
        if (selectedBtn.GetComponent<rAnswerButton>().IsCorrect)
        {
            Debug.Log("Correct Password");
            rPasswordManager.Instance.CurrentArea.GetComponent<Collider>().isTrigger = true;
            //rPasswordManager.Instance.CurrentArea.FormArmyBasedOnAreaHealth();

        }
        else
        {
            Debug.Log("Wrong Password");
        }

        checkPasswordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        playerInputs.enabled = true;
    }
    #endregion

    #region On Room Re-Visit Check Area Password through three buttons
    private void SetAnswersToButtons(string correctAns)
    {
        answers[0] = correctAns;
        answers[1] = Shuffle(correctAns);
        answers[2] = Shuffle(correctAns);

        System.Random rand = new System.Random();
        int n = answers.Length;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            string temp = answers[k];
            answers[k] = answers[n];
            answers[n] = temp;
        }


        for (int i = 0; i < ansBtns.Length; i++)
        {
            ansBtns[i].GetComponent<rAnswerButton>().IsCorrect = false;
        }

        for (int i = 0; i < ansBtns.Length; i++)
        {
            ansBtns[i].GetComponentInChildren<TMP_Text>().text = answers[i];
            if (answers[i] == correctAns)
            {
                ansBtns[i].GetComponent<rAnswerButton>().IsCorrect = true;
            }
        }
    }

    private string Shuffle(string str)
    {
        char[] chars = str.ToCharArray();
        System.Random rand = new System.Random();
        for (int i = 0; i < chars.Length - 1; i++)
        {
            int j = rand.Next(i, chars.Length);
            char temp = chars[i];
            chars[i] = chars[j];
            chars[j] = temp;
        }
        return new string(chars);
    }

    #endregion

}
