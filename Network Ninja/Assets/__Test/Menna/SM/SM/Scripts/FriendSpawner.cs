using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSpawner : MonoBehaviour
{
     FriendStates meleePrefab;
     FriendStates rangedPrefab;
     FriendStates tankPrefab;

    float meleeChance;
    float rangedChance;
    float tankChance;
    float chance;

    float min, mid, max;

    //public int armySize = 10;
    public float spawnRadius = 10f;
    public float minDistanceFromObject = 5f;
    public float maxDistanceFromObject = 10f;
    public float avoidanceDistance = 2f;  // The distance at which Friends will avoid each other.
    Transform objectToSpawnAround;


    public List<FriendStates> Friends;  // A list of all spawned Friends.

    void Start()
    {
        meleePrefab = GameObjectsManager.Instance.MeleePrefab;
        rangedPrefab = GameObjectsManager.Instance.RangedPrefab;
        tankPrefab = GameObjectsManager.Instance.TankPrefab;

        objectToSpawnAround = GameObjectsManager.Instance.Player.transform;
        Friends = new List<FriendStates>();
        //SpawnFriends(10, Soldiers.Melee);
        //meleePrefab.gameObject.SetActive(false);
    }

    public void SpawnFriends(int armySize, Soldiers soldiersType)
    {
        meleeChance = rangedChance = tankChance = 0;

        switch (soldiersType)
        {
            case Soldiers.Melee:
                meleeChance = 1;
                break;
            case Soldiers.Ranged:
                rangedChance = 1;
                break;
            case Soldiers.MeleeRanged:
                meleeChance = 0.6f;
                rangedChance = 0.4f;
                break;
            case Soldiers.MeleeRangedTank:
                meleeChance = 0.5f;
                rangedChance = 0.3f;
                tankChance = 0.2f;
                break;
        }

        for (int i = 0; i < armySize; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);

            chance = Random.Range(0f, 1f);
            //Debug.Log($"Chance {chance}");
            FriendStates friend;

            min = (meleeChance < rangedChance) ? (meleeChance < tankChance ? meleeChance : tankChance) : (rangedChance < tankChance ? rangedChance : tankChance);
            max = (meleeChance > rangedChance) ? (meleeChance > tankChance ? meleeChance : tankChance) : (rangedChance > tankChance ? rangedChance : tankChance);
            mid = (meleeChance + rangedChance + tankChance) - min - max;

            if (chance < min)
            {
                friend = Instantiate(tankPrefab, randomPosition, Quaternion.identity) as FriendStates;
            }
            else if (chance < mid)
            {
                friend = Instantiate(rangedPrefab, randomPosition, Quaternion.identity) as FriendStates;
            }
            else
            {
                friend = Instantiate(meleePrefab, randomPosition, Quaternion.identity) as FriendStates;
            }
            Friends.Add(friend);
        }
    }
}
