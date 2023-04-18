using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class rGameEventListener : MonoBehaviour
{
    // We want to know which event or radio station we want to listen to
    public rGameEvent gameEvent;

    // Unity events allow you to link method calls directly in the editor
    public UnityEvent<string> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    // OnEventRaised will be called by the rGameEvent when an event is broadcasted
    public void OnEventRaised(string str)
    {
        response.Invoke(str);
    }
}
