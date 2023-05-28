using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class m_MiniBoss : m_EnemyManager
{
    public Action<GameObject> OnMiniBossKilled;  // Declare the event


    public override void OnHealthUpdatedFunction()
    {
        if (GetComponent<StatsManager>().Stats.CurrentHealth == 0)
            Die();
    }
    public override void Die()
    {


            //animation
            if (animator != null)
                animator.SetTrigger("Death");
            Destroy(GetComponent<Collider>());
            // Raise the event when the enemy is killed
            OnMiniBossKilled.Invoke(this.gameObject);
       

    }

    public override void DeactivateGameObject()
    {
        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
        }

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

  
}
