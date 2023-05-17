using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    private Rigidbody rb;
    private float forceMagnitude = 10.0f;

    public GameObject DeathEffect;

    public int maxHealth;
    public int currentHealth;
    public Slider HP;
    public Animator animator;
    public Slider HealthBar;
    public EnemySpawner enemySpawner;
    

    void Start()
    {
        animator = GetComponent<Animator>();
       
        currentHealth = maxHealth;
        if(HP== null)
        HP = GetComponentInChildren<Slider>();
        HP.maxValue = maxHealth;
        rb = GetComponent<Rigidbody>();
        //StartCoroutine(RandomlyApplyDamage());
    }

    IEnumerator RandomlyApplyDamage()
    {
        float r = UnityEngine.Random.Range(1, 3);
        yield return new WaitForSeconds(r);
        TakeDamage(maxHealth);
    }

    private void Update()
    {
        if(HealthBar != null)
        HealthBar.value = currentHealth; 
    }


    //Health and damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("take damage");

        if(animator!= null)
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
        if(animator!= null)
        animator.SetTrigger("Death");   

        // Destroy(gameObject, 4f);
        // enemySpawner.enemies.Remove(gameObject);




        // enemySpawner.numAlive--;

        // Check if we need to spawn more enemies


    }
    void triaaaaaaaaaaal()
    {
        Debug.Log("Addforce");
        rb.AddForce(Vector3.up * forceMagnitude, ForceMode.Impulse);
    }
    void DeactivateGameObject()
    {

        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);

        }

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
