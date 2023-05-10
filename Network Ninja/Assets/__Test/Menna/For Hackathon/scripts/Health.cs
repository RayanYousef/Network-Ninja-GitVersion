using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    public Slider HealthBar;

    public EnemySpawner enemySpawner;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
       
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthBar.value = currentHealth; 
    }

    //Health and damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("take damage");
        animator.Play("TakeDamage");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died");

            Die();
            


        }
    }
    public virtual void Die()
    {
        //animation
        Debug.Log("When enemy died");
        animator.SetTrigger("Death");
        // Destroy(gameObject, 4f);
       // enemySpawner.enemies.Remove(gameObject);
       



        // enemySpawner.numAlive--;

        // Check if we need to spawn more enemies


    }

    void DeactivateGameObject()
    {

        gameObject.SetActive(false);

        enemySpawner = GetComponentInParent<EnemySpawner>();

        this.currentHealth = maxHealth;   
        enemySpawner.enemyPool.Add(gameObject);
        enemySpawner.enemies.Remove(gameObject);

        if (enemySpawner.MiniBosses.Count > 0)
        {
            enemySpawner.SpawnMoreEnemies();
        }

    }

}
