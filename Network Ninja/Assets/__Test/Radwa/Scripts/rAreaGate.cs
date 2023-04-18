using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rAreaGate : MonoBehaviour
{
    public bool isEntered = false;

    public rGameEvent OnAreaEntered;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameObjectsManager.Instance.Player)
        {
            //raise event
            // UI Manager will listen to this event
            //
            if(!isEntered)
            {
                OnAreaEntered.Raise(null);
                isEntered = true;
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == GameObjectsManager.Instance.Player)
    //    {
    //        //raise event
    //        // UI Manager will listen to this event
    //        //
    //        OnAreaEntered.Raise(null);
    //    }
    //}
}
