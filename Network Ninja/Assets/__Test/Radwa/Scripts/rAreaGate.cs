using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class rAreaGate : MonoBehaviour
{
    public bool isEntered = false;

    //public rGameEvent OnAreaEntered;

    [SerializeField] UnityEvent<string> OnAreaEnteredEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameObjectsManager.Instance.Player)
        {
            //raise event
            // UI Manager will listen to this event
            //
            if(!isEntered)
            {
                //OnAreaEntered.Raise(null);
                OnAreaEnteredEvent?.Invoke(null);   
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
