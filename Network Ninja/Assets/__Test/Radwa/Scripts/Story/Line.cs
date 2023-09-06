using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LINETYPE { New, Append }

[System.Serializable]
public class Line
{
    public Character lineSpeaker;
    public EMOTION lineEmotion;
    public string says;
    public LINETYPE lineType;

    public bool hasTextEffect;
    public TextEffects[] textEffects;
    
    public bool hasCamEffect;
    public CameraEffects camEffects;

    public bool isChangeFontSize;
    public float fontSize;
    public bool isSkippable = true;

    float defualtFontSize = 55;

    public float DefualtFontSize { get => defualtFontSize; set => defualtFontSize = value; }
}
