using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_MiniBossHealth : Health
{
    public Action<GameObject> OnMiniBossKilled;  // Declare the event

    public override void Die()
    {
        //animation
        if (animator != null)
        animator.SetTrigger("Death");
        Destroy(collider);
        // Raise the event when the enemy is killed
        OnMiniBossKilled.Invoke(this.gameObject);
    }

    public override void DeactivateGameObject()
    {
        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);

        }
        gameObject.SetActive(false);
       // Destroy(gameObject);

    }
}
