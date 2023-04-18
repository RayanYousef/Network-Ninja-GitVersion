using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyStates enemyPrefab;
    private Transform objectToSpawnAround;
    
    public int armySize = 10;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public float avoidanceDistance = 2f;  // The distance at which enemies will avoid each other.


    public List<EnemyStates> enemies;  // A list of all spawned enemies.

    void Start()
    {
        enemies = new List<EnemyStates>();
        enemyPrefab = GameObjectsManager.Instance.EnemyPrefab;
        objectToSpawnAround = GameObjectsManager.Instance.ObjectToSpawnAround;

        SpawnEnemies(armySize);

        //enemyPrefab.gameObject.SetActive(false);


    }
    void SpawnEnemies(int armySize)
    {
        for (int i = 0; i < armySize; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            EnemyStates enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemies.Add(enemy);
        }


    }
}



