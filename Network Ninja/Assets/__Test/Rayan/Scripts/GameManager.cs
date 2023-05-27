using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] GameObject WinningPanel;


    // Start is called before the first frame update
    void Start()
    {
        StartGameManager();
    }

    private void StartGameManager()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndStage(bool WonGame)
    {
        WinningPanel.SetActive(true);
    }
}
