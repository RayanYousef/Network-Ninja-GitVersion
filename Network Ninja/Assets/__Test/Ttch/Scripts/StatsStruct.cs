using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StatsStruct
{
    [SerializeField]
    private float maxHealth, currentHealth;
    [SerializeField]
    private float defaultDefense, defense;
    [SerializeField]
    private float defaultAtkDmg, atkDmg;
    [SerializeField]
    private float defaultAtkSpeed, atkSpeed;
    [SerializeField]
    private float defaultMoveSpeed, moveSpeed;
    [SerializeField]
    private float defaultCooldownReduction, cooldownReduction;
    [SerializeField]
    private float defaultLuck, luck;


    #region Setters and Getters
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    #endregion
}
