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
        animator.SetTrigger("Death");

        // Raise the event when the enemy is killed
        OnMiniBossKilled.Invoke(this.gameObject);

        Destroy(gameObject, 1.5f);

    }

}
