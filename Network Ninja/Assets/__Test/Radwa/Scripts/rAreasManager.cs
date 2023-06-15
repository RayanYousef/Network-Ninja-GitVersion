using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
using UnityEngine.Events;


//public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };

public class rAreasManager : MonoBehaviour
{
    private static rAreasManager instance;
    [SerializeField] rUIPassword passwordCanvas;

    [Header("Area Manager Components")]
    [SerializeField] rArea[] listOfLevelAreas;
    int maxSoldiersNumber = 75;
    [SerializeField] Color maxHealth, halfHealth, lowHealth;
    [SerializeField] Color darkColor;
    [SerializeField] rArea currentArea;

    [Header("Password Result Details")]
    ShortResult shortResult;
    [SerializeField] PasswordStrength strength;
    [SerializeField] string currentWarning = null;
    [SerializeField] string currentSuggestions = null;

    [Header("Tutorial")]
    [SerializeField] bool isTutorial = false;
    [SerializeField] public UnityEvent OnAreasFinished;

    /*
    [Header("Password Lists")]
    private string[] playerPersonalData;
    */

    
    public static rAreasManager Instance { get => instance; }
    public rUIPassword PasswordCanvas { get => passwordCanvas; set => passwordCanvas = value; }
    
    public rArea[] ListOfLevelAreas { get => listOfLevelAreas; set => listOfLevelAreas = value; }
    public int MaxSoldiersNumber { get => maxSoldiersNumber; }
    public Color MaxHealth { get => maxHealth; }
    public Color HalfHealth { get => halfHealth; }
    public Color LowHealth { get => lowHealth; }
    public Color DarkColor { get => darkColor; set => darkColor = value; }

    public rArea CurrentArea { get { return currentArea; } set => currentArea = value; }
    
    public string Warnings { get => currentWarning; set => currentWarning = value; }
    public string Suggestions { get => currentSuggestions; set => currentSuggestions = value; }
    public bool IsTutorial { get => isTutorial; set => isTutorial = value; }

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
            area.SharingPasswordWarningIcon.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            //area.SharingPasswordWarningIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < listOfLevelAreas.Length; i++)
        {
            for (int j = i + 1; j < listOfLevelAreas.Length; j++)
            {
                if (listOfLevelAreas[i].Password!=null && listOfLevelAreas[i].Password == listOfLevelAreas[j].Password)
                {
                    listOfLevelAreas[i].SharingPasswordWarningIcon.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                    listOfLevelAreas[j].SharingPasswordWarningIcon.gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;

                    //listOfLevelAreas[i].SharingPasswordWarningIcon.gameObject.SetActive(true);
                    //listOfLevelAreas[j].SharingPasswordWarningIcon.gameObject.SetActive(true);
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
        // loadUserPrivateData();

        shortResult = rPasswordChecker.CheckPasswordStrengthWithZxccvbn(currentArea.Password);

        strength = shortResult._Strength;
        currentWarning = shortResult._Currentwarning;
        currentSuggestions = shortResult._CurrentSuggestions;
    }

    /*
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
    */

    public void SetAreaHealthBasedOnPassword()
    {
        switch (strength)
        {
            case PasswordStrength.Weak:
                currentArea.Health = 25;
                break;

            case PasswordStrength.Moderate:
                currentArea.Health = 50;
                break;

            case PasswordStrength.Strong:
                currentArea.Health = MaxSoldiersNumber;
                break;
        }

        currentArea.MeshColourChanger.LerpBetweenObjectColours(currentArea.Health / maxSoldiersNumber);
    }
    private void CheckPassword(string password)
    {

        if (password.Length == 0)
        {
            return;
        }

        if (password == currentArea.Password)
        {
            Debug.Log("Correct Password");
            return;
        }
    }

    public bool CheckAllAreasBaseExceptCurrent()
    {
        foreach (rArea area in listOfLevelAreas)
        {
            if (area == CurrentArea)
            {
                continue;
            }
            if (area.AreaType != AreaType.Base && area.AreaType != AreaType.Main)
            {
                return false;
            }
        }
        // invoke event all areas finished
        if(isTutorial)
        {
            OnAreasFinished?.Invoke();
        }
        return true;
    }

}
