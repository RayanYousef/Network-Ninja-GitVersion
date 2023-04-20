using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseState
{
    private NPCStateMachine npcStateMachine;
    private bool IsAttack1;
    Animator animator;


    public Chase(NPCStateMachine npcstateMachine) : base("Chase", npcstateMachine)
    {
        this.npcStateMachine = npcstateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        npcStateMachine.speed *= 2;
    }
  

    public override void Update()
    {
        base.Update();
        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.chaseRange)
        {
            Seek(GameObjectsManager.Instance.Player.transform.position);
            IsAttack1 = true;
            animator.SetBool("IsAttack1", IsAttack1);
        }
        else
        {
            IsAttack1 = false;
            animator.SetBool("IsAttack1", IsAttack1);
        }

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.attackRange)
        {
            Debug.Log("ATTACK");
            this.stateMachine.OnChangeState(npcStateMachine.attack);
        }

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) > npcStateMachine.chaseRange)
        {
            Debug.Log("Idle");
            this.stateMachine.OnChangeState(npcStateMachine.idle);
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
        npcStateMachine.speed /= 2;
    }

}
