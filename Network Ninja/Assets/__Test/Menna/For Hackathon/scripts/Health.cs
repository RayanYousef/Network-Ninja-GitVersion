using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Collider collider;


    void Start()
    {
        animator = GetComponent<Animator>();
       
        currentHealth = maxHealth;
        if(HP== null)
        HP = GetComponentInChildren<Slider>();
        HP.maxValue = maxHealth;
        rb = GetComponent<Rigidbody>();
        //StartCoroutine(RandomlyApplyDamage());
        collider = GetComponent<Collider>();
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
        HealthBar.value = currentHealth ; 
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
        Destroy(collider);
        if(animator!= null)
        animator.SetTrigger("Death");   
    }
    void triaaaaaaaaaaal()
    {
        Debug.Log("Addforce");
        rb.AddForce(Vector3.up * forceMagnitude, ForceMode.Impulse);
    }
    public virtual void DeactivateGameObject()
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
