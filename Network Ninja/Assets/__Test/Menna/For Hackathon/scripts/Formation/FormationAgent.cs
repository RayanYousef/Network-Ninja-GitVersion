using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FormationAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private Formation battalion;

    public int toFollowIndex;
    public Vector3 toFollow;

    int changeIdle;

    float timer;

    private void Start()
    {
        if (anim != null) { }
        else anim = GetComponent<Animator>();

        if (battalion != null) { }
        else battalion = GetComponentInParent<Formation>();

        toFollow = battalion.IndicatorsList[toFollowIndex];
        agent.SetDestination(toFollow);
    }

    private void FixedUpdate()
    {
        if (changeIdle > 0)
            return;
        timer += Time.deltaTime;

        Debug.Log("d:" + agent.remainingDistance);
        if (agent.remainingDistance <= 0.05f && !agent.pathPending && changeIdle == 0 && timer > 1)
        {
            ChangeAnimation();
        }
    }

    private void OnEnable()
    {
        if (changeIdle != 0)
            ChangeAnimation();
    }

    void ChangeAnimation()
    {
        changeIdle = Random.Range(1, 3);
        switch(changeIdle)
        {
            case 1:
                anim.SetBool("isFirstFightPose", true);
                break;
            case 2:
                anim.SetBool("isSecondFightPose", true);
                break;
        }
    }
}
