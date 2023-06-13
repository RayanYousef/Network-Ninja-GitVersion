using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class m_IntroEffect : MonoBehaviour
{
    public CinemachineVirtualCamera effectCam;

    private void Start()
    {
        StartCoroutine(WaitForEffect());
    }

    private IEnumerator WaitForEffect()
    {
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 5.0f;
        yield return new WaitForSeconds(8);
        effectCam.enabled = false;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 5.0f;
    }
}
