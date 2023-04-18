using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStates : MonoBehaviour
{
    enum FishState { Patrol, Chase, Attack };
    private Transform[] waypoints;
    FishState currentState;
    private int wayPointsCounter;
    Animator animator;

    [SerializeField] private float speed;

    private GameObject player;
    private Rigidbody EnemyRB;

    private bool IsAttack1;
    private bool IsAttack2;

   //overlap
    public float avoidanceRadius = 1f;
    public float avoidanceForce = 1f;
    public LayerMask overlapLayer;

    private Collider[] overlappingColliders;

    void Start()
    {
        player = GameObjectsManager.Instance.Player;
        EnemyRB = GetComponent<Rigidbody>();
        currentState = FishState.Patrol;
        animator = GetComponent<Animator>();
        waypoints = GameObjectsManager.Instance.WayPoints;
    }
    private void FixedUpdate()
    {
        // Detect overlapping colliders within the specified radius
        overlappingColliders = Physics.OverlapSphere(transform.position, avoidanceRadius, overlapLayer);

        // Apply force to avoid overlapping with other colliders
        foreach (Collider collider in overlappingColliders)
        {
            if (collider.gameObject != gameObject) // Ignore self
            {
                Vector3 avoidanceDirection = transform.position - collider.transform.position;
                Vector3 avoidanceForceVector = avoidanceDirection.normalized * avoidanceForce;
                GetComponent<Rigidbody>().AddForce(avoidanceForceVector);
            }
        }
    }
    void Update()
    {
        ChangeState(currentState);
        //Debug.Log(Vector3.SqrMagnitude(player.transform.position - this.transform.position));
    }

    private void ChangeState(FishState currentState)
    {
        switch (currentState)
        {
            case FishState.Patrol: Debug.Log("PATROL"); Patrol(); break;
            case FishState.Chase: Debug.Log("CHASE"); Chase(); break;
            case FishState.Attack: Debug.Log("ATTACK"); Attack(); break;
        }
    }

    public void Seek(Vector3 target )
    {
        var direction = (target - transform.position).normalized;

        //// Rotate to face player
        transform.LookAt(transform.position + direction);
        EnemyRB.velocity = direction * speed;
    }

    private void Patrol()
    {
        Seek(waypoints[wayPointsCounter].transform.position);
        if (Vector3.SqrMagnitude(waypoints[wayPointsCounter].transform.position - this.transform.position) < 2f)
        {
            wayPointsCounter++;
            if (wayPointsCounter >= waypoints.Length)
            {
                wayPointsCounter = 0;
            }
        }


        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 30 && Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 20)
        {
            Debug.Log("CHASE");
            currentState = FishState.Chase;
        }

    }

    private void Chase()
    {
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 30f)
        {
            Seek(player.transform.position);
            IsAttack1 = true;
            if(animator != null)
            {
                animator.SetBool("IsAttack1", IsAttack1);
            }
            else
            {
                Debug.Log("Animator is null - EnemyStates.cs");
            }

        }
        else
        {
            IsAttack1 = false;
            if (animator != null)
            {
                animator.SetBool("IsAttack1", IsAttack1);
            }
            else
            {
                Debug.Log("Animator is null - EnemyStates.cs");
            }
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 20f)
        {
            //Debug.Log("Attack");
            currentState = FishState.Attack;
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 30f)
        {
            //Debug.Log("PATROL");
            currentState = FishState.Patrol;
        }

    }

    private void Attack()
    {
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 20f)
        {
            Seek(player.transform.position);
            //animator.Play("Attack2");
            IsAttack2 = true;
            if (animator != null)
            {
                animator.SetBool("IsAttack2", IsAttack2);
            }
            else
            {
                Debug.Log("Animator is null - EnemyStates.cs");
            }
        }
        else
        {
            IsAttack2 = false;
            if (animator != null)
            {
            animator.SetBool("IsAttack2", IsAttack2);
            }
            else
            {
                Debug.Log("Animator is null - EnemyStates.cs");
            }
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 20f && Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 30)
        {
            // Debug.Log("chase");
            currentState = FishState.Chase;
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 30f)
        {
            // Debug.Log("PATROL");
            currentState = FishState.Patrol;
        }
    }
}
