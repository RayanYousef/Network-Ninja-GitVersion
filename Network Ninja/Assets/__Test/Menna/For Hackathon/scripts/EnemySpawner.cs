using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Cinemachine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] bool canShowCard;
    Transform player;
    Canvas BossCanvas;

    [Header("Big Boss")]
    public GameObject BigBossPrefab;
    public GameObject cardPrefab;
    public GameObject IntroEffectPrefab;
    private PlayableDirector playableDirector;


    [Header("Enemies")]
    public GameObject enemyPrefab;
    public GameObject MiniBossPrefab;
    public Transform objectToSpawnAround;

    public int armySize = 30;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public int miniBossSize = 4;
    public float delayBeforeSpawnBoss = 3f; // Delay in seconds before spawning the boss



    public List<GameObject> enemies;  // A list of all spawned enemies.
    public List<GameObject> MiniBosses;  // A list of all spawned MonoBosses.
    public List<GameObject> enemyPool;

    public UnityEvent OnAllMiniBossesKilled, OnBigBossKilled;

    void Start()
    {
        enemies = new List<GameObject>();
        MiniBosses = new List<GameObject>();
        player = GameObjectsManager.Instance.Player.transform;

        OnAllMiniBossesKilled.AddListener(rUIManager.Instance.UiPassword.ShowCreatePasswordPanel);
        OnAllMiniBossesKilled.AddListener(delegate
        { rFaceExpressionManager.Instance.ChangeFacialExp(rFaceExpressionManager.FacialExp.Idle); });
        //obj pooling
        enemyPool = new List<GameObject>();

        for (int i = 0; i < armySize; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.transform.parent = this.transform;
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        playableDirector = BigBossPrefab.GetComponentInChildren<PlayableDirector>();


        //SpawnEnemies();
        //  enemyPrefab = GameObjectsManager.Instance.EnemyPrefab;
        // objectToSpawnAround = GameObjectsManager.Instance.ObjectToSpawnAround;

        // Subscribe to the OnEnemyKilled event for each enemy spawned
        //foreach (GameObject enemy in enemies)
        //{
        //    enemy.GetComponent<Health>().OnEnemyKilled +=HandleEnemyKilled;
        //}

    }


    #region  enemies before obj pooling
    //public void SpawnEnemies()
    //{
    //    for (int i = 0; i < armySize; i++)
    //    {
    //        Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
    //        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    //        enemy.transform.parent = this.transform;
    //       // enemy.GetComponent<Health>().OnEnemyKilled.AddListener(HandleEnemyKilled);
    //        enemies.Add(enemy);
    //    }

    //  //  StartCoroutine(AvoidEnemies());

    //}


    ////spawn enemies each interval of time
    //public void  SpawnEnemiesEachInterval()
    //{
    //    StartCoroutine("spawnMoreEnemies");
    //}

    //public IEnumerator spawnMoreEnemies()
    //{
    //    //condition when player and mini boss in area (player != null && miniboss != null) => while()
    //    while( MiniBosses.Count !=0)
    //    {
    //        SpawnEnemies();
    //        yield return (new WaitForSeconds(spawnInterval));
    //    }
    //    //for(int i = 0; i < 3; i++)
    //    //{
    //    //    SpawnEnemies();
    //    //    yield return (new WaitForSeconds(spawnInterval));
    //    //}
    //}

    ////spawn enemies each interval of time
    //public void SpawnEnemiesEachInterval()
    //{
    //    StartCoroutine("spawnMoreEnemies");
    //}

    //public IEnumerator spawnMoreEnemies()
    //{
    //    //condition when player and mini boss in area (player != null && miniboss != null) => while()
    //    while (MiniBosses.Count != 0)
    //    {
    //        GameObject enemy = GetEnemyFromPool();
    //        Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
    //        enemy.transform.position = randomPosition;
    //        enemy.SetActive(true);
    //        enemies.Add(enemy);
    //       // SpawnEnemies();
    //        yield return (new WaitForSeconds(spawnInterval));
    //    }
    //}
    #endregion

    // spawn MiniBosses
    #region old mini bosses
    //public void SpawnMiniBosses()
    //{
    //    for (int i = 0; i < miniBossSize; i++)
    //    {
    //        Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
    //        GameObject MiniBoss = Instantiate(MiniBossPrefab, randomPosition, Quaternion.identity);
    //        MiniBoss.transform.parent = this.transform;
    //        MiniBoss.GetComponent<m_MiniBoss>().OnMiniBossKilled += HandleMiniBossKilled;
    //        MiniBosses.Add(MiniBoss);
    //        //Debug.Log("mini boss spawned");
    //    }
    //}
    #endregion
    public void SpawnMiniBosses()
    {
        foreach (GameObject mb in MiniBosses)
        {
            mb.SetActive(true);
        }

        int cnt = MiniBosses.Count;

        for (int i = 0; i < miniBossSize - cnt; i++)
        {
            Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            GameObject MiniBoss = Instantiate(MiniBossPrefab, randomPosition, Quaternion.identity);
            MiniBoss.transform.parent = this.transform;
            MiniBoss.GetComponent<m_MiniBoss>().OnMiniBossKilled += HandleMiniBossKilled;
            MiniBosses.Add(MiniBoss);
            //Debug.Log("mini boss spawned");
        }
    }
    public void SpawnBigBoss()
    {
        GameObject BigBoss = Instantiate(BigBossPrefab, this.transform.position, Quaternion.identity);
        BigBoss.transform.parent = this.transform;

    }

    public void spawnBossIntroEffect()
    {
        GameObject introEffect = Instantiate(IntroEffectPrefab, this.transform.position, Quaternion.identity);
        introEffect.transform.parent = this.transform;

    }

    public void SpawnCard()
    {
        GameObject BigBoss = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
        //BigBoss.transform.parent = this.transform;
    }


    #region
    public void SpawnEnemies()
    {
      //  int enemiesToSpawn = Mathf.Min(enemiesPerSpawn, armySize - numAlive);

        for (int i = 0; i < armySize; i++)
        {
            
            GameObject enemy = GetEnemyFromPool();
            if (enemy != null)
            {
                //Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
                //enemy.transform.position = randomPosition;
                //enemy.transform.parent = this.transform;
                enemy.SetActive(true);
                enemies.Add(enemy);
            }


        }
    }

    

    #region obj pooling for miniboss
    //public void SpawnMiniBosses()
    //{
    //    for (int i = 0; i < miniBossSize; i++)
    //    {
    //        GameObject miniboss = GetMiniBossFromPool();
    //        Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
    //        miniboss.transform.position = randomPosition;
    //        miniboss.SetActive(true);
    //        miniboss.GetComponent<m_MiniBoss>().OnMiniBossKilled += HandleMiniBossKilled;
    //        minibossPool.Add(miniboss);
    //        MiniBosses.Add(miniboss);
    //    }
    //}

    //private GameObject GetMiniBossFromPool()
    //{
    //    foreach (GameObject miniboss in minibossPool)
    //    {
    //        if (!miniboss.activeInHierarchy)
    //        {
    //            return miniboss;
    //        }
    //    }
    //    GameObject newMiniBoss = Instantiate(MiniBossPrefab, Vector3.zero, Quaternion.identity);
    //    newMiniBoss.SetActive(false);
    //    newMiniBoss.GetComponent<m_MiniBoss>().OnMiniBossKilled += HandleMiniBossKilled;
    //    minibossPool.Add(newMiniBoss);
    //    return newMiniBoss;
    //}
    #endregion
    public GameObject GetEnemyFromPool()
    {
       if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool[0];
            enemyPool.RemoveAt(0);
            return enemy;
        }
        else
        {
            return null;
        }
    }

    public void AddEnemiesInPool()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemyPool.Add(enemy);
                enemy.SetActive(false);
            }
        }
        enemies.Clear();
    }
    public void SpawnMoreEnemies()
    {
        //for (int i = 0; i < enemyPool.Count ; i++)
        //{
            GameObject enemy = GetEnemyFromPool();

        if (enemy != null)
            {
                Vector3 randomPosition = objectToSpawnAround.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
                enemy.transform.position = randomPosition;
                enemy.transform.parent = this.transform;
                enemy.SetActive(true);
            // enemy.AddComponent<CapsuleCollider>();
            enemies.Add(enemy);
           
            }

       // }
    }

    public void DisableMiniBosses()
    {
        foreach (GameObject MiniBoss in MiniBosses)
        {
            if(MiniBoss != null)
            {
                //Destroy(MiniBoss);
                MiniBoss.gameObject.SetActive(false);

            }
        }
    }
    #endregion

    //invoke event when all MiniBosses died 
    void HandleMiniBossKilled(GameObject MiniBoss)
    {
       
        MiniBosses.Remove(MiniBoss);
        if (MiniBosses.Count == 0 && OnAllMiniBossesKilled != null)
        {
            GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().UltimateDisabled(false);
            foreach (GameObject Enemy in enemies)
            {
                if (Enemy != null)
                {
                    Enemy.GetComponent<m_EnemyManager>().Die();
                    Debug.Log("All DEAAAAAAAAAAAAAAAAAAAAAAAD");

                }
                // MiniBossPrefab.GetComponent<Animator>().SetTrigger("Death");
                // Destroy(m_EnemyManager);
            }
            enemyPool.Clear();
            enemies.Clear();

            bool allAreasBase = rAreasManager.Instance.CheckAllAreasBaseExceptCurrent();

            if(allAreasBase)
            {
                /// spawn big boss
                /// in case of emergency... invoke winning event here
                // GameManager.Instance.EndStage(true);
                GameManager.Instance.BossEntered = true;
                if(canShowCard)
                {
                    StartCoroutine(SpawnCardCoroutine());
                }
                else
                {
                    StartCoroutine(SpawnBossCoroutine());
                }
            }
            else
            {
                // rUIPassword listens to this event: OnEnteringAreaShowPannels()
                OnAllMiniBossesKilled?.Invoke();
            }  
        }
    }

    public IEnumerator SpawnBossCoroutine()
    {
        player.GetComponent<CS_PlayerManager>().ControllerState(false);
        player.transform.position = transform.position + new Vector3 (0f,0f,15.0f);
        yield return new WaitForSeconds(delayBeforeSpawnBoss / 2);
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.5f;
        spawnBossIntroEffect();
        yield return new WaitForSeconds(3.5f);
        // Spawn the boss
        SpawnBigBoss();


    }

    IEnumerator SpawnCardCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeSpawnBoss);

        // Spawn the card
        SpawnCard();

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



