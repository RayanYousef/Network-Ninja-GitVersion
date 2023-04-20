using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : StateMachine
{
    public float speed;
    public float chaseRange;
    public float attackRange;

    public Rigidbody RB;
    public GameObject player;



    [HideInInspector]
    public Idle idle;
    [HideInInspector]
    public Chase chase;
    [HideInInspector]
    public Attack attack;

    void Awake()
    {
        idle= new Idle(this);
        chase = new Chase(this);
        attack = new Attack(this);
    }


    protected override BaseState GetInitState()
    {
        return idle ;
    }

 
}
