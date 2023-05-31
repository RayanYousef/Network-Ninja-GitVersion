using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : StateMachine
{
    public Transform[] waypoints;
    public float speed;
    public Rigidbody RB;
    public Transform player;

    public float chaseRange;
    public float attackRange;

    [HideInInspector]
    public Patrol patrol;
    [HideInInspector]
    public Chase chase;
    [HideInInspector]
    public Attack attack;

    void Awake()
    {
        patrol = new Patrol(this);
        chase = new Chase(this);
        attack = new Attack(this);
    }

    protected override BaseState GetInitState()
    {
        return patrol;
    }

 
}
