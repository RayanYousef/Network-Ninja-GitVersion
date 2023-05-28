using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Patrol : StateMachineBehaviour
{
    public Transform[] waypoints;
    public float speed;
    public float chaseRange;
    public float attackRange;

    private int wayPointsCounter;
    Rigidbody RB;
    Transform player;




    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wayPointsCounter = 0;
        RB = animator.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Seek(waypoints[wayPointsCounter].transform.position);

        //Debug.Log(Vector3.SqrMagnitude(waypoints[wayPointsCounter].transform.position - this.transform.position));
        if (Vector3.SqrMagnitude(waypoints[wayPointsCounter].transform.position - RB.transform.position) < 1f)
        {
            //Debug.Log("bring next waypoint");
            wayPointsCounter++;
            wayPointsCounter %= waypoints.Length;
        }
        if (Vector3.SqrMagnitude(player.transform.position - RB.transform.position) < chaseRange)
        {
            Debug.Log("CHASE");
            animator.SetTrigger("Chase");


        }

    }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Chase");

    }

    public void Seek(Vector3 target)
    {
        var direction = (target - RB.transform.position).normalized;
        RB.velocity = direction * speed;

        //// Rotate to face player
       RB.transform.LookAt(RB.transform.position + direction);
        RB.velocity = direction * speed;
    }

}
