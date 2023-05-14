using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _Attack : StateMachineBehaviour
{
    public float attackRange;

    Rigidbody RB;
    private Transform player;
    private NavMeshAgent agent;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB = animator.GetComponent<Rigidbody>();
        // player = GameObjectsManager.Instance.Player.transform;
        player = GameObjectsManager.Instance.Player.transform;

        RB.velocity = Vector3.zero;
        agent = animator.GetComponent<NavMeshAgent>();  
        agent.velocity = Vector3.zero;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // agent.velocity = Vector3.zero;

        if (Vector3.Distance(player.position, RB.transform.position) > attackRange)
        {
            Debug.Log("CHASE");
           // animator.SetTrigger("Chase");
            animator.SetBool("IsAttacking", false);

        }
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // animator.ResetTrigger("Chase");

    }


}
