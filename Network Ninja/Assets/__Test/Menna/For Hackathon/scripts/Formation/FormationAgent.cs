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
    private bool changeIdle;

    private void Start()
    {
        //transform.localScale = Vector3.one; //0.05
        anim = GetComponent<Animator>();
        StartCoroutine(AgentMovement());
    }

    IEnumerator AgentMovement()
    {
        if (toFollow != null)
        {
            while(Vector3.SqrMagnitude(this.transform.position - toFollow) <= 0.2)
            {
                toFollow = battalion.IndicatorsList[toFollowIndex];
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
}
