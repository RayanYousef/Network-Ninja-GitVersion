using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Attack : StateMachineBehaviour
{
    public float speed;
    public float attackRange;

    Rigidbody RB;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB = animator.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Vector3.SqrMagnitude(player.transform.position - RB.transform.position) > attackRange)
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


}
