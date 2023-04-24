using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };
public enum PasswordStrength { Weak, Moderate, Strong };

public class rPasswordManager : MonoBehaviour
{
    private static rPasswordManager instance;
    //

    private rArea currentArea;

    private PasswordStrength strength;
    private Soldiers soldiersType;

    [Header("Password Lists")]
    [SerializeField] private Dictionary<int, string> areasPassword = new Dictionary<int, string>();
    private string[] playerPersonalData;
    private string[] easyToGuessPasswords = {"pAssword", "passw0rd", "123456789",
                                             "abcdefghi", "qwerty", "NetworkNinja"};

    //
    public static rPasswordManager Instance { get => instance; }
    public rArea CurrentArea { get { return currentArea; } set => currentArea = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    /*
     *** Future compelet implementation for ManagePassword ***
     * ManagePassword will be called when creating the new area password and will call:
     *  - CheckPassword to check the password against the previously set passwords
     *      - if the password was used before, split the army or give any visual feedback
     *      - if the password is new, call:
     *  - CheckStrength to check the new password strength ans instantiate a friendly army
     */
    //public void ManagePassword(string password)
    //{
    //    //if (currentArea.Password != null)
    //    //{
    //    //    CheckPassword(password);
    //    //    return;
    //    //}
    //    //areasPassword.Add(currentArea.areaID, currentArea.Password);
    //    currentArea.Password = password;
    //    CheckStrength(currentArea.Password);
    //    currentArea.GetComponent<Collider>().isTrigger = true;
    //}

    public void CheckCurrentAreaPasswordStrength()
    {
        /// load user personal data to check the password against them
        loadUserPrivateData();

        /// to calculate the strength of the password, the following will be checked
        /// 1. the length
        /// 2. the complexity
        /// 3. personal data (username, birth date, etc..)
        /// 4. comparing to previous passwords and common used passwords (12345678, qwerty, etc...)

        /// 1. Check length
        int length = currentArea.Password.Length;
        int complexity = 0;
        soldiersType = Soldiers.Melee;

        if (length == 0)
        {
            //Debug.Log($"Length = {length} in Area: {name}");
            return;
        }

        PlayerPrefs.SetString(name, currentArea.Password);
        //Debug.Log("Correct call");

        if (length < 8)
        {
            strength = PasswordStrength.Weak;
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

        /// 2. Check complexity
        if (System.Text.RegularExpressions.Regex.IsMatch(currentArea.Password, @"[A-Z]"))
        {
            complexity++;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(currentArea.Password, @"[a-z]"))
        {
            complexity++;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(currentArea.Password, @"[0-9]"))
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
        if (System.Text.RegularExpressions.Regex.IsMatch(currentArea.Password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
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

        /// 3. Check personal data
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

    public void FormArmy()
    {

        foreach (string weakPassword in easyToGuessPasswords)
        {
            if (currentArea.Password.ToLower().Contains(weakPassword))
            {
                strength = PasswordStrength.Weak;

            }
        }

        int solidersNumbers = 15;
        int rings = 2;
        switch (strength)
        {
            case PasswordStrength.Weak:
                break;

            case PasswordStrength.Moderate:
                solidersNumbers = 45; // 4*5
                rings = 3;
                break;

            case PasswordStrength.Strong:
                solidersNumbers = 80; // 6*5
                rings = 4;
                break;

            default:
                break;
        }

        /// later, it'd be better to send to the friendly soliders AI script both
        /// the password strength and complexity and the switch case is done there
        /// that way the functionality is separated and the password script knows nothing about the soliders

        ///also we can instantiate the army using StartCoroutine to instantiate one by one
        RadialFormation rf = currentArea.GetComponentInChildren<RadialFormation>();
        rf.Amount = solidersNumbers;
        rf.Rings = rings;
        ExampleArmy ea = currentArea.GetComponentInChildren<ExampleArmy>();
        ea.SetPrefabsTypes(soldiersType);
        ea.enabled = true;

        //currentArea.GetComponent<FriendSpawner>().SpawnFriends(solidersNumbers, soldiersType);
        //friendSpawner.SpawnFriends(solidersNumbers, soldiersType);

        // spawnFtiends(solidersNumbers, soldiersType, instantiatePos);
        /// area
        /// ID
        /// password
        /// 
        /// password manager holds list of passwords
        //areaCollider.isTrigger = true;
    }

    private void CheckPassword(string password)
    {
        int length = password.Length;

        if (length == 0)
        {
            //Debug.Log($"Length = {length} in Area: {name}");
            return;
        }

        //Debug.Log("Correct call");
        
        if (password == areasPassword[currentArea.areaID])
        {
            Debug.Log("Correct Password");
            currentArea.GetComponent<Collider>().isTrigger = true;
            return;
        }
        Debug.Log("Wrong Password");
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
}
