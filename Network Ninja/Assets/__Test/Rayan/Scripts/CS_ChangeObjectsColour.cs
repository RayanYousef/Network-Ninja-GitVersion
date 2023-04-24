using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CS_ChangeObjectsColour
{
    [SerializeField] Renderer[] objectsRenderer;

    public void ChangeColour(Color value)
    {
        foreach (Renderer renderer in objectsRenderer)
        {
            renderer.material.color = value;
        }
    }
}
