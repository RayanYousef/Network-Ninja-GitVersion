using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]
public class CS_DamageObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] StatsManager myStatsManager;
    [SerializeField] AudioSource audioSource;

    [Header("Variables")]
    [SerializeField] float skillMultiplier=1;
    [SerializeField] string weaponName;

    public StatsManager MyStatsManager { get => myStatsManager; set { 
            if(myStatsManager==null) myStatsManager = value; } }

    public string WeaponName { get => weaponName; }
    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        audioSource.playOnAwake= false;
        audioSource.loop= false;
    }

    #region Logic
    private void OnEnable()
    {
        if(myStatsManager!=null)
        myStatsManager.HitObjects.Clear();

        if (audioSource != null && audioSource.clip!=null)
            audioSource.Play();
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
                    //Apply Hitstop
                    if (otherStatsManager.TryGetComponent<HitStopHandler>(out HitStopHandler handler))
                        handler.GetComponent<HitStopHandler>().AnimationStop(0.5f, 0f);


                    otherStatsManager.ApplyDamage(myStatsManager);
                    if (other!= myStatsManager.gameObject)
                        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                        other.GetComponent<Rigidbody>().velocity = myStatsManager.GetComponent<Rigidbody>().velocity * 2;
                }
            }

        }

    } 
    #endregion
}
