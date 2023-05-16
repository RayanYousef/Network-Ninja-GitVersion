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

    private bool hasAnimation = false;

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
        if (agent.remainingDistance <= 0.25f && !agent.pathPending && !hasAnimation)
        {
            bool changeIdle;
            changeIdle = (UnityEngine.Random.Range(0, 2) == 1);
            Debug.Log(changeIdle);
            if (changeIdle)
                anim.SetBool("isFirstFightPose", true);
            else
                anim.SetBool("isSecondFightPose", true);
            hasAnimation = true;
        }
    }
}
