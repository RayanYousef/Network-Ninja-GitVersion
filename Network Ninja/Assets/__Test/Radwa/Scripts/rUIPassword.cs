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
    [Header("Create Password Panel")]
    [SerializeField] private GameObject createPasswordPanel;
    TMP_InputField passwordIF;
    string passwordInput;
    Button passwordBtn;

    [Header("Check Password Panel")]
    [SerializeField] private GameObject checkPasswordPanel;
    Button[] ansBtns;
    string[] answers = new string[3];

    [Header("Menu")]
    [SerializeField] private GameObject resetPasswordPanel;
    Button resetBtn;

    [Header("Message Panel")]
    [SerializeField] private GameObject feedbackPanel;
    private TMP_Text feedbackTxt;
    //private Button OKBtn;

    public string PasswordInput
    {
        get => passwordInput;
        set
        {
            passwordInput = value;

            passwordIF.text = Regex.Replace(passwordIF.text, @"[^a-zA-Z0-9 !@#$%^&*()_+=\[{\]};:<>|./?,-]", "");

            if (createPasswordPanel.activeSelf || checkPasswordPanel.activeSelf || resetPasswordPanel.activeSelf)
            {
                if (Input.anyKeyDown && passwordIF.isFocused && passwordIF.text.Length != 0)
                {
                    /// play clicks sounds only when inputfield is focused,
                    AudioManager.instance.PlayVariedPitcheAudio(AudioManager.instance.ClickClips);

                    // play other sounds for clicking buttons and showing panels.
                }
            }

            /// while the input field length is more than 4
            if (passwordIF.text.Length >= 4)
            {
                ShortResult r = new ShortResult();

                r = rPasswordChecker.CheckPasswordStrengthWithZxccvbn(passwordIF.text);

                switch (r._Strength)
                {
                    case PasswordStrength.Weak:
                        passwordIF.GetComponent<Image>().color = rAreasManager.Instance.LowHealth;
                        break;

                    case PasswordStrength.Moderate:
                        passwordIF.GetComponent<Image>().color = rAreasManager.Instance.HalfHealth;
                        break;

                    case PasswordStrength.Strong:
                        passwordIF.GetComponent<Image>().color = rAreasManager.Instance.MaxHealth;
                        break;
                }
            }
        }
    }

    void Start()
    {
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

        resetPasswordPanel.SetActive(false);
        resetBtn = resetPasswordPanel.GetComponentInChildren<Button>();
        resetBtn.onClick.AddListener(ShowHideSidePanel);
        resetBtn.onClick.AddListener(ShowCreatePasswordPanel);

        feedbackPanel.SetActive(false);
        feedbackTxt = feedbackPanel.GetComponentInChildren<TMP_Text>();
        //OKBtn = feedbackPanel.GetComponentInChildren<Button>();
        //OKBtn.onClick.AddListener(OnClickOKBtn);

        rUIManager.instance.InteractivePanels.Add(createPasswordPanel);
        rUIManager.instance.InteractivePanels.Add(checkPasswordPanel);
        rUIManager.instance.InteractivePanels.Add(resetPasswordPanel);

        rUIManager.instance.IndependantUIElements.Add(createPasswordPanel);
        rUIManager.instance.IndependantUIElements.Add(checkPasswordPanel);
        rUIManager.instance.IndependantUIElements.Add(resetPasswordPanel);
        rUIManager.instance.IndependantUIElements.Add(feedbackPanel);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            ShowHideSidePanel();
        }

        PasswordInput = passwordIF.text;
    }

    #region Reset Password Side Menu Panel
    void ShowHideSidePanel()
    {
        if (createPasswordPanel.activeSelf || checkPasswordPanel.activeSelf)
            return;
        switch (resetPasswordPanel.activeSelf)
        {
            case true:
                resetPasswordPanel.SetActive(false);
                rUIManager.instance.IsAnyInteractivePanelEnabled = false;
                break;

            case false:
                resetPasswordPanel.SetActive(true);
                rUIManager.instance.HideAllIndependantUIElementsExceptLast(resetPasswordPanel) ;
                rUIManager.instance.IsAnyInteractivePanelEnabled = true;
                break;
        }
    }

    public void ResetPasswordButtonInteractbility(bool value)
    {
        resetBtn.interactable = value;
    }

    #endregion

    #region UI Panels
    public void ShowCreatePasswordPanel()
    {
        createPasswordPanel.SetActive(true);
        ResetPasswordIF();
        rUIManager.Instance.HideAllIndependantUIElementsExceptLast(createPasswordPanel);
        rAreasManager.Instance.CurrentArea.IsFlashing = true;
        rAreasManager.Instance.CurrentArea.StartCoroutine(nameof(rAreasManager.Instance.CurrentArea.StartFlashing));
        StartCoroutine(nameof(WaitAndShowPanel));
    }

    IEnumerator WaitAndShowPanel()
    {
        yield return new WaitForSeconds(2f);
        rUIManager.Instance.IsAnyInteractivePanelEnabled = true;
     
        passwordIF.Select();
    }

    public void ShowCheckPasswordPanel()
    {

        string correctAns = GameObjectsManager.Instance.CurrentGate.NextArea.Password;
        //Debug.Log($"Correct Answer is {correctAns}");

        /// generate 2 answers shuffled from the correct answer
        /// and randomly set answers to buttons
        SetAnswersToButtons(correctAns);

        checkPasswordPanel.SetActive(true);
        rUIManager.Instance.HideAllIndependantUIElementsExceptLast(checkPasswordPanel);
        rUIManager.instance.IsAnyInteractivePanelEnabled = true;
    }
    #endregion

    #region On Button Clicked Do Functions
    public void OnClickFormArmyBasedOnPassword()
    {
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            return;
        }

        if (passwordIF.text.Length < 4)
        {
            switch(GameManager.Instance.GameLang)
            {
                case GameLang.English:
                    feedbackTxt.text = "The secrect code can't be less than 4 characters.";
                    break;
                case GameLang.Arabic:
                    feedbackTxt.text = "الكود السري المكون من 3 أحرف ضعيف جدًا ويمكن اختراقه بسهولة.";
                    break;
            }
            feedbackPanel.SetActive(true);
            return;
        }

        if(rAreasManager.Instance.CurrentArea.AreaType == AreaType.Base)
        {
            // if the area is already a base, this means the player is reseting the password
            // so destroy the allies already there
            // to save the hassle of checking how many allies the new password should add

            rAreasManager.Instance.CurrentArea.DestroyAllAllies();
        }

        rAreasManager.Instance.CurrentArea.Password = passwordIF.text;
        rAreasManager.Instance.CheckCurrentAreaPasswordStrength();
        rAreasManager.Instance.CurrentArea.AreaType = AreaType.Base;
        rAreasManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(true);

        rAreasManager.Instance.CurrentArea.IsFlashing = false;

        rAreasManager.Instance.SetAreaHealthBasedOnPassword();
        rAreasManager.Instance.CurrentArea.FormArmyBasedOnAreaHealth();
        rAreasManager.Instance.AreasWithSamePasswordAsCurrent();


        createPasswordPanel.SetActive(false);

        ShowFeedback();

        rUIManager.instance.IsAnyInteractivePanelEnabled = false;
        ResetPasswordIF();
    }

    private void ResetPasswordIF()
    {
        passwordIF.text = null;
        passwordIF.GetComponent<Image>().color = Color.white;
    }

    private void ShowFeedback()
    {
        string tempStr = rAreasManager.Instance.Warnings;
        if (tempStr.Length > 0)
        {
            feedbackTxt.text = tempStr += ".\n";
            tempStr = rAreasManager.Instance.Suggestions;
            if(tempStr.Length > 0)
            {
                feedbackTxt.text += tempStr;
            }

            feedbackPanel.SetActive(true);
            rUIManager.Instance.HideAllIndependantUIElementsExceptLast(feedbackPanel);

            StartCoroutine(rUIManager.instance.FadePanel(feedbackPanel.GetComponent<CanvasGroup>(), 7));
        }
    }

    //public void OnClickOKBtn()
    //{
    //    feedbackPanel.SetActive(false);
    //}

    public void TakeAns(Button selectedBtn)
    {
        if (selectedBtn.GetComponent<rAnswerButton>().IsCorrect)
        {
            //Debug.Log("Correct Password");

            GameObjectsManager.Instance.CurrentGate.NextArea.ShowAlliesBasedOnAreaHealth();
            GameObjectsManager.Instance.CurrentGate.PathController.PlayerEnteredPath(GameObjectsManager.Instance.CurrentGate.State);
        }
        else
        {
            //Debug.Log("Wrong Password");
        }

        checkPasswordPanel.SetActive(false);
        rUIManager.instance.IsAnyInteractivePanelEnabled = false;
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
