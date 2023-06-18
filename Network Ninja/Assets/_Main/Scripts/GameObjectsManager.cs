using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameObjectsManager : MonoBehaviour
{

    private static GameObjectsManager instance;


    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] public CinemachineBrain cameraBrain;
    [SerializeField] CinemachineVirtualCamera playerCamera;

    [Header("Ally Prefab")]
    [SerializeField] FormationAgent allyPrefab;
    [SerializeField] Formation allyBatalionPrefab;
    [SerializeField] GameObject spawnEffect;

    [Header("Boss")]
    [SerializeField] GameObject boss;
    [SerializeField] bool duringCutScene;

    [Header("Current Gate")]
    [SerializeField] GateController currentGate;

    public static GameObjectsManager Instance { get => instance; }


    public GameObject Player { get => player; set => player = value; }
    public GameObject Boss { get => boss; }
    public CinemachineVirtualCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public FormationAgent AllyPrefab { get => allyPrefab; }
    public GameObject SpawnEffect { get => spawnEffect; set => spawnEffect = value; }
    public Formation AllyBatalionPrefab { get => allyBatalionPrefab; set => allyBatalionPrefab = value; }
    public CinemachineBrain CameraBrain { get => cameraBrain; set => cameraBrain = value; }
    public GateController CurrentGate { get => currentGate; set => currentGate = value; }
    public bool DuringCutScene { get => duringCutScene; set => duringCutScene = value; }


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

    
}
