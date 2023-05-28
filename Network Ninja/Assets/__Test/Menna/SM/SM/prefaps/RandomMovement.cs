using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class RandomMovement : MonoBehaviour 
{
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in


    //overlap
    public float avoidanceRadius = 1f;
    public float avoidanceForce = 0.1f;
    public LayerMask overlapLayer;

    private Collider[] overlappingColliders;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

    }

    private void FixedUpdate()
    {
        // Detect overlapping colliders within the specified radius
        overlappingColliders = Physics.OverlapSphere(transform.position, avoidanceRadius, overlapLayer);

        // Apply force to avoid overlapping with other colliders
        foreach (Collider collider in overlappingColliders)
        {
            if (collider.gameObject != gameObject) // Ignore self
            {
                Vector3 avoidanceDirection = transform.position - collider.transform.position;
                Vector3 avoidanceForceVector = avoidanceDirection.normalized * avoidanceForce;
                GetComponent<Rigidbody>().AddForce(avoidanceForceVector);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 20.0f, NavMesh.AllAreas)) 
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}