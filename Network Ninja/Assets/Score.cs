using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ScoreObject",menuName ="SO/ScoreObject",order =0)]
public class Score : ScriptableObject
{
    [SerializeField] private float _score;

    public float ScoreHaamada { get => _score; set { _score = value; Debug.LogError($"Score {name} --- {value}"); } }

    [ContextMenu("Init")]
    public void Init() {
        Debug.Log("Hi");
        ScoreHaamada += 5;
    }
}