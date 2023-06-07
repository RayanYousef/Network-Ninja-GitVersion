using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zxcvbn;

public class ShortResult
{
    private PasswordStrength strength = PasswordStrength.Strong;
    private string currentwarning = "";
    private string currentSuggestions = "";

    public ShortResult(PasswordStrength strengthVal = PasswordStrength.Weak, string currentWarningVal = "", string currentSuggestionsVal = "")
    {
        strength = strengthVal;
        currentwarning = currentWarningVal;
        currentSuggestions = currentSuggestionsVal;
    }

    public PasswordStrength _Strength { get => strength; set => strength = value; }
    public string _Currentwarning { get => currentwarning; set => currentwarning = value; }
    public string _CurrentSuggestions { get => currentSuggestions; set => currentSuggestions = value; }
}

public enum PasswordStrength { Weak, Moderate, Strong };

public static class rPasswordChecker
{
    public static ShortResult CheckPasswordStrengthWithZxccvbn(string password)
    {
        ShortResult r = new ShortResult();
        Result result = Core.EvaluatePassword(password);

        if (result.Score == 4)
            r._Strength = PasswordStrength.Strong;
        else if (result.Score == 3)
            r._Strength = PasswordStrength.Moderate;
        else
            r._Strength = PasswordStrength.Weak;

        r._Currentwarning = result.Feedback.Warning;

        int cnt = result.Feedback.Suggestions.Count;
        if (cnt > 0)
        {
            int i = Random.Range(0, cnt);
            r._CurrentSuggestions = result.Feedback.Suggestions[i];
        }
        
        return r;
    }
}
