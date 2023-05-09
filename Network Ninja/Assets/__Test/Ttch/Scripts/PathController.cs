using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public GameObject player;
    public Transform startPoint;
    public Transform endPoint;
    public List<Transform> wayPoints = new List<Transform>();

    public bool reversePath = false;
    private bool playerIsOnPath = false;

    private int currentWayPointIndex = 0;
    public float speed = 50f;

    void FixedUpdate()
    {

        if (playerIsOnPath)
        {
            MovePlayerAlongPath();
        }
    }

    public void GateState(GateStates state, Collider other)
    {
        if (other.gameObject.transform.parent.gameObject == player) 
        {
            switch (state)
            {
                case GateStates.Start:
                    {
                        reversePath = false;
                        break;
                    }
                case GateStates.End:
                    {
                        reversePath = true;
                        break;
                    }
                default:
                    break;
            }
            StartMovingPlayer();
        }
    }

    public void StartMovingPlayer()
    {
        playerIsOnPath = true;
        currentWayPointIndex = reversePath ? wayPoints.Count - 1 : 0;
       // Debug.Log("Started Moving");
    }

    public void StopMovingPlayer()
    {
        playerIsOnPath = false;
    }

    void MovePlayerAlongPath()
    {
        //Debug.Log("Moving along path");

        Transform currentWayPoint = wayPoints[currentWayPointIndex];

        // Move towards the current waypoint


        Debug.Log("Current waypoint is " + currentWayPointIndex);

        player.transform.position = Vector3.MoveTowards(
            player.transform.position,
            currentWayPoint.position,
            Time.deltaTime * speed
        );

        //Move to the next wayPoint
        if (Vector3.Distance(player.transform.position, currentWayPoint.position)<0.5f)
        {

            currentWayPointIndex += reversePath ? -1 : 1;

            if (currentWayPointIndex < 0 || currentWayPointIndex >= wayPoints.Count)
            {
               // Debug.Log("Stopping");
                StopMovingPlayer();
            }
        }
    }
}
