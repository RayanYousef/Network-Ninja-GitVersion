using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class msoundEffects : MonoBehaviour
{
    public AudioSource walk;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void walkSound()
    {
        walk.Play();
    }
}
