using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public int maxHealth = 100;
   public int currentHealth;
    Animator animator;
    public Slider HealthBar;


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

    public void Die()
    {
        animator.SetTrigger("Death");

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("playerhealth");
    //        PlayerHealth PH = other.gameObject.GetComponent<PlayerHealth>();
    //        PH.TakeDamage(25);
    //    }
    //}
}
