using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public enum RoomType { Base, Fight };
public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };
public enum PasswordStrength { Weak, Moderate, Strong };

public class rArea : MonoBehaviour
{
    #region Password_Logic

    [SerializeField] RoomType roomType;
    [SerializeField] private bool fightCompleted = false;
    [SerializeField] private bool isPasswordCreated = false;

    //[SerializeField] private bool isInside = false;

    private PasswordStrength strength;
    private Soldiers soldiersType;

    private FriendSpawner friendSpawner;
    private BoxCollider areaCollider;

    //[Header("Area Password Events")]
    //[SerializeField] UnityEvent OnBaseFirstVisitOrFightCompleted;

    [Header("Password Lists")]
    private string[] playerPersonalData;
    private string[] easyToGuessPasswords = {"pAssword", "passw0rd", "123456789",
                                             "abcdefghi", "qwerty", "NetworkNinja"};

    // void Start()
    //{
    //  areaCollider.isTrigger = false;
    //}

    private void Awake()
    {
        //uiPassword = GetComponent<rUIPassword>();
        friendSpawner = GetComponent<FriendSpawner>();
        areaCollider = GetComponent<BoxCollider>();
    }

    //private void Start()
    //{
    //    uiPassword.OnTakeNewPassword.AddListener(CheckStrength);
    //    uiPassword.OnTakePreSetPassword.AddListener(CheckPassword);
    //}

