using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Visual Novel/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Emotion[] arrayOfEmotions;
}
