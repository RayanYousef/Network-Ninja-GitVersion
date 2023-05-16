using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameObjectsManager : MonoBehaviour
{

    private static GameObjectsManager instance;
    //

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] CinemachineVirtualCamera playerCamera;

    [Header("Ally Prefab")]
    [SerializeField] FormationAgent allyPrefab;
    [SerializeField] Formation allyBatalionPrefab;
    [SerializeField] GameObject spawnEffect;

    [Header("Boss")]
    [SerializeField] m_CombatManager combatManager;
    [SerializeField] m_BossUI_Manager bossUiManager;

    [Header("Level Objects")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform pointInArea1;
    [SerializeField] Transform pointInArea2;
    [SerializeField] Transform pointInArea3;
    //  [SerializeField] Transform[] wayPoints;

    [Header("Boss")]
    [SerializeField] m_CombatManager combatManager;
    [SerializeField] m_BossUI_Manager bossUiManager;



    [SerializeField] Transform pointInArea1;
    [SerializeField] Transform pointInArea2;
    [SerializeField] Transform pointInArea3;

    public static GameObjectsManager Instance { get => instance; }
    public GameObject EnemyPrefab { get => enemyPrefab; }
    public Transform PointInArea1 { get => pointInArea1; }
    public Transform PointInArea2 { get => pointInArea2; }
    public Transform PointInArea3 { get => pointInArea3; }

   // public Transform[] WayPoints { get => wayPoints; }
    public GameObject Player { get => player; }
    public CinemachineVirtualCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public FormationAgent AllyPrefab { get => allyPrefab; }
    public GameObject SpawnEffect { get => spawnEffect; set => spawnEffect = value; }
    public Formation AllyBatalionPrefab { get => allyBatalionPrefab; set => allyBatalionPrefab = value; }
    public m_BossUI_Manager BossUiManager { get => bossUiManager; }
    public m_CombatManager CombatManager{ get => combatManager; }

    public m_BossUI_Manager BossUiManager { get => bossUiManager; }
    public m_CombatManager CombatManager { get => combatManager; }
    public Transform PointInArea1 { get => pointInArea1; }
    public Transform PointInArea2 { get => pointInArea2; }
    public Transform PointInArea3 { get => pointInArea3; }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
