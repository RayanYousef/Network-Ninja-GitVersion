using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_MiniBossHealth : Health
{
    public Action<GameObject> OnEnemyKilled;  // Declare the event

    public override void Die()
    {
        //animation
        animator.SetTrigger("Death");
        Destroy(gameObject, 1.5f);

    }

}
