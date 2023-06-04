using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class m_BossPatrolState : StateMachineBehaviour
{
    float timer;
    Transform player;
    Transform Boss;
    bool isChasing = false;
    NavMeshAgent agent;
    m_BossMovement bossMovement;


    public float chaseRange;
    public float spawnRadius = 20f;
    public float minDistanceFromObject = 15f;
    public float maxDistanceFromObject = 25f;

    List<Vector3>waypoints= new List<Vector3>();


    [SerializeField] float AttackRange;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        player = GameObjectsManager.Instance.Player.transform;
        Boss = GameObjectsManager.Instance.Boss.transform;
        bossMovement = animator.GetComponent<m_BossMovement>();

        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        timer = 0;

       // GameObject go= GameObject.FindGameObjectWithTag("waypoints");
        for(int i =0; i<3; i++)
        {
            Vector3 randomPosition = player.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)).normalized * Random.Range(minDistanceFromObject, maxDistanceFromObject);
            waypoints.Add(randomPosition);
        }

        agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)]);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (agent.remainingDistance<= agent.stoppingDistance)
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)]);
        }

        timer += Time.deltaTime;
        if (distance > chaseRange)
        {
            if (timer > 3)
            {
                animator.SetBool("isPatrolling", false);
            }
        }

        if (distance <= chaseRange && distance > AttackRange)
        {
            animator.SetBool("isChasing", true);
            isChasing= true;
        }
        if (distance <= AttackRange)
        {
            if(bossMovement.LookAtPlyer == true)
            {
                bossMovement.LookAtPlayer();
            }
            animator.SetBool("isPatrolling", false);
        }
    }
    public bool TheDragonIsChasing()
    {
        if (isChasing == true)
        {
            return true;
        }
        return false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
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
