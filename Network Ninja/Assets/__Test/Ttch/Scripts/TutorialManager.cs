using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public int popUpIndex = 0;
    private bool jumped, dashed;
    private int LAttackCount = 0;
    private int RAttackCount = 0;
    private bool ultUsed = false;
    public rArea Area1,Area2,Area3;

    [SerializeField] public UnityEvent OnTutorialFinish;

    // Update is called once per frame
    void Update()
    {
        CheckIndexAndIncrement();

        for (int i = 0; i < popUps.Length; i++)
        {
            if(i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }

        }
    }


    void CheckIndexAndIncrement()
    {
        switch (popUpIndex)
        {
            //Movement Tutorial
            case 0:
                if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    popUpIndex++;
                }
                break;

            //Jump and Dash Tutorial
            case 1:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumped = true;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    dashed = true;
                }
                if(jumped && dashed)
                {
                    popUpIndex++;
                }
                break;
            //Left Click Attack Tutorial
            case 2:
                
                    if (LAttackCount == 8)
                    {
                        popUpIndex++;
                    }
                
                break;

            //Right Click Attack Tutorial
            case 3:
                
                    if (RAttackCount == 3)
                    {
                        popUpIndex++;
                    }
                
                break;

            //Securing Rooms Tutorial
            case 4:
                if (Area1.AreaType == AreaType.Base)
                {
                    popUpIndex++;
                }
                break;

            //Ultimate Attack Tutorial in Area 2
            case 5:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    ultUsed = true;
                }
                if(ultUsed && Area2.AreaType == AreaType.Base)
                {
                    popUpIndex++;
                }
                break;

            //Area 3 Securing
            case 6:
                if(Area3.AreaType == AreaType.Base)
                {
                    Area2.Health = 50;
                    popUpIndex++;
                }
                break;

            //Resetting Password Tutorial
            case 7:
                if(Area2.Health == 100)
                {
                    Area1.AreaType = AreaType.Fight;
                    popUpIndex++;
                }
                break;

            case 8:
                if(Area1.AreaType == AreaType.Base)
                {
                    CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.GameplayScene);
                }
                break;

            default:
                break;

        }
    }

    public void IncrementLAttack()
    {
        LAttackCount++;
    }
    public void IncrementRAttack()
    {
        RAttackCount++;
    }
}
