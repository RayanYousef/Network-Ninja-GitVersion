using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState
{
    private NPCStateMachine npcStateMachine;
    private int wayPointsCounter;
    private Transform[] wayPoints;


    public Patrol(NPCStateMachine npcstateMachine) : base("Patrol", npcstateMachine)
    {
        this.npcStateMachine = npcstateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        wayPointsCounter = 0;
    }
 

    public override void Update()
    {
        base.Update();
        Seek(wayPoints[wayPointsCounter].transform.position);
        npcStateMachine.transform.localScale = Vector3.one;

        //Debug.Log(Vector3.SqrMagnitude(waypoints[wayPointsCounter].transform.position - this.transform.position));
        if (Vector3.SqrMagnitude(wayPoints[wayPointsCounter].transform.position - npcStateMachine.transform.position) < 1f)
        {
            //Debug.Log("bring next waypoint");
            wayPointsCounter++;
            wayPointsCounter %= wayPoints.Length;
        }
        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.chaseRange)
        {
            Debug.Log("CHASE");
            this.stateMachine.OnChangeState(npcStateMachine.chase);
        }
    }

    public void Seek(Vector3 target)
    {
        var direction = (target - npcStateMachine.transform.position).normalized;
        npcStateMachine.RB.velocity = direction * npcStateMachine.speed;
    }

    public override void Exit()
    {
        base.Exit();

    }
}
