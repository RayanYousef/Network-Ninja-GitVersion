using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
/**
 * Game event acts as a channel or radio station
 * it stores the list of listeners (rGameEventListener)
 */
public class rGameEvent : ScriptableObject
{
    public List<rGameEventListener> listeners = new List<rGameEventListener>();

    // Raise event (broadcast) through different methods
    public void Raise(string str)
    {
        // loop over all listeners and call OnEventRaised
        for(int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(str);
        }
    }

    // Manage listeners
    public void RegisterListener(rGameEventListener listener)
    {
        if(!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(rGameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

}
