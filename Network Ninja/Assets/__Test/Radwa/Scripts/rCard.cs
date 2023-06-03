using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class rCard : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cardCam;
    [SerializeField] Animator anim;
    void Start()
    {
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 1.0f;
        anim.SetBool("startRotation", true);
        cardCam.enabled = true;
    }

    public void CardPlaced()
    {
        // play video

        // light leds

        // show next btn
    }

    public void OnClkNext()
    {
        // spawn big boss

        // disable card cam

        // hide card canvas
    }

}
