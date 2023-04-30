using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState;
    void Start()
    {
        currentState = GetInitState();
        if(currentState == null)
        {
            currentState.Enter();
        }
    }

    public void OnChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitState()
    {
        return null;
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }
}
