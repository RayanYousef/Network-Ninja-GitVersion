using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mMainMenu : MonoBehaviour
{
    public string FirstLevel;
    public string Level1;
    public string Level2;
    public string Level3;
    public GameObject optionsScreen;
    public GameObject controlScreen;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void startGame()
    {
        SceneManager.LoadScene(FirstLevel);
    }

    public void GoToLevel1()
    {
        if(Level1 != null)
        {
            SceneManager.LoadScene(Level1);

        }

    }

    public void GoToLevel2()
    {
        if (Level2 != null)
        {
            SceneManager.LoadScene(Level2);

        }
    }
    public void GoToLevel3()
    {
        if (Level3 != null)
        {
            SceneManager.LoadScene(Level3);

        }
    }


    public void openOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void closeOptions()
    {

        optionsScreen.SetActive(false);
    }
    public void openControls()
    {
        controlScreen.SetActive(true);
    }

    public void closeControls()
    {

        controlScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
