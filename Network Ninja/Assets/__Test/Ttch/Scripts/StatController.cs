using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{

    [SerializeField]
     List<Collider> hitObjects = new List<Collider>();

    [SerializeField] 
    StatsStruct myStats = new StatsStruct();


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


    #region Setters And Getters 
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    public StatsStruct MyStats { get => myStats; }
    public List<Collider> HitObjects { get => hitObjects; set => hitObjects = value; }

    #endregion

    private void Awake()
    {
        currentHealth = maxHealth;
        defense = defaultDefense;
        atkDmg = defaultAtkDmg;
        atkSpeed = defaultAtkSpeed;
        moveSpeed = defaultMoveSpeed;
        cooldownReduction = defaultCooldownReduction;
    }

    #region HealthFunctions
    public void Heal(float value = 20)
    {
        CurrentHealth += value;
    }
    public void ApplyDamage(StatController AttackerStats)
    {
        float dmg = AttackerStats.CalculateAttackStrength() - this.GetDefense();
        CurrentHealth -= dmg;
    }
    #endregion

    #region AttackStrengthFunctions
    //Apply permenant/temporary buffs or debuffs to attack DAMAGE
    public void BuffAttackStat(float changeValue = 10)
    {
        atkDmg += changeValue;
    }

    public void DebuffAttackStat(float changeValue = 10)
    {
        atkDmg -= changeValue;
    }

    public void ResetAttackToDefault()
    {
        atkDmg = defaultAtkDmg;
    }

    public float CalculateAttackStrength()
    {
        float critChance = Random.Range(0, 1);
        if(critChance + luck > 0.8f)
        {
            return atkDmg * 2;
        }
        return atkDmg;
    }

    #endregion

    #region AttackSpeedFunctions
    //Apply permenanty/temporary buffs or debuffs to attack SPEED
    public void BuffAttackSpeed(float buffValue = 0.2f)
    {
        atkSpeed += buffValue;
    }
    public void DebuffAttackSpeed(float debuffValue = 0.2f)
    {
        atkSpeed -= debuffValue;
    }
    public void ResetAttackSpeedToDefault()
    {
        atkSpeed = defaultAtkSpeed;
    }
  
    public float GetAtackSpeed()
    {
        return atkSpeed;
    }
    #endregion

    #region DefenseFunctions
    
    public void BuffDefense(float changeValue = 10)
    {
        defense += changeValue;
    }
    public void DebuffDefense(float changeValue = 10)
    {
        defense -= changeValue;
    }

    public void ResetDefenseToDefault()
    {
        defense = defaultDefense;
    }

    public float GetDefense()
    {
        return defense;
    }
    #endregion

    #region MoveSpeedFunctions
    
    // Movement Speed Modifier should be small, like 1.2 or 1.5, it will be multiplied by the character stats. 
    public void BuffMoveSpeed(float changeValue = 10)
    {
        moveSpeed += changeValue;
    }
    public void DebuffMoveSpeed(float changeValue = 5)
    {
        moveSpeed -= changeValue;
    }
    public void ResetMoveSpeedToDefault()
    {
        moveSpeed = defaultMoveSpeed;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    #endregion

    #region CooldownReductionFunctions

    public void BuffCooldownReduction(float changeVal)
    {
        cooldownReduction *= changeVal;
    }
    public void DebuffCoolDownReduction(float changeVal)
    {
        cooldownReduction /= changeVal;
    }
    private void ResetCooldownReductionToDefault()
    {
        cooldownReduction = defaultCooldownReduction;
    }
    private float GetCooldownReduction()
    {
        return cooldownReduction;
    }
    private float CalculateCooldown(float BaseSkillCooldown)
    {
        return BaseSkillCooldown * (1 - GetCooldownReduction() / 100);
    }

    #endregion

    #region LuckFunctions

    public void BuffLuck (float changeVal = 0.1f)
    {
        luck += changeVal;
    }

    public void DebuffLuck(float changeVal = 0.1f)
    {
        luck -= changeVal;
    }
    public void ResetLuckToDefault()
    {
        luck = defaultLuck;
    }
    public float GetLuck()
    {
        return luck;
    }
    #endregion

}