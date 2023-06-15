using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CS_SceneManager : MonoBehaviour
{
    
    [SerializeField] Animator _animator;

    public CS_SceneManager SceneManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager == null)
        {
            SceneManager = this;
            DontDestroyOnLoad(this);
            _animator= GetComponent<Animator>();    
        }

        else Destroy(this);
    }


    public void LoadSceneWithNumber(int SceneNumber)
    {
        
    }

}
