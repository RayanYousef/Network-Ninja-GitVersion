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

    [Header("Level Objects")]
    //[SerializeField] Transform[] wayPoints;
    [SerializeField] rArea[] listOfLevelAreas;

    public static GameObjectsManager Instance { get => instance; }

   // public Transform[] WayPoints { get => wayPoints; }
    public GameObject Player { get => player; }
    public CinemachineVirtualCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }
    public FormationAgent AllyPrefab { get => allyPrefab; }
    public GameObject SpawnEffect { get => spawnEffect; set => spawnEffect = value; }
    public Formation AllyBatalionPrefab { get => allyBatalionPrefab; set => allyBatalionPrefab = value; }
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
            if (area.AreaType != AreaType.Base)
            {
                return false;
            }
        }
        return true;
    }
}
