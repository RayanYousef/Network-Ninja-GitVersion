using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMOTION { Talking, Idle, Surprised }

[System.Serializable]

public class Emotion
{
    public EMOTION emotionType;
    public Sprite[] emotionSprite;
}
