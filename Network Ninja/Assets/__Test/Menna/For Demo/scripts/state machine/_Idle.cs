using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Idle : StateMachineBehaviour
{
    public float chaseRange;


    Rigidbody RB;
    private Transform player;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player = GameObjectsManager.Instance.Player.transform;
        RB = animator.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle");

        if (Vector3.Distance(player.transform.position , RB.transform.position) < chaseRange)
        {
            Debug.Log("CHASE");
            //animator.SetTrigger("Chase");
            animator.SetBool("IsChasing" , true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // animator.ResetTrigger("Chase");
    }
}
