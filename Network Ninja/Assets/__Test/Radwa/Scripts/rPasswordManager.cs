using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
using Zxcvbn;


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

    [SerializeField] rArea currentArea;
    [SerializeField] PasswordStrength strength;
    [SerializeField] Result result;

    [Header("Password Result Details")]
    [SerializeField] string warnings = null;
    [SerializeField] string suggestions = null;

    [Header("Password Lists")]
    private string[] playerPersonalData;

    //
    public static rPasswordManager Instance { get => instance; }
    public rArea CurrentArea { get { return currentArea; } set => currentArea = value; }

    public int MaxSoldiersNumber { get => maxSoldiersNumber; }
    public Color MaxHealth { get => maxHealth; }
    public Color HalfHealth { get => halfHealth; }
    public Color LowHealth { get => lowHealth; }
    public Color EnemyColor { get => enemyColor;}
    public rUIPassword PasswordCanvas { get => passwordCanvas; set => passwordCanvas = value; }
    public string Warnings { get => warnings; set => warnings = value; }
    public string Suggestions { get => suggestions; set => suggestions = value; }

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
        else if (result.Score == 3 || result.Score == 4)
            strength = PasswordStrength.Moderate;
        else
            strength = PasswordStrength.Weak;

        warnings = result.Feedback.Warning;
        foreach(string s in result.Feedback.Suggestions)
        {
            suggestions += s;
            suggestions += ". ";
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
        private void CheckPassword(string password)
        {

        if (password.Length == 0)
        {
            return;
        }

        if (password == currentArea.Password)
        {
            Debug.Log("Correct Password");
            currentArea.GetComponent<Collider>().isTrigger = true;
            return;
        }
    }
}
