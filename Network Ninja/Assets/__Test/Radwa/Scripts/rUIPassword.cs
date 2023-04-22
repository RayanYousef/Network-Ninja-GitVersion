using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class rUIPassword : MonoBehaviour
{
    #region Password_UI
    //private static rUIPassword instance;

    private CS_PlayerManager playerManager;
    public UnityEvent OnTakePassword;

    [Header("Create Password Panel")]
    [SerializeField] private GameObject createPasswordPanel;
    private TMP_InputField passwordIF;
    private Button passwordBtn;

    [Header("Check Password Panel")]
    [SerializeField] private GameObject checkPasswordPanel;
     private Button[] ansBtns;
    private string[] answers = new string[3];


    //public static rUIPassword Instance { get => instance; }
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}

    void Start()
    {
        playerManager = GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>();

        passwordIF = createPasswordPanel.GetComponentInChildren<TMP_InputField>();
        passwordBtn = createPasswordPanel.GetComponentInChildren<Button>();
        passwordBtn.onClick.AddListener(TakeNewPassword);
        createPasswordPanel.SetActive(false);
        checkPasswordPanel.SetActive(false);

        ansBtns = checkPasswordPanel.GetComponentsInChildren<Button>();
        ansBtns[0].onClick.AddListener(() => { TakeAns(ansBtns[0]); });
        ansBtns[1].onClick.AddListener(() => { TakeAns(ansBtns[1]); });
        ansBtns[2].onClick.AddListener(() => { TakeAns(ansBtns[2]); });

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TakeAns(Button selectedBtn)
    {
        if(selectedBtn.GetComponent<rAnswerButton>().IsCorrect)
        {
            Debug.Log("Correct Password");
            rPasswordManager.Instance.CurrentArea.GetComponent<Collider>().isTrigger = true;
            rPasswordManager.Instance.CheckStrength(selectedBtn.GetComponentInChildren<TMP_Text>().text);
        }
        else
        {
            Debug.Log("Wrong Password");
        }

        checkPasswordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        playerManager.enabled = true;
    }

    public void ShowCreatePasswordPanel()
    {
        playerManager.enabled = false;
        Time.timeScale = 0f;

        createPasswordPanel.SetActive(true);
        passwordIF.Select();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ShowCheckPasswordPanel()
    {
        playerManager.enabled = false;
        Time.timeScale = 0f;
        string correctAns = rPasswordManager.Instance.CurrentArea.areaPassword;
        Debug.Log($"Correct Answer is {correctAns}");

        /// generate 2 answers shuffled from the correct answer
        /// and randomly set answers to buttons
        SetAnswersToButtons(correctAns);

        checkPasswordPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    /**
     * TakePassword() is called when password button is clicked 
     */
    private void TakeNewPassword()
    {
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            return;
        }

        rPasswordManager.Instance.ManagePassword(passwordIF.text);

        createPasswordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        passwordIF.text = null;
        playerManager.enabled = true;
    }


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
