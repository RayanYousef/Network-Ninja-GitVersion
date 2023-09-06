using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMOTION { IDLE, SAD, SURPRISED, EXCITED }

[System.Serializable]

public class Emotion
{
    public EMOTION emotionType;
    public Sprite emotionSprite;
}
