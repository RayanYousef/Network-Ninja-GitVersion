using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rCipherManager : MonoBehaviour
{
    private static rCipherManager instance;

    [SerializeField] bool autoDie;

    public static rCipherManager Instance { get => instance; }

    public bool AutoDie { get => autoDie; set => autoDie = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
