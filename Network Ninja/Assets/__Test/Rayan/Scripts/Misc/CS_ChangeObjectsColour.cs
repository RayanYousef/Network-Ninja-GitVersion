using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CS_ChangeObjectsColour
{
    public Renderer[] MeshRenderers;
    public Image[] Images;
    public Color MaxHealth, HalfHealth, LowHealth;


    public void ChangeToColour(Color color)
    {
        if (MeshRenderers.Length > 0)
            foreach (Renderer renderer in MeshRenderers)
            {
                renderer.material.color = color;
            }

        if (Images.Length > 0)
            foreach (Image image in Images)
            {
                image.color = color;
            }
    }
    public void LerpBetweenThreeGivenColours(float value, Color LowHealth, Color HalfHealth, Color MaxHealth)
    {
        if (MeshRenderers.Length > 0)
            foreach (Renderer renderer in MeshRenderers)
            {
                renderer.material.color = LerpColors(value, LowHealth, HalfHealth, MaxHealth);
            }

        if (Images.Length > 0)
            foreach (Image image in Images)
            {
                image.color = LerpColors(value, LowHealth, HalfHealth, MaxHealth);
            }
    }

    public void LerpBetweenObjectColours(float value)
    {

        if (MeshRenderers.Length > 0)
            foreach (Renderer renderer in MeshRenderers)
        {
            renderer.material.color = LerpColors(value, LowHealth, HalfHealth, MaxHealth);
        }

        if (Images.Length > 0)
            foreach (Image image in Images)
            {
                image.color = LerpColors(value, LowHealth, HalfHealth, MaxHealth);
            }
    }

    public Color LerpColors(float t, Color LowHealth, Color HalfHealth, Color MaxHealth)
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
