using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_BossIdleState : StateMachineBehaviour
{
     [SerializeField] float chaseRange;
     [SerializeField] float AttackRange;

     Transform player;
     float timer;
    m_BossMovement bossMovement;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObjectsManager.Instance.Player.transform;
        bossMovement = animator.GetComponent<m_BossMovement>();
        timer =0;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if(distance > chaseRange)
        {
            if (timer > 3)
            {
                animator.SetBool("isPatrolling", true);
            }
        }


        if (distance <= chaseRange && distance > AttackRange)
        {
            animator.SetBool("isChasing", true);
        }

        if (distance <= AttackRange)
        {
            if (bossMovement.LookAtPlyer == true)
            {
                bossMovement.LookAtPlayer();
            }
            if (timer > bossMovement.IntervalBetweenBossAttacks)
            {
                //  animator.SetTrigger("Attack");
                animator.SetBool("isAttacking", true);
                timer = 0;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
