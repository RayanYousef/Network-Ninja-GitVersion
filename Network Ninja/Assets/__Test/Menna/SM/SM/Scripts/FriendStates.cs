using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FriendStates : MonoBehaviour
{
    enum FishState { Patrol, Chase, Attack };
    //public Transform[] waypoints;
    FishState currentState;
    // private int wayPointsCounter;
    Animator animator;

    [SerializeField] private float speed;
    private Rigidbody FriendRB;

    private bool IsAttack1;
    private bool IsAttack2;

    //FriendSpawner FriendSpawner;
    EnemySpawner EnemySpawner;

    public Transform player;
    public float distanceFromPlayer = 10.0f;
    public float movementSpeed = 5.0f;

    private Vector3 targetPosition;


    //overlap
    public float avoidanceRadius = 1f;
    public float avoidanceForce = 1f;
    public LayerMask overlapLayer;

    private Collider[] overlappingColliders;

    void Start()
    {
        player = GameObjectsManager.Instance.Player.transform;
        FriendRB = GetComponent<Rigidbody>();
        currentState = FishState.Patrol;
        animator = GetComponent<Animator>();

        //Calculate the target position that is distanceFromPlayer units away from the player
        targetPosition = player.position + (distanceFromPlayer * player.forward);

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

    public void Seek(Vector3 target)
    {
        var direction = (target - transform.position).normalized;

        // Rotate to face player
        transform.LookAt(transform.position + direction);
        FriendRB.velocity = direction * speed;
    }

    private void Patrol()
    {

        // Vector3 position = obj.transform.position;

        // Calculate the direction from the army's current position to the target position
        Vector3 directionToTarget = targetPosition - transform.position;

        // Normalize the direction vector to get a unit vector
        Vector3 movementDirection = directionToTarget.normalized;

        // Move the army towards the target position using the movement speed
        transform.position += movementDirection * movementSpeed * Time.deltaTime;

        // Rotate the army to face the player's direction
        transform.LookAt(player);


        //foreach (GameObject obj in EnemySpawner.enemies)
        //{
        //    if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) < 30 && Vector3.SqrMagnitude(obj.transform.position - this.transform.position) > 20)
        //    {
        //        Debug.Log("CHASE");
        //        currentState = FishState.Chase;
        //    }
        //}
    }

    private void Chase()
    {
        foreach (GameObject obj in EnemySpawner.enemies)
        {
            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) < 30f)
            {
                Seek(obj.transform.position);
                // animator.Play("Attack1");
                IsAttack1 = true;
                animator.SetBool("IsAttack1", IsAttack1);
            }

            else
            {
                IsAttack1 = false;
                animator.SetBool("IsAttack1", IsAttack1);
            }

            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) < 20f)
            {
                //Debug.Log("Attack");
                currentState = FishState.Attack;
            }

            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) > 30f)
            {
                //Debug.Log("PATROL");
                currentState = FishState.Patrol;
            }

        }
    }


    private void Attack()
    {
        foreach (GameObject obj in EnemySpawner.enemies)
        {
            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) < 20f)
            {
                Seek(obj.transform.position);
                //animator.Play("Attack2");
                IsAttack2 = true;
                animator.SetBool("IsAttack2", IsAttack2);
            }

            else
            {
                IsAttack2 = false;
                animator.SetBool("IsAttack2", IsAttack2);
            }

            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) > 20f && Vector3.SqrMagnitude(obj.transform.position - this.transform.position) < 30)
            {
                // Debug.Log("chase");
                currentState = FishState.Chase;
            }

            if (Vector3.SqrMagnitude(obj.transform.position - this.transform.position) > 30f)
            {
                // Debug.Log("PATROL");
                currentState = FishState.Patrol;
            }
        }
    }
}



















//////////////////////////////////////////////

//public Transform player;
//public float followDistance = 10f;
//public float chaseDistance = 5f;
//public float attackDistance = 1f;

//private enum ArmyState
//{
//    Idle,
//    Follow,
//    Chase,
//    Attack
//}
//private ArmyState currentState = ArmyState.Idle;

////private List<Transform> armyList;
////private List<Transform> enemyArmyList;

//EnemySpawner EnemySpawner;


//private float followSpeed = 3f;
//private float chaseSpeed = 5f;

//void Start()
//{
//    //armyList = new List<Transform>(GameObject.FindGameObjectsWithTag("m_EnemyManager"));
//    //enemyArmyList = new List<Transform>(GameObject.FindGameObjectsWithTag("Player"));


//}

//void Update()
//{
//    // Change state based on distance from player and enemy army
//    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//    float distanceToEnemy = GetDistanceToEnemy();

//    if (distanceToEnemy < chaseDistance)
//    {
//       // currentState = ArmyState.Chase;
//    }
//    else if (distanceToPlayer < followDistance)
//    {
//        currentState = ArmyState.Follow;
//    }
//    else
//    {
//        currentState = ArmyState.Idle;
//    }

//    // Handle state actions
//    switch (currentState)
//    {
//        case ArmyState.Idle:
//            // Do nothing
//            break;

//        case ArmyState.Follow:
//            FollowPlayer();
//            Debug.Log("follow player");
//            break;

//        //case ArmyState.Chase:
//        //    ChaseEnemy();
//        //    break;

//        //case ArmyState.Attack:
//        //    AttackEnemy();
//        //    break;
//    }
//}

//void FollowPlayer()
//{
//    // Move towards player
//    transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);

//    // Rotate towards player
//    transform.LookAt(player);
//}

//void ChaseEnemy()
//{
//    // Find closest enemy army
//    Transform closestEnemy = GetClosestEnemy();

//    // Move towards enemy
//    transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, chaseSpeed * Time.deltaTime);

//    // Rotate towards enemy
//    transform.LookAt(closestEnemy);

//    // Attack enemy if in range
//    float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.position);
//    if (distanceToEnemy < attackDistance)
//    {
//        currentState = ArmyState.Attack;
//    }
//}

//void AttackEnemy()
//{
//    // Find closest enemy army
//    Transform closestEnemy = GetClosestEnemy();

//    // Rotate towards enemy
//    transform.LookAt(closestEnemy);

//    // TODO: Add attack logic
//}

//float GetDistanceToEnemy()
//{
//    float closestDistance = Mathf.Infinity;

//    foreach (GameObject enemy in EnemySpawner.enemies )
//    {
//        float distance = Vector3.Distance(transform.position, enemy.transform.position);
//        if (distance < closestDistance)
//        {
//            closestDistance = distance;
//        }
//    }

//    return closestDistance;
//}

//Transform GetClosestEnemy()
//{
//    Transform closestEnemy = null;
//    float closestDistance = Mathf.Infinity;

//    foreach (GameObject enemy in EnemySpawner.enemies)
//    {
//        float distance = Vector3.Distance(transform.position, enemy.transform.position);
//        if (distance < closestDistance)
//        {
//            closestEnemy = enemy.transform;
//            closestDistance = distance;
//        }
//    }

//    return closestEnemy;
//}

//}
/////////////////////////////////////////////////



