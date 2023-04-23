using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameObjectTeam { PlayerTeam, EnemyTeam, Neither}
public class CS_ObjectStats : MonoBehaviour,IDamageable
{
    [SerializeField] public GameObjectTeam team;
    [SerializeField] public float health,maxHealth, attack;

    public void ApplyDamage(float AttackPower)
    {
       
        health = Mathf.Clamp(health - AttackPower,0,maxHealth);
    }

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
