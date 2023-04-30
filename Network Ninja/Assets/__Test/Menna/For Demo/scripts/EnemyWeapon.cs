using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int attackDamage = 20;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * attackOffset.z;
        pos += transform.up * attackOffset.y;
        pos += transform.right * attackOffset.x;

        Collider[] hitColliders = Physics.OverlapSphere(pos, attackRange, attackMask);
        foreach (Collider col in hitColliders)
        {
            col.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }

   
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * attackOffset.z;
        pos += transform.up * attackOffset.y;
        pos += transform.right * attackOffset.x;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
