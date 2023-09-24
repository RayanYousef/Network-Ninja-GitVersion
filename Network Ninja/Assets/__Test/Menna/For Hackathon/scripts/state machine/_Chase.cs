using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class _Chase : StateMachineBehaviour
{

    public float speed;
    Rigidbody RB;
    private Transform player;
    NavMeshAgent agent;
    float timer;
    m_EnemyManager enemyManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB = animator.GetComponent<Rigidbody>();
        player = GameObjectsManager.Instance.Player.transform;
        agent = RB.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        timer = 0;
        enemyManager = animator.GetComponent<m_EnemyManager>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(player.position);


        if (Vector3.Distance(player.position, RB.transform.position) > enemyManager.enemyChaseRange)
        {
            //Debug.Log("IDLE");
            animator.SetBool("IsChasing", false);
        }

        if (Vector3.Distance(player.position, RB.transform.position) <enemyManager.enemyAttackRange)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("IsChasing", false);
        }
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = 0;
        agent.velocity = Vector3.zero;
    }

}
