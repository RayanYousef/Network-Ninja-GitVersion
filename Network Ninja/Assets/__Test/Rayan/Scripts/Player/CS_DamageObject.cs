using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public enum ProjectOnPlaneAxis { forward, right };

public class CS_DamageObject : MonoBehaviour
{
    [Header("Charater Stats")]
    [SerializeField] StatsManager myStatsManager;
    //[Header("Player and Weapon")]
    //[SerializeField] Transform weaponTip;
    //[SerializeField] Vector3 firstPoint, secondPoint;

    //[Header("Variables")]
    //[SerializeField] float speed;

    public StatsManager MyStatsManager { get => myStatsManager; set { 
            if(myStatsManager==null) myStatsManager = value; } }

    //public void SetFirstPoint()
    //{
    //    firstPoint = weaponTip.transform.position;
    //}

    //public void ProjectOnAxis(Vector3 axis, Vector3 playerForward, Vector3 position)
    //{
    //    secondPoint = weaponTip.transform.position;
    //    //RotateTowardDirection();
    //    CalculateRotation(axis);

    //    transform.position = position;
    //    GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
    //    GetComponentInChildren<Rigidbody>().AddForce(playerForward * speed, ForceMode.VelocityChange);
    //}

    ////public void RotateTowardDirection()
    ////{
    ////    transform.position = firstPoint;
    ////    transform.LookAt(secondPoint);
    ////    firstPoint = Vector3.ProjectOnPlane(transform.position, player.forward);
    ////    secondPoint = Vector3.ProjectOnPlane(transform.GetChild(0).transform.position, player.forward);
    ////    Vector3 direction = firstPoint - secondPoint;
    ////    transform.rotation = Quaternion.LookRotation(direction);
    ////}

    //private void CalculateRotation(Vector3 value)
    //{
    //    Vector3 direction = secondPoint - firstPoint;
    //    Vector3 projectedDirection = Vector3.ProjectOnPlane(direction, value);
    //    transform.rotation = Quaternion.LookRotation(projectedDirection);

    //}



    #region HERE WE NEED EVERYTHING HERE
    private void OnEnable()
    {
        if(myStatsManager!=null)
        myStatsManager.HitObjects.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (myStatsManager != null && !myStatsManager.HitObjects.Contains(other) && other.TryGetComponent<StatsManager>(out StatsManager otherStatsManager))
        //{
        //    myStatsManager.HitObjects.Add(other);
        //    if (otherStatsManager.Stats.CurrentHealth > 0 && otherStatsManager!= myStatsManager)
        //    {
        //        if(otherStatsManager.Team != myStatsManager.Team) { }
        //        otherStatsManager.ApplyDamage(35);
        //        //other.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
        //    }

        //}

        if (myStatsManager != null && !myStatsManager.HitObjects.Contains(other) && other.TryGetComponent<Health>(out Health otherHealth) && other.TryGetComponent<StatsManager>(out StatsManager otherStatsManager))
        {
            myStatsManager.HitObjects.Add(other);
            if (otherHealth.currentHealth >0  && otherStatsManager.Team != myStatsManager.Team)
            {

                if (otherStatsManager.Team == CharacterTeam.Enemy)
                {
                    other.GetComponent<Rigidbody>().velocity = myStatsManager.GetComponent<Rigidbody>().velocity * 2;
                    otherHealth.TakeDamage(100);
                }
                else
                    otherHealth.TakeDamage(5);

            }

        }
    } 
    #endregion
}
