using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CS_AnimatorController : MonoBehaviour
{

    [Header("GameObject Components")]
    [SerializeField] Animator anim;
    [SerializeField] CS_DamageObject damageOBject;

    [Header("Applied Force To Animation")]
    [SerializeField] float appliedForce;

    [Header("Animator Parameters")]
    [SerializeField] string i_Combo_1;
    [SerializeField] string i_Combo_2;
    [SerializeField] string f_Direction, f_MotionTime, f_animSpeed;
    [SerializeField] string b_Grounded, b_Attacking, b_Dashing, b_Jumping, b_canTransit, t_Dash, t_Jump;

    public CS_DamageObject AttackHandler { get => damageOBject; }
    public string I_Combo_1 { get => i_Combo_1; }
    public string I_Combo_2 { get => i_Combo_2; set => i_Combo_2 = value; }
    public string F_MotionTime { get => f_MotionTime; }
    public string F_Direction { get => f_Direction; }
    public string F_animSpeed { get => f_animSpeed; }
    public string B_canTransit { get => b_canTransit; }
    public string B_Attacking { get => b_Attacking; }
    public string B_Grounded { get => b_Grounded; }
    public string B_Jumping { get => b_Jumping; }
    public string B_Dashing { get => b_Dashing; }
    public string T_Jump { get => t_Jump; }
    public string T_Dash { get => t_Dash; }


    public bool Grounded
    {
        get => anim.GetBool(b_Grounded);
    }



    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        damageOBject = GetComponentInChildren<CS_DamageObject>();   
    }

    public void SetGrounded(bool value)
    {
        anim.SetBool(b_Grounded, value);
    }

    #region Animation State Modifiers
    public void SetAnimationMotion(float motionTime)
    {
        anim.SetFloat(f_MotionTime, motionTime);
    }

    public void SetAnimationSpeed(float speed)
    {
        anim.SetFloat(f_animSpeed, speed);
    }


    #endregion

    #region Animation Events

    public void PlayFootstepsAudio()
    {if(AudioManager.instance!= null)
        AudioManager.instance.PlayVariedPitcheAudio(AudioManager.instance.Footsteps);
    }

    public void ResetCombo()
    {
        anim.SetInteger(I_Combo_1, 0);
        anim.SetInteger(I_Combo_2, 0);
        anim.SetBool(b_Attacking, false);
        //attackHandler.gameObject.SetActive(false);

    }

    public void CanTransit()
    {
        anim.SetBool(b_canTransit, true);
        damageOBject.gameObject.SetActive(false);
    }

    public void EnableDamageObject()
    {
        damageOBject.gameObject.SetActive(true);
    }

    public void NegateDashing()
    {
        anim.SetBool(B_Dashing, false);
    }

    public void NegateJumping()
    {
        anim.SetBool(B_Jumping, false);
    }

    public void ApplyForwardForce(float force)
    {
        GetComponent<Rigidbody>().
            AddForce(transform.forward * (appliedForce + force), ForceMode.Impulse);
  
    }

    public void OnAttackSetFirstPoint()
    {
       // attackHandler.SetFirstPoint();  
    }

    public void OnAttackSetPointTwo(ProjectOnPlaneAxis axis)
    {
        Vector3 projectionAxis= Vector3.zero;
        switch(axis)
        {
            case ProjectOnPlaneAxis.forward: projectionAxis = transform.forward; break;
            case ProjectOnPlaneAxis.right: projectionAxis = transform.right; break;

        }

        damageOBject.gameObject.SetActive(true);
        //attackHandler.ProjectOnAxis(projectionAxis, transform.forward, GetComponent<Collider>().bounds.center);
    }

    #endregion
}
