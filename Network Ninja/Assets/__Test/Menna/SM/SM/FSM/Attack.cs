using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    private NPCStateMachine npcStateMachine;


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
        base.Update();
        Seek(npcStateMachine.player.transform.position);
      //  npcStateMachine.transform.localScale = new Vector3(2f, 2f, 2f);

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) > npcStateMachine.attackRange)
        {
            //Debug.Log("CHASE");
            this.stateMachine.OnChangeState(npcStateMachine.chase);
        }
    }
    public void Seek(Vector3 target)
    {
        var direction = (target - npcStateMachine.transform.position).normalized;
        npcStateMachine.RB.velocity = direction * npcStateMachine.speed;
        //// Rotate to face player
        npcStateMachine.transform.LookAt(npcStateMachine.transform.position + direction);
        npcStateMachine.RB.velocity = direction * npcStateMachine.speed;
    }

    public override void Exit()
    {
        base.Exit();
    }

}
