using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAM_EFFECT { Shake }

[System.Serializable]

public class CameraEffects
{
    public CAM_EFFECT camEffectType = CAM_EFFECT.Shake;

    public float animationSpeed = 1;

    public void ApplyCamEffects()
    {
        switch (camEffectType)
        {
            case CAM_EFFECT.Shake:
                // call func
                CameraShake();
                break;

            default:
                break;
        }
    }

    public void CameraShake()
    {
        CameraManager.instance.Anim.SetTrigger("Shake");
        CameraManager.instance.Anim.speed = animationSpeed;
    }
}
