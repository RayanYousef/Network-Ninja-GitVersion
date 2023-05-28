using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DisableParticleOnEnd : MonoBehaviour
{
    ParticleSystem particle;
    // Start is called before the first frame update
    void Awake()
    {
        particle= GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(particle.isPlaying==false)
            DisableGameObject();

    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
