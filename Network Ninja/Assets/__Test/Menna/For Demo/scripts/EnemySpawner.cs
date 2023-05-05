using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform objectToSpawnAround;

    public int armySize = 15;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public bool allArmyDied = false;
    public int spawnInterval = 5;
   // public float avoidanceDistance = 2f;  // The distance at which enemies will avoid each other.


    public List<GameObject> enemies;  // A list of all spawned enemies.

    public UnityEvent OnAllEnemiesKilled;


    void Start()
    {
        enemies = new List<GameObject>();

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
            enemy.GetComponent<Health>().OnEnemyKilled += HandleEnemyKilled;
            //enemy.GetComponent<Health>().OnEnemyKilled.AddListener(HandleEnemyKilled);
            enemies.Add(enemy);
        }
      //  StartCoroutine(AvoidEnemies());


    }

    public void  SpawnEnemiesEachInterval()
    {
        StartCoroutine("spawnMoreEnemies");
    }
    public IEnumerator spawnMoreEnemies()
    {
        //condition when player and mini boss in area (player != null && miniboss != null)
        while (true)
        {
            SpawnEnemies();
            yield return (new WaitForSeconds(spawnInterval));
        }
    }

    void HandleEnemyKilled(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0 && OnAllEnemiesKilled != null)
        {
            OnAllEnemiesKilled.Invoke();
            Debug.Log("All DEAAAAAAAAAAAAAAAAAAAAAAAD");
        }
    }

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

}



