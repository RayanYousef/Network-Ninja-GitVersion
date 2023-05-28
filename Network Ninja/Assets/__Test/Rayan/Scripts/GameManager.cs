using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy, Normal, Hard
}

public enum GameState
{
    InProgress,Won,Lost
}
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] UnityEvent OnWinGame, OnLoseGame;

    public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
    public GameState CurrentGameState 
    { 
        get => currentGameState; 
        set 
        { 
            currentGameState = value;

            switch (CurrentGameState)
            {
                case GameState.InProgress:
                    break;

                case GameState.Lost:
                    OnLoseGame?.Invoke();
                    break;

                case GameState.Won:
                    OnWinGame?.Invoke();
                    break;
            }
        } 
    }

    [Header("Game State")]
    [SerializeField] GameState currentGameState;
    [SerializeField] Difficulty difficulty;
    [SerializeField] bool bossEntered;

    [Header("Panels")]
    [SerializeField] GameObject WinningPanel, LosePanel;


    // Start is called before the first frame update
    void Start()
    {
        StartGameManager();
        OnWinGame.AddListener(WinGame);
        OnLoseGame.AddListener(LostGame);
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

    public void WinGame()
    {
        WinningPanel.SetActive(true);
    }
    public void LostGame()
    {
        LosePanel.SetActive(true);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);  
    }
}
