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

    public float distance;
    public float speed = 50f;



    void FixedUpdate()
    {

        if (playerIsOnPath&& currentWayPointIndex >= 0 && currentWayPointIndex <= wayPoints.Count - 1)
        {
            MoveObjectTowards(player, ChooseDestination());
        }
    }


    public void MoveObjectTowards(GameObject objectToMove, Vector3 Destnation)
    {
        objectToMove.transform.position = Vector3.MoveTowards(
        objectToMove.transform.position,
                       Destnation,
            Time.deltaTime * speed
        );
    }

    Vector3 ChooseDestination()
    {
        Transform currentWayPoint = wayPoints[currentWayPointIndex];
        distance = Vector3.Distance(player.transform.position, currentWayPoint.position);

        if (Vector3.Distance(player.transform.position, currentWayPoint.position) < 2)
        {
            if (currentWayPointIndex >= 0 && currentWayPointIndex <= wayPoints.Count-1)
            {
                currentWayPointIndex += reversePath ? -1 : 1;
            }
            else playerIsOnPath = false;

        }

        if(currentWayPointIndex<0)
        return wayPoints[0].position;
        else if(currentWayPointIndex > wayPoints.Count - 1) return wayPoints[wayPoints.Count - 1].position;
        else return wayPoints[currentWayPointIndex].position;

    }


    public void PlayerEnteredPath(GateStates state, Collider other)
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
            playerIsOnPath = true;
            currentWayPointIndex = reversePath ? wayPoints.Count - 1 : 0;
        }
    }
}
