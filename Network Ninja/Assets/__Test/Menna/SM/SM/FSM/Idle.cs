using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    private NPCStateMachine npcStateMachine;

    public Idle(NPCStateMachine npcstateMachine) : base("Idle", npcstateMachine)
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
       var direction = (npcStateMachine.player.transform.position - npcStateMachine.transform.position).normalized;
       npcStateMachine.transform.LookAt(npcStateMachine.player.transform.position + direction);

        if (Vector3.SqrMagnitude(npcStateMachine.player.transform.position - npcStateMachine.transform.position) < npcStateMachine.chaseRange)
        {
            Debug.Log("CHASE");
            this.stateMachine.OnChangeState(npcStateMachine.chase);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
