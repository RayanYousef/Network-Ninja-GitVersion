using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{

    [SerializeField]
     List<Collider> hitObjects = new List<Collider>();

    [SerializeField] 
    StatsStruct myStats = new StatsStruct();
    public StatsStruct MyStats { get => myStats; }
    public List<Collider> HitObjects { get => hitObjects; set => hitObjects = value; }

    private void Awake()
    {
        myStats.CurrentHealth = myStats.MaxHealth;
        myStats.Defense = myStats.DefaultDefense;
        myStats.AtkDmg = myStats.DefaultAtkDmg;
        myStats.AtkSpeed = myStats.DefaultAtkSpeed;
        myStats.MoveSpeed = myStats.DefaultMoveSpeed;
        myStats.CooldownReduction = myStats.DefaultCooldownReduction;

        var damageHandlers = GetComponentsInChildren<CS_DamageHandler>();
        foreach (CS_DamageHandler handler in damageHandlers)
        {
            handler.stats = this;
        }
    }

    #region HealthFunctions
    public void Heal(float value = 20)
    {
        myStats.CurrentHealth += value;
    }
    public void ApplyDamage(StatController AttackerStats)
    {
        float dmg = AttackerStats.CalculateAttackStrength() - this.GetDefense();
        myStats.CurrentHealth -= dmg;
    }
    #endregion

    #region AttackStrengthFunctions
    //Apply permenant/temporary buffs or debuffs to attack DAMAGE
    public void BuffAttackStat(float changeValue = 10)
    {
        myStats.AtkDmg += changeValue;
    }

    public void DebuffAttackStat(float changeValue = 10)
    {
        myStats.AtkDmg -= changeValue;
    }

    public void ResetAttackToDefault()
    {
        myStats.AtkDmg = myStats.DefaultAtkDmg;
    }

    public float CalculateAttackStrength()
    {
        float critChance = Random.Range(0, 1);
        if(critChance + myStats.Luck > 0.8f)
        {
            return myStats.AtkDmg * 2;
        }
        return myStats.AtkDmg;
    }

    #endregion

    #region AttackSpeedFunctions
    //Apply permenanty/temporary buffs or debuffs to attack SPEED
    public void BuffAttackSpeed(float buffValue = 0.2f)
    {
        myStats.AtkSpeed += buffValue;
    }
    public void DebuffAttackSpeed(float debuffValue = 0.2f)
    {
        myStats.AtkSpeed -= debuffValue;
    }
    public void ResetAttackSpeedToDefault()
    {
        myStats.AtkSpeed = myStats.DefaultAtkSpeed;
    }
  
    public float GetAtackSpeed()
    {
        return myStats.AtkSpeed;
    }
    #endregion

    #region DefenseFunctions
    
    public void BuffDefense(float changeValue = 10)
    {
        myStats.Defense += changeValue;
    }
    public void DebuffDefense(float changeValue = 10)
    {
        myStats.Defense -= changeValue;
    }

    public void ResetDefenseToDefault()
    {
        myStats.Defense = myStats.DefaultDefense;
    }

    public float GetDefense()
    {
        return myStats.Defense;
    }
    #endregion

    #region MoveSpeedFunctions
    
    // Movement Speed Modifier should be small, like 1.2 or 1.5, it will be multiplied by the character stats. 
    public void BuffMoveSpeed(float changeValue = 1.3f)
    {
        myStats.MoveSpeed += changeValue;
    }
    public void DebuffMoveSpeed(float changeValue = 1.2f)
    {
        myStats.MoveSpeed -= changeValue;
    }
    public void ResetMoveSpeedToDefault()
    {
        myStats.MoveSpeed = myStats.DefaultMoveSpeed;
    }
    public float GetMoveSpeed()
    {
        return myStats.MoveSpeed;
    }

    #endregion

    #region CooldownReductionFunctions

    public void BuffCooldownReduction(float changeVal)
    {
        myStats.CooldownReduction *= changeVal;
    }
    public void DebuffCoolDownReduction(float changeVal)
    {
        myStats.CooldownReduction /= changeVal;
    }
    private void ResetCooldownReductionToDefault()
    {
        myStats.CooldownReduction = myStats.DefaultCooldownReduction;
    }
    private float GetCooldownReduction()
    {
        return myStats.CooldownReduction;
    }
    private float CalculateCooldown(float BaseSkillCooldown)
    {
        return BaseSkillCooldown * (1 - GetCooldownReduction() / 100);
    }

    #endregion

    #region LuckFunctions

    public void BuffLuck (float changeVal = 0.1f)
    {
        myStats.Luck += changeVal;
    }

    public void DebuffLuck(float changeVal = 0.1f)
    {
        myStats.Luck -= changeVal;
    }
    public void ResetLuckToDefault()
    {
        myStats.Luck = myStats.DefaultLuck;
    }
    public float GetLuck()
    {
        return myStats.Luck;
    }
    #endregion

}