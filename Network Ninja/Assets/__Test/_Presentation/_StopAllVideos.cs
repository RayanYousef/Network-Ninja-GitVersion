using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class _StopAllVideos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        foreach (VideoPlayer video in GetComponentsInChildren<VideoPlayer>())
            video.Play();
    }
    private void OnDisable()
    {
        foreach (VideoPlayer video in GetComponentsInChildren<VideoPlayer>())
            video.Stop();
    }
}
