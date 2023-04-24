using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class _Chase : StateMachineBehaviour
{

    public float speed;
    public float attackRange;

    Rigidbody RB;
    private Transform player;
    NavMeshAgent agent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RB = animator.GetComponent<Rigidbody>();
      //  player = GameObjectsManager.Instance.Player.transform;
        player = GameObjectsManager.Instance.Player.transform;

        agent = RB.GetComponent<NavMeshAgent>();


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Debug.Log(Time.deltaTime);
       // Debug.Log("nav mesh");
       agent.SetDestination(player.position);
        Vector3 target = agent.destination;
        //  Seek(player.transform.position);
        RB.transform.LookAt(target);
        agent.isStopped = true;
        RB.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.SqrMagnitude(player.position - RB.transform.position) < attackRange)
        {
            Debug.Log("ATTACK");
            // animator.SetTrigger("Attack");
            animator.SetBool("IsAttacking", true);
        }


        //if (Vector3.SqrMagnitude(player.transform.position - RB.transform.position) > chaseRange)
        //{
        //    Debug.Log("IDLE");
        //    // animator.SetTrigger("Idle");
        //    animator.SetBool("IsChasing", false);
        //}

    }
    



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // animator.ResetTrigger("Attack");
      //  animator.ResetTrigger("Idle");

    }

}
