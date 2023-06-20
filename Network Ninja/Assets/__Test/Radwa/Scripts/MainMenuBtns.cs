using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBtns : MonoBehaviour
{
    public void OpenStory()
    {
        CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.StoryScene);
    }
    public void OpenTutorial()
    {
        CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.TutorialScene);
    }

    public void OpenLevel1()
    {
        CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.GameplayScene);
    }
}
