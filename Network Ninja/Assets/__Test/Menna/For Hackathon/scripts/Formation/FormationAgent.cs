using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FormationAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    public int toFollowIndex;
    public Vector3 toFollow;
    private bool changeIdle;
    private bool noAnimation = true;

    private void Start()
    {
        //transform.localScale = Vector3.one;
        anim = GetComponent<Animator>();

        StartCoroutine(AgentMovement());
    }

    IEnumerator AgentMovement()
    {
        if (toFollow != null)
        {
            while(Vector3.SqrMagnitude(this.transform.position - toFollow) <= 0.2)
            {
                toFollow = GetComponentInParent<Formation>().IndicatorsList[toFollowIndex];
                agent.SetDestination(toFollow);
                yield return null;
            }
            if (anim != null)
            {
                changeIdle = (UnityEngine.Random.Range(0, 2) == 1);
                Debug.Log(changeIdle);
                if (changeIdle)
                    anim.SetBool("isFirstFightPose", true);
                else
                    anim.SetBool("isSecondFightPose", true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (toFollow != null)
        {
            toFollow = GetComponentInParent<Formation>().IndicatorsList[toFollowIndex];
            agent.SetDestination(toFollow);
        }

        ////  if(animator != null)
        ////  animator.SetFloat("Move", agent.velocity.magnitude);

        //if(Vector3.SqrMagnitude(this.transform.position - toFollow) <= 0.2)
        //{
        //    if(noAnimation)
        //    {
        //        changeIdle = (UnityEngine.Random.Range(0, 2) == 1);
        //        Debug.Log(changeIdle);
        //        if (changeIdle)
        //        {
        //            anim.SetBool("isFirstIdlePose", true);
        //        }
        //        else
        //        {
        //            anim.SetBool("isSecondIdlePose", true);
        //        }
        //        noAnimation = false;
        //    }
        //}
        ////else
        ////{
        ////    anim.SetBool("isFirstIdlePose", false);
        ////    anim.SetBool("isSecondIdlePose", false);
        ////}
    }
}
