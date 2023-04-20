using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    private NPCStateMachine npcStateMachine;
    private bool IsAttack2;
    Animator animator;

    public Attack(NPCStateMachine npcstateMachine) : base("Attack", npcstateMachine)
    {
        this.npcStateMachine = npcstateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.attackRange)
        {
            base.Update();
            Seek(npcStateMachine.player.transform.position);
            IsAttack2 = true;
            animator.SetBool("IsAttack2", IsAttack2);
        }
        else
        {
            IsAttack2 = false;
            animator.SetBool("IsAttack2", IsAttack2);
        }

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) > npcStateMachine.attackRange)
        {
            Debug.Log("CHASE");
            this.stateMachine.OnChangeState(npcStateMachine.chase);
        }

    }
    public void Seek(Vector3 target)
    {
        var direction = (target - npcStateMachine.transform.position).normalized;
        npcStateMachine.RB.velocity = direction * npcStateMachine.speed;
    }

    public override void Exit()
    {
        base.Exit();
    }

}
