using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform objectToSpawnAround;

    public int armySize = 10;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public float avoidanceDistance = 2f;  // The distance at which enemies will avoid each other.


    public List<GameObject> enemies;  // A list of all spawned enemies.

    public UnityEvent OnAllEnemiesKilled;


    void Start()
    {
        enemies = new List<GameObject>();
        SpawnEnemies(armySize);
      //  enemyPrefab = GameObjectsManager.Instance.EnemyPrefab;
       // objectToSpawnAround = GameObjectsManager.Instance.ObjectToSpawnAround;


    }
    void SpawnEnemies(int armySize)
    {
        for (int i = 0; i < armySize; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.GetComponent<Health>().OnEnemyKilled.AddListener(HandleEnemyKilled);
            enemies.Add(enemy);
        }


    }

    void HandleEnemyKilled(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0 && OnAllEnemiesKilled != null)
        {
            OnAllEnemiesKilled.Invoke();
        }
    }
}



