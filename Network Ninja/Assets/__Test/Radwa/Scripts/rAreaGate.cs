using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rAreaGate : MonoBehaviour
{
    public rGameEvent OnAreaEntered;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            //raise event
            // UI Manager will listen to this event
            OnAreaEntered.Raise(null);
        }
    }
}
