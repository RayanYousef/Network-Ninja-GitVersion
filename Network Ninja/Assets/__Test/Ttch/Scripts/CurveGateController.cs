using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveGateController : MonoBehaviour
{
    public PathCreator path;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PathFollower>() != null)
        {
            other.gameObject.GetComponent<PathFollower>().GetCurrentPath(path);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<PathFollower>() != null)
    //    {
    //        other.gameObject.GetComponent<PathFollower>().EmptyPath();
    //    }
    //}
}
