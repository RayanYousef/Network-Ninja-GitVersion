using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class CS_DamageObject : MonoBehaviour
{
    [Header("My Stats")]
    [SerializeField] StatsManager myStatsManager;

    [Header("Tag")]
    [SerializeField] string weaponName;

    public StatsManager MyStatsManager { get => myStatsManager; set { 
            if(myStatsManager==null) myStatsManager = value; } }

    public string WeaponName { get => weaponName; }


    #region Logic
    private void OnEnable()
    {
        if(myStatsManager!=null)
        myStatsManager.HitObjects.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (myStatsManager != null && !myStatsManager.HitObjects.Contains(other) && other.TryGetComponent<StatsManager>(out StatsManager otherStatsManager))
        {
            myStatsManager.HitObjects.Add(other);
            if (otherStatsManager.Stats.CurrentHealth > 0 && otherStatsManager != myStatsManager)
            {
                if (otherStatsManager.Team != myStatsManager.Team)
                {
                    otherStatsManager.ApplyDamage(myStatsManager);
                    if (other!= myStatsManager.gameObject)
                        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        other.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 2;
                }
            }

        }

    } 
    #endregion
}
