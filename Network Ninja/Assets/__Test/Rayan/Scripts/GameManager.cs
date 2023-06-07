using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy, Normal, Hard
}

public enum GameLang { English, Arabic }

public enum GameState
{
    InProgress,Won,Lost
}
public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] UnityEvent OnWinGame, OnLoseGame;

    [SerializeField] string MainMenu;

    public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
    public GameLang GameLang { get => gameLang; set => gameLang = value; }
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
                    rAreasManager.Instance.CurrentArea.MeshColourChanger.ChangeToColour(rAreasManager.Instance.MaxHealth);
                    OnWinGame?.Invoke();
                    break;
            }
        } 
    }

    public bool BossEntered 
    { 
        get => bossEntered;
        set 
        {
            bossEntered = value;
            switch(value)
            {
                case true:
                    AudioManager.instance.musicSource.Stop();
                    AudioManager.instance.BossMusic.gameObject.SetActive(true);
                    break;

                case false:

                    break;

            }
        } 
    }

    [Header("Game State")]
    [SerializeField] GameState currentGameState;
    [SerializeField] Difficulty difficulty;
    [SerializeField] GameLang gameLang;
    [SerializeField] bool bossEntered;

    [Header("Panels")]
    [SerializeField] GameObject WinningPanel, LosePanel;


    // Start is called before the first frame update
    void Start()
    {
        StartGameManager();
        OnWinGame.AddListener(WinGame);
        OnLoseGame.AddListener(LostGame);

        gameLang = GameLang.English;
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
        Invoke("LoadMainMenu", 3);
 
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenu);  
    }
}