    public void CheckStrength(string password)
    {
        //if(!isInside)
        //{
        //    return;
        //}
        // load user personal data to check the password against them
        loadUserPrivateData();

        // to calculate the strength of the password, the following will be checked
        // 1. the length
        // 2. the complexity
        // 3. personal data (username, birth date, etc..)
        // 4. comparing to previous passwords and common used passwords (12345678, qwerty, etc...)

        // 1. Check length
        int length = password.Length;
        int complexity = 0;
        soldiersType = Soldiers.Melee;

        if (length == 0)
        {
            //Debug.Log($"Length = {length} in Area: {name}");
            return;
        }

        PlayerPrefs.SetString(name, password);
        //Debug.Log("Correct call");

        if (length < 8)
        {
            strength = PasswordStrength.Weak;
            FormArmy();
            return;
        }
        else if (length < 12)
        {
            strength = PasswordStrength.Moderate;
        }
        else
        {
            strength = PasswordStrength.Strong;
        }

        // 2. Check complexity
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
        {
            complexity++;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]"))
        {
            complexity++;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]"))
        {
            if (complexity > 0)
            {
                soldiersType = Soldiers.MeleeRanged;
            }
            else
            {
                soldiersType = Soldiers.Ranged;
            }
            complexity++;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
        {
            soldiersType = Soldiers.MeleeRangedTank;
            complexity++;
        }

        if (complexity <= 2)
        {
            strength = PasswordStrength.Moderate;
        }
        else
        {
            strength = PasswordStrength.Strong;
        }

        //// 3. Check personal data
        //foreach (string weakPassword in playerPersonalData)
        //{
        //    if (password.ToLower().Contains(weakPassword))
        //    {
        //        strength = PasswordStrength.Weak;
        //        FormArmy();
        //        return;
        //    }
        //}

        // 4. Check common used passwords
        foreach (string weakPassword in easyToGuessPasswords)
        {
            if (password.ToLower().Contains(weakPassword))
            {
                strength = PasswordStrength.Weak;
                FormArmy();
                return;
            }
        }

        FormArmy();
    }

    void loadUserPrivateData()
    {
        playerPersonalData = new string[2];
        for (int i = 0; i < playerPersonalData.Length; i++)
        {
            playerPersonalData[i] = null;
        }
        playerPersonalData[0] = PlayerPrefs.GetString("username").ToLower();
        playerPersonalData[1] = PlayerPrefs.GetString("birthDate").ToLower();
    }

    void FormArmy()
    {
        int solidersNumbers = 15;
        switch (strength)
        {
            case PasswordStrength.Weak:
                break;

            case PasswordStrength.Moderate:
                solidersNumbers = 20; // 4*5
                break;

            case PasswordStrength.Strong:
                solidersNumbers = 30; // 6*5
                break;

            default:
                break;
        }

        // later, it'd be better to send to the friendly soliders AI script both
        // the password strength and complexity and the switch case is done there
        // that way the functionality is separated and the password script knows nothing about the soliders

        //also we can instantiate the army using StartCoroutine to instantiate one by one

        friendSpawner.SpawnFriends(solidersNumbers, soldiersType);
        areaCollider.isTrigger = true;

        // spawnFtiends(solidersNumbers, soldiersType, instantiatePos);
        /// area
        /// ID
        /// password
        /// 
        /// password manager holds list of passwords
    }

    public void CheckPassword(string password)
    {
        //if(!isInside)
        //{
        //    return;
        //}

        int length = password.Length;

        if (length == 0)
        {
            //Debug.Log($"Length = {length} in Area: {name}");
            return;
        }

        //Debug.Log("Correct call");
        string currentPassword = PlayerPrefs.GetString(name);
        if (currentPassword == password)
        {
            Debug.Log("Correct Password");
            areaCollider.isTrigger = true;
            return;
        }
        Debug.Log("Wrong Password");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameObjectsManager.Instance.Player)
        {
            //raise event
            // UI will listen to this event

            if (roomType == RoomType.Base)
            {
                if (isPasswordCreated)
                {
                    // prompt the user to check the previously set password
                    ShowPasswordPanel();
                    //uiPassword.ShowCheckPasswordPanel();
                }
                else
                {
                    // invoke the event to prompt the user to create new password
                    //OnBaseFirstVisitOrFightCompleted?.Invoke();
                    //uiPassword.ShowPasswordPanel();
                    ShowPasswordPanel();
                    isPasswordCreated = true;
                }
            }
            else
            {
                if (fightCompleted)
                {
                    // prompt the user to check the previously set password
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //isInside = true;
    //    if (other.gameObject == GameObjectsManager.Instance.Player)
    //    {
    //        //raise event
    //        // UI will listen to this event

    //        if (roomType == RoomType.Base)
    //        {
    //            if (isPasswordCreated)
    //            {
    //                // prompt the user to check the previously set password
    //                ShowPasswordPanel();
    //                //uiPassword.ShowCheckPasswordPanel();
    //            }
    //            else
    //            {
    //                // invoke the event to prompt the user to create new password
    //                //OnBaseFirstVisitOrFightCompleted?.Invoke();
    //                //uiPassword.ShowPasswordPanel();
    //                ShowPasswordPanel();
    //                isPasswordCreated = true;
    //            }
    //        }
    //        else
    //        {
    //            if (fightCompleted)
    //            {
    //                // prompt the user to check the previously set password
    //            }
    //        }
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        areaCollider.isTrigger = false;
        //isInside = false;
        // TO DO
        // Disable the army in the area the player just left
    }

    //public void AutoTest()
    //{
    //    string[] autoTestPasswords = { "pAssw0rd", "12345678", "abcdefghi",
    //                                    "qwerty", "000000000" , "rrrrrrrrr",
    //                                    "Daiavoloz", "21102000", "01120273611",

    //                                    "menn@97", "ttch2007", "Ray1993", "Nadzy_3103",

    //                                    "bestNinja_2000", "MyFavColorGreen@001",
    //                                    "Rayan_93@NetworkNinja"};
    //    foreach (string testPassword in autoTestPasswords)
    //    {
    //        CheckStrength(testPassword);
    //        Debug.Log($"{testPassword } is { strength}");
    //    }
    //} 
    #endregion

    #region Password_UI

    //[Header("Login Panel")]
    //[SerializeField] private GameObject loginPanel;
    private CS_PlayerManager playerManager;

    [Header("Password Panel")]
    [SerializeField] private GameObject passwordPanel;
    private TMP_InputField passwordIF;
    private Button passwordBtn;

    //string username = "Daiavoloz";
    //string birthDate = "21102000";

    //public UnityEvent<string> OnTakeNewPassword;
    //public UnityEvent<string> OnTakePreSetPassword;
    private bool isNew = true;

    void Start()
    {
        playerManager = GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>();

        //uiPassword = GetComponentInChildren<Canvas>();
        passwordIF = passwordPanel.GetComponentInChildren<TMP_InputField>();
        passwordBtn = passwordPanel.GetComponentInChildren<Button>();
        passwordBtn.onClick.AddListener(TakePassword);
        passwordPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    //public void LoginBtnClicked()
    //{
    //    Time.timeScale = 1f;
    //    PlayerPrefs.SetString("username", username);
    //    PlayerPrefs.SetString("birthDate", birthDate);
    //    //loginPanel.SetActive(false);
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    public void ShowPasswordPanel()
    {
        playerManager.enabled = false;
        Time.timeScale = 0f;
        if (isNew)
        {
            passwordBtn.GetComponentInChildren<TMP_Text>().text = "Create Password";
        }
        else
        {
            passwordBtn.GetComponentInChildren<TMP_Text>().text = "Check Password";
        }
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
        if (isNew)
        {
            //OnTakeNewPassword?.Invoke(passwordIF.text);
            CheckStrength(passwordIF.text);
            isNew = false;
        }
        else
        {
            //OnTakePreSetPassword?.Invoke(passwordIF.text);
            CheckPassword(passwordIF.text);
        }
        passwordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        passwordIF.text = null;
        playerManager.enabled = true;
    }
    #endregion
}
