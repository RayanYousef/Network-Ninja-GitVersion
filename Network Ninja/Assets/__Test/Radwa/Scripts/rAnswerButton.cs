using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rAnswerButton : MonoBehaviour
{
    [SerializeField] private bool isCorrect;

    public bool IsCorrect { get => isCorrect; set => isCorrect = value; }

}
