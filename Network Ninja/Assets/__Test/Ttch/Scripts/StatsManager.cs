using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public enum CharacterTeam
{
    None,Player, Enemy
}
public class StatsManager : MonoBehaviour
{
    [Header("Parent Of This Object")]
    [SerializeField] GameObject parent;

    [SerializeField] List<Collider> hitObjects = new List<Collider>();

    [SerializeField] StatsStruct myStats = new StatsStruct();

    [SerializeField] public CharacterTeam Team= CharacterTeam.None;

    [SerializeField] CS_DamageObject[] damageObjects;
    [SerializeField] Slider HealthBar;



    #region Setter and Getters
    public StatsStruct Stats { get => myStats; }
    public List<Collider> HitObjects { get => hitObjects; set => hitObjects = value; } 
    #endregion

    private void Awake()
    {
        myStats.CurrentHealth = myStats.MaxHealth;
        myStats.Defense = myStats.DefaultDefense;
        myStats.Atk = myStats.DefaultAtk;
        myStats.AtkSpeed = myStats.DefaultAtkSpeed;
        myStats.MoveSpeed = myStats.DefaultMoveSpeed;
        myStats.CooldownReduction = myStats.DefaultCooldownReduction;

        if (HealthBar != null)
        {
            HealthBar.maxValue = myStats.MaxHealth;
            HealthBar.value = myStats.CurrentHealth;
        }

        // Set Parent to this if parent field was null
        if (parent == null)
            parent = gameObject;

        // Damaging Objects are every child of this game object that contains the script CS_DamageObject.
        damageObjects = parent.GetComponentsInChildren<CS_DamageObject>();
        foreach (CS_DamageObject damageObject in damageObjects)
        {
            damageObject.MyStatsManager = this;
        }
    }

    #region Enable/Disable Damage Collider Based on Animation Event
    public void EnableAllWeapons()
    {
        foreach (CS_DamageObject damageObject in damageObjects)
        {
            damageObject.gameObject.SetActive(true);
        }
    }
    public void DisableAllWeapons()
    {
        foreach (CS_DamageObject damageObject in damageObjects)
        {
            damageObject.gameObject.SetActive(false);
        }
    }

    public void EnableWeaponWithName(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in damageObjects)
        {
            if(damageObject.WeaponName == WeaponName)
                damageObject.gameObject.SetActive(true);
        }
    }

    public void DisableWeaponWithName(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in damageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
                damageObject.gameObject.SetActive(false);
        }
    }


    #endregion

    #region HealthFunctions
    public void Heal(float value = 20)
    {
        myStats.CurrentHealth += value;
        HealthBar.value = myStats.CurrentHealth;
    }
    public void ApplyDamage(StatsManager AttackerStats)
    {
        float dmg = AttackerStats.CalculateAttackStrength() - this.GetDefense();
        myStats.CurrentHealth -= dmg;
        if(HealthBar!=null) 
        HealthBar.value = myStats.CurrentHealth;
        Debug.Log(myStats.CurrentHealth);
    }

    public void ApplyDamage(float attack=30)
    {
        myStats.CurrentHealth -= attack;
        if(HealthBar!= null)
        HealthBar.value = myStats.CurrentHealth;
        if (myStats.CurrentHealth == 0)
        {
            parent.SetActive(false);
        }

    }
    #endregion

    #region AttackStrengthFunctions
    //Apply permenant/temporary buffs or debuffs to attack DAMAGE
    public void BuffAttackStat(float changeValue = 10)
    {
        myStats.Atk += changeValue;
    }

    public void DebuffAttackStat(float changeValue = 10)
    {
        myStats.Atk -= changeValue;
    }

    public void ResetAttackToDefault()
    {
        myStats.Atk = myStats.DefaultAtk;
    }

    public float CalculateAttackStrength()
    {
        float critChance = Random.Range(0, 1);
        if(critChance + myStats.Luck > 0.8f)
        {
            return myStats.Atk * 2;
        }
        return myStats.Atk;
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