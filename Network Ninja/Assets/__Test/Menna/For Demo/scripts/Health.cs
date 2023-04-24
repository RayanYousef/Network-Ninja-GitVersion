using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
   public int currentHealth;
    Animator animator;
    public Slider HealthBar;

    public UnityEvent OnEnemyKilled;  // Declare the event



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died");

            Die();
        }
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

    public void Die()
    {
        // Raise the event when the enemy is killed
        OnEnemyKilled.Invoke();
        //animation
        animator.SetTrigger("Death");
        Destroy(gameObject);

    }

}
