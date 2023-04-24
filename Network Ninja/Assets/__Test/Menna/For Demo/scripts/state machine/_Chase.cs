using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _Chase : StateMachineBehaviour
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

        //  Seek(player.transform.position);
        RB.transform.LookAt(player);
        RB.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.SqrMagnitude(player.transform.position - RB.transform.position) < attackRange)
        {
            Debug.Log("ATTACK");
            animator.SetTrigger("Attack");
        }



    }

    //public void Seek(Vector3 target)
    //{
    //    var direction = (target - RB.transform.position).normalized;
    //    RB.velocity = direction * speed;
    //    //// Rotate to face player
    //    RB.transform.LookAt(RB.transform.position + direction);
    //}


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        //animator.ResetTrigger("Idle");

    }

}
