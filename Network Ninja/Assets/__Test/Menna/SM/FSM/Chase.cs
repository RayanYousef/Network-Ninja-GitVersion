using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : BaseState
{
    private NPCStateMachine npcStateMachine;


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
        Seek(npcStateMachine.player.transform.position);
       // npcStateMachine.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.attackRange)
        {
            Debug.Log("ATTACK");
            this.stateMachine.OnChangeState(npcStateMachine.attack);
        }

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) > npcStateMachine.chaseRange)
        {
            Debug.Log("PATROL");
            this.stateMachine.OnChangeState(npcStateMachine.patrol);
        }
    }

    public void Seek(Vector3 target)
    {
        var direction = (target - npcStateMachine.transform.position).normalized;
        npcStateMachine.RB.velocity = direction * npcStateMachine.speed;

        //// Rotate to face player
        npcStateMachine.transform.LookAt(npcStateMachine.transform.position + direction);
        npcStateMachine.RB.velocity = direction *npcStateMachine.speed;
    }

    public override void Exit()
    {
        base.Exit();
        npcStateMachine.speed /= 2;
    }

}
