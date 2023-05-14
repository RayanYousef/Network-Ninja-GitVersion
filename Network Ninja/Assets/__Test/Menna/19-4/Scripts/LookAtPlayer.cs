using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    //make HP looak at player (camera)
    public Transform cam;

    void Start()
    {
        //cam = GameObjectsManager.Instance.PlayerCamera.GetComponentInChildren<Camera>().transform;
        
    }

    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
