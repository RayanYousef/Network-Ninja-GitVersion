using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct StatsStruct
{
    [SerializeField] UnityEvent<float> OnHealthUpdated;

    [SerializeField]
    private float maxHealth, currentHealth;
    [SerializeField]
    private float defaultDefense, defense;
    [SerializeField]
    private float defaultAtk, atk;
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
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthUpdated?.Invoke(currentHealth);
        }
    }

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float DefaultDefense { get => defaultDefense; set => defaultDefense = value; }
    public float Defense { get => defense; set => defense = value; }
    public float DefaultAtk { get => defaultAtk; set => defaultAtk = value; }
    public float Atk { get => atk; set => atk = value; }
    public float DefaultAtkSpeed { get => defaultAtkSpeed; set => defaultAtkSpeed = value; }
    public float AtkSpeed { get => atkSpeed; set => atkSpeed = value; }
    public float DefaultMoveSpeed { get => defaultMoveSpeed; set => defaultMoveSpeed = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DefaultCooldownReduction { get => defaultCooldownReduction; set => defaultCooldownReduction = value; }
    public float CooldownReduction { get => cooldownReduction; set => cooldownReduction = value; }
    public float DefaultLuck { get => defaultLuck; set => defaultLuck = value; }
    public float Luck { get => luck; set => luck = value; }

    #endregion
}
