using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : StateMachineBehaviour
{
     m_EnemyManager enemyManager;
     float timer;
    Animator animator;
    Coroutine coroutine;
    Rigidbody rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyManager = animator.GetComponent<m_EnemyManager>();
        timer = 0;
        animator = animator.GetComponent<Animator>();
        rb = animator.GetComponent<Rigidbody>();
        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //timer += Time.deltaTime;
        //if (timer > 10)
        //{
        //    animator.SetBool("BackToAttack", true);

        //}
    }

    //  OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // animator.SetBool("BackToAttack", true);
        rb.GetComponent<MonoBehaviour>().StartCoroutine(IntervalToBackToAttack());

    }

       public IEnumerator IntervalToBackToAttack()
    {
        yield return new WaitForSeconds(10);
        animator.SetBool("BackToAttack", true);
    }


}
