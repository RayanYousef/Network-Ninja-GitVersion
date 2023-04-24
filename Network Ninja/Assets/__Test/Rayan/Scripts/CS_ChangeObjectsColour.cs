using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CS_ChangeObjectsColour :MonoBehaviour
{
    [SerializeField] Renderer[] objectsRenderer;
    [SerializeField] Color MaxHealth, HalfHealth, LowHealth;

    //[SerializeField,Range(0, 1)] float colorState;

    //private void Update()
    //{
    //    ChangeColour(colorState);
    //}

    public void ChangeColour(float value)
    {
        foreach (Renderer renderer in objectsRenderer)
        {
            renderer.material.color = LerpColors(value);
        }
    }

    public Color LerpColors(float t)
    {
        if (t < 0.5f)
        {
            return Color.Lerp(LowHealth, HalfHealth, t * 2f);
        }
        else
        {
            return Color.Lerp(HalfHealth, MaxHealth, (t - 0.5f) * 2f);
        }
    }
}
