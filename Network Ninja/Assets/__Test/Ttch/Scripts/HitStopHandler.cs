using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopHandler : MonoBehaviour
{
    private bool isWaitingForAnimationStop = false;
    private bool isWaitingForTimeStop = false;

    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
    }


    //HitStop Logic with ANIMATION
    public void AnimationStop(float duration, float rate)
    {
        if (isWaitingForAnimationStop)
            return;
        rb.isKinematic = true;
        animator.speed = rate;
        StartCoroutine(WaitForAnimationStop(duration));
    }
    public void AnimationStop(float duration)
    {
        AnimationStop(duration, 0.0f);
    }
    IEnumerator WaitForAnimationStop(float duration)
    {
        isWaitingForAnimationStop = true;
        yield return new WaitForSecondsRealtime(duration);
        rb.isKinematic = false;
        animator.speed = 1;
        isWaitingForAnimationStop = false;
    }


    //HitStop Logic with TIME SCALE
    public void TimeStop(float duration, float timeScale)
    {
        if (isWaitingForTimeStop)
            return;
        Time.timeScale = timeScale;
        StartCoroutine(WaitForTimeStop(duration));
    }
    public void TimeStop(float duration)
    {
        TimeStop(duration, 0.0f);
    }
    IEnumerator WaitForTimeStop(float duration)
    {
        isWaitingForTimeStop = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        isWaitingForTimeStop = false;
    }
}
