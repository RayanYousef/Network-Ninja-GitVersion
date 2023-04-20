using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaseAndAttack : MonoBehaviour
{
    enum FishState { Chase, Attack };
    FishState currentState;
    Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    private Rigidbody fishRB;

    private bool IsAttack1;
    private bool IsAttack2;
    [SerializeField] float distance;
    void Start()
    {
        fishRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
            case FishState.Chase: Debug.Log("CHASE"); Chase(); break;
            case FishState.Attack: Debug.Log("ATTACK"); Attack(); break;
        }
    }

    public void Seek(Vector3 target)
    {
        var direction = (target - transform.position).normalized;

        //// Rotate to face player
        transform.LookAt(transform.position + direction);
        fishRB.velocity = direction * speed;


    }

    

    private void Chase()
    {
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 30f)
        {
            Vector3 targetPositopn;
            targetPositopn = player.transform.position - transform.forward * distance;
            Seek(targetPositopn);
            IsAttack1 = true;
            animator.SetBool("IsAttack1", IsAttack1);
        }
        else
        {
            IsAttack1 = false;
            animator.SetBool("IsAttack1", IsAttack1);
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 20f)
        {
            currentState = FishState.Attack;
        }


    }

    private void Attack()
    {
        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 20f)
        {
            Vector3 targetPositopn;
            targetPositopn = player.transform.position - transform.forward * distance;
            Seek(targetPositopn);
            IsAttack2 = true;
            animator.SetBool("IsAttack2", IsAttack2);
        }
        else
        {
            IsAttack2 = false;
            animator.SetBool("IsAttack2", IsAttack2);
        }

        if (Vector3.SqrMagnitude(player.transform.position - this.transform.position) > 20f && Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 30)
        {
            // Debug.Log("chase");
            currentState = FishState.Chase;
        }

    }
}
