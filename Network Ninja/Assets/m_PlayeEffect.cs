using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_PlayeEffect : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

    }


}
