using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;

    NavMeshAgent agent;


    ////overlap
    //public float avoidanceRadius = 1f;
    //public float avoidanceForce = 1f;
    //public LayerMask overlapLayer;

    //private Collider[] overlappingColliders;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObjectsManager.Instance.Player.transform;
        //player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.position);
        //transform.LookAt(player);
        agent.SetDestination(player.position);
        
    }

    //private void FixedUpdate()
    //{
    //    // Detect overlapping colliders within the specified radius
    //    overlappingColliders = Physics.OverlapSphere(transform.position, avoidanceRadius, overlapLayer);

    //    // Apply force to avoid overlapping with other colliders
    //    foreach (Collider collider in overlappingColliders)
    //    {
    //        if (collider.gameObject != gameObject) // Ignore self
    //        {
    //            Vector3 avoidanceDirection = transform.position - collider.transform.position;
    //            Vector3 avoidanceForceVector = avoidanceDirection.normalized * avoidanceForce;
    //            GetComponent<Rigidbody>().AddForce(avoidanceForceVector);
    //        }
    //    }
    //}


}
