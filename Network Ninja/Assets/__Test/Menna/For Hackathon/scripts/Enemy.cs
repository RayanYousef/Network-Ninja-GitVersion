using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;


    #region //variables for overlap
    ////overlap
    //public float avoidanceRadius = 1f;
    //public float avoidanceForce = 1f;
    //public LayerMask overlapLayer;

    //private Collider[] overlappingColliders;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = Vector3.one * 0.05f;

        player = GameObjectsManager.Instance.Player.transform;
        //player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    public void showHealth()
    {
        Debug.Log(GetComponent<StatsManager>().Stats.CurrentHealth + gameObject.name);
    }



}
