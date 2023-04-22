using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsManager : MonoBehaviour
{

    private static GameObjectsManager instance;
    //

    [Header("Player")]
    [SerializeField] GameObject player;
    [Header("Army Prefabs")]
    [SerializeField] FriendStates meleePrefab;
    [SerializeField] FriendStates rangedPrefab;
    [SerializeField] FriendStates tankPrefab;

    [Header("Level Objects")]
    [SerializeField] NPCStateMachine enemyPrefab;
    [SerializeField] Transform objectToSpawnAround;
    [SerializeField] Transform[] wayPoints;



    public static GameObjectsManager Instance { get => instance; }
    public NPCStateMachine EnemyPrefab { get => enemyPrefab; }
    public Transform ObjectToSpawnAround { get => objectToSpawnAround; }
    public Transform[] WayPoints { get => wayPoints; }
    public GameObject Player { get => player; }
    public FriendStates MeleePrefab { get => meleePrefab; }
    public FriendStates RangedPrefab { get => rangedPrefab;  }
    public FriendStates TankPrefab { get => tankPrefab;}

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
