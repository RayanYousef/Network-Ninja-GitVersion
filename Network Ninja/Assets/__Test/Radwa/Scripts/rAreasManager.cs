using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
using Zxcvbn;


//public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };
public enum PasswordStrength { Weak, Moderate, Strong };

public class rAreasManager : MonoBehaviour
{
    private static rAreasManager instance;
    [SerializeField] rUIPassword passwordCanvas;

    [Header("Password Manager Components")]
    [SerializeField] rArea[] listOfLevelAreas;
    int maxSoldiersNumber = 75;
    [SerializeField] Color maxHealth, halfHealth, lowHealth, enemyColor;

    [SerializeField] rArea currentArea;
   // [SerializeField] rArea nextArea;
    [SerializeField] PasswordStrength strength;
    [SerializeField] Result result;

    [Header("Password Result Details")]
    [SerializeField] string currentWarnings = null;
    [SerializeField] string currentSuggestions = null;

    [Header("Password Lists")]
    private string[] playerPersonalData;

    //
    public static rAreasManager Instance { get => instance; }
    public rArea CurrentArea { get { return currentArea; } set => currentArea = value; }
   // public rArea NextArea { get => nextArea; set => nextArea = value; }

    public int MaxSoldiersNumber { get => maxSoldiersNumber; }
    public Color MaxHealth { get => maxHealth; }
    public Color HalfHealth { get => halfHealth; }
    public Color LowHealth { get => lowHealth; }
    public Color EnemyColor { get => enemyColor;}
    public rUIPassword PasswordCanvas { get => passwordCanvas; set => passwordCanvas = value; }
    public string Warnings { get => currentWarnings; set => currentWarnings = value; }
    public string Suggestions { get => currentSuggestions; set => currentSuggestions = value; }
    public rArea[] ListOfLevelAreas { get => listOfLevelAreas; set => listOfLevelAreas = value; }

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
        // loadUserPrivateData();
        
        result = Core.EvaluatePassword(currentArea.Password);

        if (result.Score == 4)
            strength = PasswordStrength.Strong;
        else if (result.Score == 3)
            strength = PasswordStrength.Moderate;
        else
            strength = PasswordStrength.Weak;

        currentWarnings = null;
        currentWarnings = result.Feedback.Warning;

        currentSuggestions = null;

        int cnt = result.Feedback.Suggestions.Count;
        if (cnt > 0)
        {
            int i = UnityEngine.Random.Range(0, cnt);
            currentSuggestions = result.Feedback.Suggestions[i];
        }
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
            if (area == rAreasManager.Instance.CurrentArea)
            {
                continue;
            }
            if (area.AreaType != AreaType.Base)
            {
                return false;
            }
        }
        return true;
    }

}
