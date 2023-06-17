using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class m_BossAttackState : StateMachineBehaviour
{
    m_BossManager bossMovement;
    float timer;


    Transform player;
    private float[] attackOptions = new float[] { 0f, 0.2f, 0.4f , 0.6f , 0.8f, 1f };

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObjectsManager.Instance.Player.transform;
        bossMovement = animator.GetComponent<m_BossManager>();
        timer = 0;
        bossMovement = animator.GetComponent<m_BossManager>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);
        timer += Time.deltaTime;
        if (distance <bossMovement.BossAttackRange)
        {
            if (timer > bossMovement.AttackDuration)
            {
                animator.SetBool("isAttacking", false);
                timer = 0;
            }
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    public int ChooseDragonAttack()
    {
        int DragonAttack = Random.Range(0, 6);
        return DragonAttack;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.SetFloat("attacks", attackOptions[ChooseDragonAttack()]);
        animator.GetComponent<StatsManager>().DisableAllWeapons();
    }
}
