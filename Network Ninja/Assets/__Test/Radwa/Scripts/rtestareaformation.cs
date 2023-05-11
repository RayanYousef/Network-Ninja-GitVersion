using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class rtestareaformation : MonoBehaviour
{
    [SerializeField] private Transform[] alliesSpawnPos;
    [SerializeField] private Formation alliesSpawnerPrefab;

    [SerializeField] private List<FormationAgent> alliesList = new List<FormationAgent>();

    Formation allies1, allies2, allies3;

    private void Start()
    {
        allies1 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[0].position, Quaternion.identity);
        allies1.transform.parent = alliesSpawnPos[0];

        allies2 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[1].position, Quaternion.identity);
        allies2.transform.parent = alliesSpawnPos[1];

        allies3 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[2].position, Quaternion.identity);
        allies3.transform.parent = alliesSpawnPos[2];

        alliesList = allies1.AgentsList.Concat(allies2.AgentsList.Concat(allies3.AgentsList)).ToList();
    }
}
