using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rTakeEnter : MonoBehaviour
{
    private Button myBtn;
    void Start()
    {
        myBtn = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            myBtn.onClick.Invoke();
        }
    }
}
