using System;

using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class CS_DamageObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] StatsManager myStatsManager;
    [SerializeField] CS_PlayerManager playerManager;

    [Header("SFX")]
    [SerializeField] AudioClip SFXClip;
    [SerializeField, Range(-3, 3)] float pitch = 1;
    [SerializeField, Range(0, 1)] float volume = 1;

    [Header("Events")]
    [SerializeField] Action<Collider> OnHittingEnemy;

    [Header("Variables")]
    [SerializeField] float skillMultiplier = 1;
    [SerializeField] string weaponName;

    public StatsManager MyStatsManager
    {
        get => myStatsManager;
        set
        {
            if (myStatsManager == null) myStatsManager = value;

            myStatsManager.TryGetComponent<CS_PlayerManager>(out playerManager);
        }
    }

    public string WeaponName { get => weaponName; }
    private void Start()
    {
        if(playerManager!= null)
        {
            OnHittingEnemy += OnHitStopObject;
            OnHittingEnemy += ApplyForceToHitObject;
            OnHittingEnemy += EnableHitParticles;
            OnHittingEnemy += playerManager.RecoverEnergy;
            OnHittingEnemy += myStatsManager.ApplyDamage;

        }
    }

    #region Logic

    #region On Enable Functions
    private void OnEnable()
    {
        if (myStatsManager != null)
            myStatsManager.HitObjects.Clear();
        ApplySFX();
    }
    private void OnDisable()
    {
        if (myStatsManager != null)
            myStatsManager.HitObjects.Clear();
    }
    private void ApplySFX()
    {
        if (SFXClip != null && AudioManager.instance != null)
            foreach (var audioSource in AudioManager.instance.audioSources)
            {
                if (audioSource.isPlaying == false)
                {
                    audioSource.volume = volume;
                    audioSource.clip = SFXClip;
                    audioSource.pitch = pitch;
                    audioSource.Play();
                    return;
                }

            }
    }

    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (myStatsManager != null && !myStatsManager.HitObjects.Contains(other) && other.TryGetComponent<StatsManager>(out StatsManager otherStatsManager))
        {

            myStatsManager.HitObjects.Add(other);
            if (otherStatsManager.Stats.CurrentHealth > 0 && otherStatsManager != myStatsManager)
            {
                if (otherStatsManager.Team != myStatsManager.Team)
                {
                    // Apply Damage to enemy.
                    otherStatsManager.TakeDamage(myStatsManager);

                    OnHittingEnemy?.Invoke(other);
                }
            }

        }

    }

    #region On Hit Functions
    private void OnHitStopObject(Collider other)
    {
        if (other.TryGetComponent(out StatsManager otherStatsManager))
            if (otherStatsManager.TryGetComponent<HitStopHandler>(out HitStopHandler handler))
                handler.GetComponent<HitStopHandler>().AnimationStop(0.5f, 0f);

    }
    private void ApplyForceToHitObject(Collider other)
    {
        // Apply Force to enemies on hit.
        if (other != myStatsManager.gameObject)
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                other.GetComponent<Rigidbody>().velocity = myStatsManager.GetComponent<Rigidbody>().velocity * 2;

    }
    private void EnableHitParticles(Collider other)
    {
        // Apply Particle effect when hitting enemy
        if (playerManager != null)
            foreach (var particle in playerManager.HitEffects)
            {
                if (particle.gameObject.activeInHierarchy == false)
                {
                    particle.transform.position = other.bounds.center;
                    particle.gameObject.SetActive(true);
                    return;
                }

            }
    }
    #endregion


    #endregion
}
