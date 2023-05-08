//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using Zxcvbn;

//public class PreBuiltLibrary : MonoBehaviour
//{
//    string[] autoTestPasswords = { "pAssw0rd", "12345678", "abcdefghi",
//                                    "qwerty", "000000000" , "rrrrrrrrr",
//                                    "Daiavoloz", "21102000", "01120273611",

//                                    "menn@97", "ttch2007", "Ray1993", "Nadzy_3103",

//                                    "bestNinja_2000", "MyFavColorGreen@001",
//                                    "Rayan_93@NetworkNinja", "itsNeverLate4869@f"};

//    string fileLocation = "C:\\Users\\Radwa\\Desktop\\Graduation\\Network-Ninja-GitVersion\\Network Ninja\\Assets\\__Test\\Radwa\\zxcvbn";
//    string fileName = "ComparePasswordResults.txt";

//    void Start()
//    {
//        // delete file if exist
//        File.Delete(Path.Combine(fileLocation, fileName));

//        foreach (string testPassword in autoTestPasswords)
//        {
//            CheckCurrentAreaPasswordStrength(testPassword);
//            AutoTest(testPassword);
//        }
//    }

//    public void AutoTest(string testPassword)
//    {
//        Result result = Zxcvbn.Core.EvaluatePassword(testPassword);

//        //string str = $"zxcvbn score for {testPassword } is { result.Score}.\n" +
//        //             $"zxcvb CalcTime for {testPassword} is {result.CalcTime}.\n" +

//        //             $"zxcvb CrackTime for {testPassword} is {result.CrackTime.OfflineFastHashing1e10PerSecond}.\n" +
//        //             $"zxcvb CrackTimeDisplay for {testPassword} is {result.CrackTimeDisplay.OfflineFastHashing1e10PerSecond}.\n" +

//        //             $"zxcvb Feedback Warning for {testPassword} is {result.Feedback.Warning}.\n" +
//        //             $"zxcvb Feedback Suggestion 1 for {testPassword} is {result.Feedback.Suggestions[0]}.\n" +
//        //             $"zxcvb Guesses for {testPassword} is {result.Guesses}.\n" +

//        //             $"zxcvb GuessesLog10 for {testPassword} is {result.GuessesLog10}.\n" +
//        //             $"zxcvb MatchSequence for {testPassword} is {result.MatchSequence}.\n";


//        string str = $"zxcvbn score for {testPassword } is { result.Score}.\n" +
//                     $"zxcvb CrackTimeDisplay for {testPassword} is {result.CrackTimeDisplay.OfflineFastHashing1e10PerSecond}.\n"
//                     //+
//                     //$"zxcvb CrackTimeDisplay for {testPassword} is {result.CrackTimeDisplay.OfflineSlowHashing1e4PerSecond}.\n" +
//                     //$"zxcvb CrackTimeDisplay for {testPassword} is {result.CrackTimeDisplay.OnlineNoThrottling10PerSecond}.\n" +
//                     //$"zxcvb CrackTimeDisplay for {testPassword} is {result.CrackTimeDisplay.OnlineThrottling100PerHour}.\n"
//                     ;

//        if (result.Feedback.Warning.Length != 0)
//        {
//            str += $"zxcvb Feedback Warning for {testPassword} is {result.Feedback.Warning}.\n";
//        }

//        int cnt = result.Feedback.Suggestions.Count;

//        if (cnt != 0)
//        {
//            for (int i = 0; i < cnt; i++)
//            {
//                str += $"zxcvb Feedback Suggestion {i} for {testPassword} is {result.Feedback.Suggestions[i]}.\n";
//            }
//        }

//        WriteToFile(fileName, str);

//        //Debug.Log(str);
//    }

//    public void CheckCurrentAreaPasswordStrength(string password)
//    {
//        PasswordStrength strength;
//        /// load user personal data to check the password against them
//        ///loadUserPrivateData();

//        /// to calculate the strength of the password, the following will be checked
//        /// 1. the length
//        /// 2. the complexity
//        /// 3. personal data (username, birth date, etc..)
//        /// 4. comparing to previous passwords and common used passwords (12345678, qwerty, etc...)

//        /// 1. Check length
//        int length = password.Length;
//        int complexity = 0;

//        if (length == 0)
//        {
//            //Debug.Log($"Length = {length} in Area: {name}");
//            return;
//        }

//        //Debug.Log("Correct call");

//        if (length < 8)
//        {
//            strength = PasswordStrength.Weak;
//        }
//        else if (length < 12)
//        {
//            strength = PasswordStrength.Moderate;
//        }
//        else
//        {
//            strength = PasswordStrength.Strong;
//        }

//        /// 2. Check complexity
//        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
//        {
//            complexity++;
//        }
//        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]"))
//        {
//            complexity++;
//        }
//        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]"))
//        {
//            //if (complexity > 0)
//            //{
//            //    soldiersType = Soldiers.MeleeRanged;
//            //}
//            //else
//            //{
//            //    soldiersType = Soldiers.Ranged;
//            //}
//            complexity++;
//        }
//        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
//        {
//            //soldiersType = Soldiers.MeleeRangedTank;
//            complexity++;
//        }

//        if (complexity < 2)
//            strength = PasswordStrength.Weak;
//        else if (complexity <= 3)
//            strength = PasswordStrength.Moderate;
//        else if (complexity > 3)
//            strength = PasswordStrength.Strong;

//        //Debug.Log(complexity);
//        string str = $"Manual algorithm strength for {password } is { strength}.";
//        WriteToFile(fileName, str);
//        //Debug.Log(str);
//    }

//    public void WriteToFile(string fileName, string text)
//    {
//        // Combine the filename with the persistent data path (the directory where Unity saves data)
//        string filePath = Path.Combine(fileLocation, fileName);

//        // Create a new stream writer object to write to the file
//        StreamWriter writer = new StreamWriter(filePath, true);

//        // Write the text to the file
//        writer.WriteLine(text);

//        // Close the writer to save the changes to the file
//        writer.Close();
//    }

//}
