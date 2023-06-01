using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class m_StateManager : MonoBehaviour
{
    enum EnemyState { Idle, Chase, Attack };
    EnemyState currentState;
    Animator animator;
    private Transform player;
    private Rigidbody RB;
    NavMeshAgent agent;
    float timer;

    [SerializeField] private float speed;
    [SerializeField] float chaseRange;
    [SerializeField] float AttackRange;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObjectsManager.Instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Idle;
        timer = 0;
        agent.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeState(currentState);

    }
    private void ChangeState(EnemyState currentState)
    {
        switch (currentState)
        {
            case EnemyState.Idle:   Idle(); break;
            case EnemyState.Chase:  Chase(); break;
            case EnemyState.Attack: Attack(); break;
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(player.position, RB.transform.position) < chaseRange)
        {
            currentState = EnemyState.Chase;
        }

        if (Vector3.Distance(player.position, RB.transform.position) < AttackRange)
        {
            currentState = EnemyState.Attack;
       
        }
    }

    private void Chase()
    {

        if (Vector3.Distance(player.position, RB.transform.position) < chaseRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("IsChasing", true);
        }
        else
        {
            animator.SetBool("IsChasing", false);
        }


        if (Vector3.Distance(player.position, RB.transform.position) < AttackRange)
        {
            currentState = EnemyState.Attack;

        }

        if (Vector3.Distance(player.position, RB.transform.position) > chaseRange)
        {
            currentState = EnemyState.Idle;
        }


    }

    private void Attack()
    {
        if (Vector3.Distance(player.position, RB.transform.position) < AttackRange)
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                animator.SetTrigger("Attack");
                timer = 0;
            }
        }


        if (Vector3.Distance(player.position, RB.transform.position) < chaseRange)
        {
            currentState = EnemyState.Chase;
        }

        if (Vector3.Distance(player.position, RB.transform.position) < AttackRange)
        {
            currentState = EnemyState.Attack;

        }

    }
}