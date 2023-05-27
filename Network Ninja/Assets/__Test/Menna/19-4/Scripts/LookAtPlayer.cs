using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    //make HP looak at player (camera)
    public Transform cam;

    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
