using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;


public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };
public enum PasswordStrength { Weak, Moderate, Strong };

public class rPasswordManager : MonoBehaviour
{
    private static rPasswordManager instance;
    [SerializeField] rUIPassword passwordCanvas;

    [Header("Password Manager Components")]
    [SerializeField] rArea[] listOfLevelAreas;
    [SerializeField] int maxSoldiersNumber = 80;
    [SerializeField] Color maxHealth, halfHealth, lowHealth, enemyColor;

    [Header("Current Area Info")]
    [SerializeField] rArea currentArea;
    [SerializeField] PasswordStrength strength;
    [SerializeField] Soldiers soldiersType;

    [Header("Password Lists")]
    private string[] playerPersonalData;
    private string[] easyToGuessPasswords = {"pAssword", "passw0rd", "123456789",
                                             "abcdefghi", "qwerty", "NetworkNinja"};

    //
    public static rPasswordManager Instance { get => instance; }
    public rArea CurrentArea { get { return currentArea; } set => currentArea = value; }

    public int MaxSoldiersNumber { get => maxSoldiersNumber; }
    public Color MaxHealth { get => maxHealth; }
    public Color HalfHealth { get => halfHealth; }
    public Color LowHealth { get => lowHealth; }
    public Color EnemyColor { get => enemyColor;}
    public rUIPassword PasswordCanvas { get => passwordCanvas; set => passwordCanvas = value; }

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

    
    public void AreasWithSamePasswordAsCurrent()
    {

        foreach (rArea area in listOfLevelAreas)
        {
            area.SharingPasswordWarningIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < listOfLevelAreas.Length; i++)
        {
            for (int j = i + 1; j < listOfLevelAreas.Length; j++)
            {
                if (listOfLevelAreas[i].Password!=null && listOfLevelAreas[i].Password == listOfLevelAreas[j].Password)
                {
                    listOfLevelAreas[i].SharingPasswordWarningIcon.gameObject.SetActive(true);
                    listOfLevelAreas[j].SharingPasswordWarningIcon.gameObject.SetActive(true);

                }
            }
        }
        CurrentArea = currentArea;
    }

    public void OnHealthZeroDestroyAreasWithSamePassword(rArea lostArea)
    {
        foreach(rArea area in listOfLevelAreas)
        {
            if (area != lostArea && area.Password == lostArea.Password)
                area.Health = 0;
        }
    }

    public void CheckCurrentAreaPasswordStrength()
    {
        /// load user personal data to check the password against them
        ///loadUserPrivateData();

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

        //Debug.Log("Correct call");

        if (length < 8)
        {
            strength = PasswordStrength.Weak;
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
        if (Regex.IsMatch(currentArea.Password, @"[A-Z]"))
        {
            complexity++;
        }
        if (Regex.IsMatch(currentArea.Password, @"[a-z]"))
        {
            complexity++;
        }
        if (Regex.IsMatch(currentArea.Password, @"[0-9]"))
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

        if (complexity < 2)
            strength = PasswordStrength.Weak;
        else if (complexity <= 3)
            strength = PasswordStrength.Moderate;
        else if (complexity > 3)
            strength = PasswordStrength.Strong;

        Debug.Log(complexity);

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


    public void SetAreaHealthBasedOnPassword()
    {
        foreach (string weakPassword in easyToGuessPasswords)
        {
            if (currentArea.Password.ToLower().Contains(weakPassword))
            {
                strength = PasswordStrength.Weak;
            }
        }

        switch (strength)
        {
            case PasswordStrength.Weak:
                currentArea.Health = MaxSoldiersNumber / 4;
                break;

            case PasswordStrength.Moderate:
                currentArea.Health = MaxSoldiersNumber / 2;
                break;

            case PasswordStrength.Strong:
                currentArea.Health = MaxSoldiersNumber;
                break;
        }

        currentArea.MeshColourChanger.LerpBetweenObjectColours(currentArea.Health / maxSoldiersNumber);
    }
    #region Radial formation
    //public void FormArmyBasedOnAreaHealth()
    //{
    //    int rings = 0;
    //    if (currentArea.Health <= MaxSoldiersNumber / 4)
    //        rings = 1;
    //    else if (currentArea.Health <= MaxSoldiersNumber / 2)
    //        rings = 2;
    //    else if (currentArea.Health <= MaxSoldiersNumber)
    //        rings = 3;

    //    /// later, it'd be better to send to the friendly soliders AI script both
    //    /// the password strength and complexity and the switch case is done there
    //    /// that way the functionality is separated and the password script knows nothing about the soliders

    //    ///also we may instantiate the army using StartCoroutine to instantiate one by one
    //    //RadialFormation rf = currentArea.GetComponentInChildren<RadialFormation>();
    //    //rf.Amount =(int)currentArea.Health;
    //    //rf.Rings = rings;
    //    AlliesSpawner ea = currentArea.GetComponentInChildren<AlliesSpawner>();
    //    ea.SetPrefabsTypes(soldiersType);
    //    ea.SetFormation();

    //    //currentArea.GetComponent<FriendSpawner>().SpawnFriends(solidersNumbers, soldiersType);
    //}
    #endregion

    //public void FormArmyBasedOnAreaHealth()
    //{
    //    currentArea.WeakArmy.gameObject.SetActive(false);
    //    currentArea.ModerateArmy.gameObject.SetActive(false);
    //    currentArea.StrongArmy.gameObject.SetActive(false);

    //    if (currentArea.Health <= MaxSoldiersNumber / 4)
    //    {
    //        currentArea.WeakArmy.gameObject.SetActive(true);
    //    }
    //    else if (currentArea.Health <= MaxSoldiersNumber / 2)
    //    {
    //        currentArea.WeakArmy.gameObject.SetActive(true);
    //        currentArea.ModerateArmy.gameObject.SetActive(true);

    //    }
    //    else if (currentArea.Health <= MaxSoldiersNumber)
    //    {
    //        currentArea.WeakArmy.gameObject.SetActive(true);
    //        currentArea.ModerateArmy.gameObject.SetActive(true);
    //        currentArea.StrongArmy.gameObject.SetActive(true);
    //    }

    ///// later, it'd be better to send to the friendly soliders AI script both
    ///// the password strength and complexity and the switch case is done there
    ///// that way the functionality is separated and the password script knows nothing about the soliders

    ///// also we may instantiate the army using StartCoroutine to instantiate one by one
    //        BoxFormation bf = currentArea.GetComponentInChildren<BoxFormation>();
    //        AlliesSpawner[] allies = currentArea.GetComponentsInChildren<AlliesSpawner>();
    //        foreach(AlliesSpawner s in allies)
    //        {
    //            s.SetPrefabsTypes(soldiersType);
    //            s.SetFormation();
    //        }

    //      //  currentArea.GetComponent<FriendSpawner>().SpawnFriends(solidersNumbers, soldiersType);
    //    }

        /// 

        private void CheckPassword(string password)
    {
        int length = password.Length;

        if (length == 0)
        {
            //Debug.Log($"Length = {length} in Area: {name}");
            return;
        }

        //Debug.Log("Correct call");

        if (password == currentArea.Password)
        {
            Debug.Log("Correct Password");
            currentArea.GetComponent<Collider>().isTrigger = true;
            return;
        }
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
