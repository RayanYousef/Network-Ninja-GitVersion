using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterState { Idling, Running, Attacking, Jumping, Dashing,Falling, Hit, Dying, Ultimate };

public class CS_StatesHandler : StateMachineBehaviour
{


    [SerializeField]CharacterState thisState;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.TryGetComponent<CS_PlayerManager>(out CS_PlayerManager playerManager);
        if (playerManager != null)
        {
            playerManager.OnStateEnter(thisState);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.TryGetComponent<CS_PlayerManager>(out CS_PlayerManager playerManager);
        if (playerManager != null)
        {
            playerManager.OnStateExit(thisState);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
