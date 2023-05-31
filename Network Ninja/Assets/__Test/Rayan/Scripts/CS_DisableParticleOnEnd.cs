using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DisableParticleOnEnd : MonoBehaviour
{
    [SerializeField]ParticleSystem particle;
    [SerializeField]AudioClip SFXClip;
    [SerializeField, Range(-3, 3)] float pitch = 1;
    [SerializeField, Range(0, 1)] float volume = 1;
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

    private void OnEnable()
    {
        if (SFXClip != null && AudioManager.instance != null)
            foreach (var audioSource in AudioManager.instance.audioSources)
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.volume = volume;
                    audioSource.clip = SFXClip;
                    audioSource.pitch = pitch;
                    audioSource.Play();
                    return;
                }

            }
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
