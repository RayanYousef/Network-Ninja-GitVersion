using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameObjectsManager : MonoBehaviour
{

    private static GameObjectsManager instance;
    

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
    [SerializeField] GameObject boss;



    [Header("Level Objects")]
    [SerializeField] rArea[] listOfLevelAreas;


    public static GameObjectsManager Instance { get => instance; }


    public GameObject Player { get => player; }
    public GameObject Boss { get => boss; }
    public CinemachineVirtualCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public FormationAgent AllyPrefab { get => allyPrefab; }
    public GameObject SpawnEffect { get => spawnEffect; set => spawnEffect = value; }
    public Formation AllyBatalionPrefab { get => allyBatalionPrefab; set => allyBatalionPrefab = value; }
    public m_BossUI_Manager BossUiManager { get => bossUiManager; }
    public m_CombatManager CombatManager { get => combatManager; }
 
    public rArea[] ListOfLevelAreas { get => listOfLevelAreas; set => listOfLevelAreas = value; }

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

    public bool CheckAllAreasBaseExceptCurrent()
    {
        foreach(rArea area in listOfLevelAreas)
        {
            if(area == rPasswordManager.Instance.CurrentArea)
            {
                continue;
            }
            if(area.AreaType != AreaType.Base)
            {
                return false;
            }
        }
        return true;
    }
}
