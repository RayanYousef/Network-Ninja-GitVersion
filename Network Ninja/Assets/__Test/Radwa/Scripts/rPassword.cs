using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PasswordStrength { Weak, Moderate, Strong };

/* an all alphabet password spawns only melee soldiers,
 * an all numbers password spawns ranged soldiers,
 * an all symbols password spawns tanks.
 * Using all of them spawns a varying army depending on which type was used the most.
 */
public enum Soldiers { Melee, Ranged, MeleeRanged, MeleeRangedTank };

public class rPassword : MonoBehaviour
{
    private string[] playerPersonalData;
    private string[] easyToGuessPasswords = {"pAssword", "passw0rd", "123456789",
                                             "abcdefghi", "qwerty", "NetworkNinja"};
    private PasswordStrength strength;
    private Soldiers soldiersType;

    private int soldierNumber=0;

    //[SerializeField] private rSolidersManager solidersManager;

    [SerializeField] private FriendSpawner friendSpawner;


    public void CheckStrength(string password)
    {
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
            if(complexity > 0)
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

        // 3. Check personal data
        foreach (string weakPassword in playerPersonalData)
        {
            if (password.ToLower().Contains(weakPassword))
            {
                strength = PasswordStrength.Weak;
                FormArmy();
                return;
            }
        }
        
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
        switch(strength)
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

        friendSpawner.SpawnFriends(solidersNumbers, soldiersType);
        //solidersManager.InstantiateSoliders(solidersRows, soldiersType); 
    }

    public void AutoTest()
    {
        string[] autoTestPasswords = { "pAssw0rd", "12345678", "abcdefghi",
                                        "qwerty", "000000000" , "rrrrrrrrr",
                                        "Daiavoloz", "21102000", "01120273611",

                                        "menn@97", "ttch2007", "Ray1993", "Nadzy_3103",

                                        "bestNinja_2000", "MyFavColorGreen@001",
                                        "Rayan_93@NetworkNinja"};
        foreach (string testPassword in autoTestPasswords)
        {
            CheckStrength(testPassword);
            Debug.Log($"{testPassword } is { strength}");
        }
    }
}
