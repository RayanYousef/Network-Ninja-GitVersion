using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class m_EnemyManager : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private EnemySpawner enemySpawner;
    private Collider collider;
    private float currentHealth;
    private float maxHealth;


    public GameObject DeathEffect;
    public Animator animator;






    #region //variables for overlap
    ////overlap
    //public float avoidanceRadius = 1f;
    //public float avoidanceForce = 1f;
    //public LayerMask overlapLayer;

    //private Collider[] overlappingColliders;

    #endregion

    // Start is called before the first frame update
   public  void Start()
    {
        player = GameObjectsManager.Instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        currentHealth = GetComponent<StatsManager>().Stats.CurrentHealth;
        maxHealth = GetComponent<StatsManager>().Stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    public void showHealth()
    {
        Debug.Log(GetComponent<StatsManager>().Stats.CurrentHealth + gameObject.name);
    }

    public virtual void OnHealthUpdatedFunction()
    {
        if (GetComponent<StatsManager>().Stats.CurrentHealth == 0)
            Die();
    }

    public virtual void Die()
    {
        //animation
        Debug.Log("When enemy died");
        Destroy(collider);
        if (animator != null)
            animator.SetTrigger("Death");
    }

    public virtual void DeactivateGameObject()
    {

        if (DeathEffect != null)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);

        }

        enemySpawner = GetComponentInParent<EnemySpawner>();
        gameObject.SetActive(false);
        //this.currentHealth = maxHealth;
        enemySpawner.enemyPool.Add(gameObject);
        enemySpawner.enemies.Remove(gameObject);

        if (enemySpawner.MiniBosses.Count > 0)
        {
            enemySpawner.SpawnMoreEnemies();
        }

    }

}
