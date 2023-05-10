using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
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
        currentHealth += value;
        
        //Current Health can't exceed Max. Health
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void ApplyDamage(StatController AttackerStats)
    {
        float dmg = AttackerStats.CalculateAttackStrength() - this.GetDefense();
        currentHealth -= dmg;
    }
    
    public float GetHealth()
    {
        return currentHealth;
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