using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    Transform player;

    public GameObject enemyPrefab;
    public GameObject MiniBossPrefab;
    public Transform objectToSpawnAround;

    public int armySize = 10;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public bool allArmyDied = false;
    public int spawnInterval = 20;
    public int miniBossSize = 4;
   // public float avoidanceDistance = 2f;  // The distance at which enemies will avoid each other.


    public List<GameObject> enemies;  // A list of all spawned enemies.
    public List<GameObject> MiniBosses;  // A list of all spawned MonoBosses.

    public UnityEvent OnAllMiniBossesKilled;


    void Start()
    {
        enemies = new List<GameObject>();
        MiniBosses = new List<GameObject>();
        player = GameObjectsManager.Instance.Player.transform;

        //SpawnEnemies();
        //  enemyPrefab = GameObjectsManager.Instance.EnemyPrefab;
        // objectToSpawnAround = GameObjectsManager.Instance.ObjectToSpawnAround;

        // Subscribe to the OnEnemyKilled event for each enemy spawned
        //foreach (GameObject enemy in enemies)
        //{
        //    enemy.GetComponent<Health>().OnEnemyKilled +=HandleEnemyKilled;
        //}

    }
    public void SpawnEnemies()
    {
        for (int i = 0; i < armySize; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.transform.parent = this.transform;
           // enemy.GetComponent<Health>().OnEnemyKilled.AddListener(HandleEnemyKilled);
            enemies.Add(enemy);
        }

      //  StartCoroutine(AvoidEnemies());


    }

    //spawn enemies each interval of time
    public void  SpawnEnemiesEachInterval()
    {
        StartCoroutine("spawnMoreEnemies");
    }

    public IEnumerator spawnMoreEnemies()
    {
        //condition when player and mini boss in area (player != null && miniboss != null) => while()
        while( MiniBosses.Count !=0)
        {
            SpawnEnemies();
            yield return (new WaitForSeconds(spawnInterval));
        }
        //for(int i = 0; i < 3; i++)
        //{
        //    SpawnEnemies();
        //    yield return (new WaitForSeconds(spawnInterval));
        //}
    }



    // spawn MiniBosses
    public void SpawnMiniBosses()
    {
        for (int i = 0; i < miniBossSize; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            GameObject MiniBoss = Instantiate(MiniBossPrefab, randomPosition, Quaternion.identity);
            MiniBoss.transform.parent = this.transform;
            MiniBoss.GetComponent<m_MiniBossHealth>().OnMiniBossKilled += HandleMiniBossKilled;
            MiniBosses.Add(MiniBoss);
           //Debug.Log("mini boss spawned");

        }
    }


    //invoke event when all MiniBosses died 
    void HandleMiniBossKilled(GameObject MiniBoss)
    {
        MiniBosses.Remove(MiniBoss);
        if (MiniBosses.Count == 0 && OnAllMiniBossesKilled != null)
        {
            Debug.Log("All DEAAAAAAAAAAAAAAAAAAAAAAAD");
            foreach (GameObject Enemy in enemies)
            {
                if(Enemy != null)
                {
                    Enemy.GetComponent<Health>().Die();

                }
                // MiniBossPrefab.GetComponent<Animator>().SetTrigger("Death");
                // Destroy(Enemy);
            }
            enemies.Clear();
            OnAllMiniBossesKilled.Invoke();
        }
    }
    #region //trials
    //IEnumerator AvoidEnemies()
    //{
    //    while (true)
    //    {
    //        foreach (GameObject enemy in enemies)
    //        {
    //            Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, avoidanceDistance);

    //            Vector3 avoidDirection = Vector3.zero;
    //            int numEnemies = 0;

    //            foreach (Collider hitCollider in hitColliders)
    //            {
    //                if (hitCollider.gameObject != enemy)
    //                {
    //                    avoidDirection += enemy.transform.position - hitCollider.transform.position;
    //                    numEnemies++;
    //                }
    //            }

    //            if (numEnemies > 0)
    //            {
    //                avoidDirection /= numEnemies;
    //                avoidDirection.Normalize();
    //                enemy.transform.position += avoidDirection * avoidanceDistance * Time.deltaTime;
    //            }
    //        }

    //        yield return null;
    //    }
    //}



    //void HandleEnemyKilled()
    //{
    //    // Check if all enemies have been killed
    //    bool allEnemiesKilled = true;
    //    foreach (GameObject enemy in enemies)
    //    {
    //        if (enemy != null)
    //        {
    //            allEnemiesKilled = false;
    //            break;
    //        }
    //    }

    //    // If all enemies are killed, raise the OnAllEnemiesKilled event
    //    if (allEnemiesKilled)
    //    {
    //        OnAllEnemiesKilled.Invoke();
    //    }

    //    allArmyDied = allEnemiesKilled;
    //}
    #endregion
}



