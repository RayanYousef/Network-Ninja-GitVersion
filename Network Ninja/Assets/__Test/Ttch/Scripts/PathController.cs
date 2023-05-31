using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PathController : MonoBehaviour
{
    private GameObject player;
    public Transform startPoint;
    public Transform endPoint;

    public List<Transform> wayPoints = new List<Transform>();

    public float defaultblendtime = 2;

    private CinemachineBrain brain;

    //List of cameras that change the view based on waypoint
    public List<CinemachineVirtualCamera> WaypointCameras = new List<CinemachineVirtualCamera>();

    public float blendtime = 0.5f;


    public bool reversePath = false;
    private bool playerIsOnPath = false;

    private int currentWayPointIndex = 0;

    public float distance;
    public float speed = 50f;


    private void Start()
    {
        player = GameObjectsManager.Instance.Player;
        brain = GameObjectsManager.Instance.CameraBrain;
    }

    void FixedUpdate()
    {

        if (playerIsOnPath&& currentWayPointIndex >= 0 && currentWayPointIndex <= wayPoints.Count - 1)
        {
            brain.m_DefaultBlend.m_Time = blendtime;
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
        CinemachineVirtualCamera currentCamera = WaypointCameras[currentWayPointIndex];

        //Enable camera and remove blend time or instantaneous change
        currentCamera.enabled = true;
        brain.m_DefaultBlend.m_Time = blendtime;

        if (Vector3.Distance(player.transform.position, currentWayPoint.position) < 2)
        {
            if (currentWayPointIndex >= 0 && currentWayPointIndex <= wayPoints.Count-1)
            {
                currentWayPointIndex += reversePath ? -1 : 1;
            }
            else playerIsOnPath = false;
            foreach (CinemachineVirtualCamera cam in WaypointCameras)
            {
                brain.m_DefaultBlend.m_Time = defaultblendtime;
                cam.enabled = false;
            }

        }

        if(currentWayPointIndex<0)
        return wayPoints[0].position;
        else if(currentWayPointIndex > wayPoints.Count - 1) return wayPoints[wayPoints.Count - 1].position;
        else return wayPoints[currentWayPointIndex].position;

    }


    public void PlayerEnteredPath(GateStates state)
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
