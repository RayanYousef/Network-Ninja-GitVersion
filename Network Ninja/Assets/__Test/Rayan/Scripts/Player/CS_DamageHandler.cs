using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public enum ProjectOnPlaneAxis { forward, right };

public class CS_DamageHandler : MonoBehaviour
{
    [Header("List Of Hit Objects (can't take damage again)")]
    [SerializeField] List<Collider> hitObjects = new List<Collider>();

    [Header("Player and Weapon")]
    [SerializeField] Transform weaponTip;
    [SerializeField] Vector3 firstPoint, secondPoint;

    [Header("Variables")]
    [SerializeField] float speed;


    public void SetFirstPoint()
    {
        firstPoint = weaponTip.transform.position;
    }

    public void ProjectOnAxis(Vector3 axis, Vector3 playerForward, Vector3 position)
    {
        secondPoint = weaponTip.transform.position;
        //RotateTowardDirection();
        CalculateRotation(axis);

        transform.position = position;
        GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        GetComponentInChildren<Rigidbody>().AddForce(playerForward * speed, ForceMode.VelocityChange);
    }

    //public void RotateTowardDirection()
    //{
    //    transform.position = firstPoint;
    //    transform.LookAt(secondPoint);
    //    firstPoint = Vector3.ProjectOnPlane(transform.position, player.forward);
    //    secondPoint = Vector3.ProjectOnPlane(transform.GetChild(0).transform.position, player.forward);
    //    Vector3 direction = firstPoint - secondPoint;
    //    transform.rotation = Quaternion.LookRotation(direction);
    //}

    private void CalculateRotation(Vector3 value)
    {
        Vector3 direction = secondPoint - firstPoint;
        Vector3 projectedDirection = Vector3.ProjectOnPlane(direction, value);
        transform.rotation = Quaternion.LookRotation(projectedDirection);

    }



    #region HERE WE NEED EVERYTHING HERE

    public StatController stats;

    private void OnEnable()
    {
        hitObjects.Clear();
        stats.HitObjects.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!hitObjects.Contains(other) && other.TryGetComponent<Health>(out Health stats))
        {
            hitObjects.Add(other);
            if (stats != null && stats.currentHealth > 0)
            {
                stats.TakeDamage(35);
                other.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
            }

        }
    } 
    #endregion
}
