using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CS_SceneManager : MonoBehaviour
{
    enum SceneMethod { Number,String};
    
    public static CS_SceneManager Instance;
    [Header("The Number of the Scene in Build Hierarchy")]
    [SerializeField] int mainMenuScene = 0;
    [SerializeField] int storyScene=1,tutorialScene=2,easyLevelScene=3,gameplayScene=4;

    [Header("Next Scene")]
    int nextSceneNumber;
    string nextSceneString;
    SceneMethod loadingSceneMethod;

    public int MainMenuScene { get => mainMenuScene;}
    public int StoryScene { get => storyScene;}
    public int TutorialScene { get => tutorialScene;}
    public int EasyLevelScene { get => easyLevelScene; }
    public int GameplayScene { get => gameplayScene;}


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    public void LoadNextScene()
    {
        switch (loadingSceneMethod)
            {
            case SceneMethod.Number:
                SceneManager.LoadScene(nextSceneNumber);
                break;
            case SceneMethod.String:
                SceneManager.LoadScene(nextSceneString);
                break;
        }
    }

    public void LoadSceneByNumber(int number)
    {
        gameObject.SetActive(true);
        loadingSceneMethod = SceneMethod.Number;
        nextSceneNumber= number;
        GetComponent<Animator>().Play("Close Scene");
    }

    public void LoadSceneByString(string name)
    {
        gameObject.SetActive(true);
        loadingSceneMethod = SceneMethod.String;
       nextSceneString= name;
        GetComponent<Animator>().Play("Close Scene");



    }
    
    public  void DisableObject()
    {
        gameObject.SetActive(false);
    }

}
