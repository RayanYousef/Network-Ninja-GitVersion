using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _Idle : StateMachineBehaviour
{
    Rigidbody RB;
    private Transform player;
    NavMeshAgent agent;
    Animator animator;
    float timer;
    m_EnemyManager enemyManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB = animator.GetComponent<Rigidbody>();
        player = GameObjectsManager.Instance.Player.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        animator = animator.GetComponent<Animator>();
        timer =0;
        enemyManager = animator.GetComponent<m_EnemyManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (Vector3.Distance(player.position, RB.transform.position) <enemyManager.enemyChaseRange && Vector3.Distance(player.position, RB.transform.position) >enemyManager.enemyAttackRange)
        {
            animator.SetBool("IsChasing", true);
        }

        if(Vector3.Distance(player.position , RB.transform.position) <enemyManager.enemyAttackRange)
        {
            if (timer > enemyManager.intervalBetweenAttacks)
            {
                animator.SetTrigger("Attack");
                timer = 0;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
