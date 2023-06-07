using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class m_BossChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    [SerializeField] int ChaseRange ;
    [SerializeField] int AttackRange;
    

// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObjectsManager.Instance.Player.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 12f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);

        Debug.Log("distance betweeen boss and player is " +  distance);
        if (distance > ChaseRange || distance <= AttackRange)
        {
            animator.SetBool("isChasing", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }


}
