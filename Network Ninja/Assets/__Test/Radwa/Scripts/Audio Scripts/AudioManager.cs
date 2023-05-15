using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, pitchVariedsfxSource;

    [Header("Keyboard Single Clicks Array")]
    [SerializeField] AudioClip[] clickClips;
    [SerializeField] AudioClip[] footsteps;

    public AudioClip[] ClickClips { get => clickClips; set => clickClips = value; }
    public AudioClip[] Footsteps { get => footsteps; set => footsteps = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       // PlayMusic("MainMenuBG");
        PlayMusic("BackGround");
    }


    public void PlayMusic(string name)
    {

        Sound s = Array.Find(musicSounds, x => x.name == name);

        //Debug.Log(s.name);

        if (s != null)
        {
            // musicSource.PlayOneShot(s.clip, 0.2f);
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }


    //public void playSFX(string name, float SFXVolume)
    //{
    //    Sound s = Array.Find(sfxSounds, x => x.name == name);

    //    if (s != null)
    //    {
    //        if (sfxSource.clip == s.clip && sfxSource.isPlaying)
    //        {
    //            return;
    //        }
    //        Debug.Log(sfxSource.pitch);
    //        sfxSource.PlayOneShot(s.clip, SFXVolume);
    //    }
    //}

    public void playSFX(string name)
    {

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }

    }
    public void PlayVariedPitcheAudio(AudioClip[] audioClipsArray)
    {
        pitchVariedsfxSource.clip = audioClipsArray[UnityEngine.Random.Range(0, clickClips.Length)];
        pitchVariedsfxSource.volume = UnityEngine.Random.Range(0.5f, 1f);
        pitchVariedsfxSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        pitchVariedsfxSource.PlayOneShot(pitchVariedsfxSource.clip);
    }
}