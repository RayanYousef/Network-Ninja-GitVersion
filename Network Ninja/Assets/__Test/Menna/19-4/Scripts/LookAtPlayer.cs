using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    //make HP looak at player (camera)
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObjectsManager.Instance.Player.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam);
        
    }
}
