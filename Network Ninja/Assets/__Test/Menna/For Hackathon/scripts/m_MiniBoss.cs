using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_MiniBoss : m_EnemyManager
{
    public Action<GameObject> OnMiniBossKilled;  // Declare the event

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

        Destroy(gameObject);
    }
}
