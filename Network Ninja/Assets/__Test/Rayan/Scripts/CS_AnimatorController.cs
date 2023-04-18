using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AnimatorController : MonoBehaviour
{

    [Header("GameObject Components")]
    [SerializeField] Animator anim;
    [SerializeField] CS_PlayerManager playerManager;
    [SerializeField] CS_MovementController moveController;

    [Header("Applied Force To Animation")]
    [SerializeField] float appliedForce;

    [Header("Animator Parameters")]
    [SerializeField] string i_Combo;
    [SerializeField] string f_Direction, f_MotionTime, f_animSpeed;
    [SerializeField] string b_Grounded, b_Attacking, b_Dashing, b_Jumping, b_canTransit, t_Dash, t_Jump;


    public string I_Combo { get => i_Combo; }
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
        playerManager = GetComponentInChildren<CS_PlayerManager>();
        moveController = GetComponentInChildren<CS_MovementController>();
        anim = GetComponentInChildren<Animator>();
    }

    public void SetGrounded(bool value)
    {
        anim.SetBool(b_Grounded, value);
    }

    #region Animation Modifiers
    public void SetAnimationMotion(float motionTime)
    {
        anim.SetFloat(f_MotionTime, motionTime);
    }

    public void SetAnimationSpeed(float speed)
    {
        anim.SetFloat(f_animSpeed, speed);
    }

    public void CanTransit()
    {
        anim.SetBool(b_canTransit, true);
    }
    #endregion



    #region Parameters Modifiers
    public void ApplyForwardForce(float force)
    {
        GetComponent<Rigidbody>().
            AddForce(transform.forward * (appliedForce+force), ForceMode.Impulse);
    }
    public void ResetCombo()
    {
        anim.SetInteger(I_Combo, 0);

    }

    public void NegateDashing()
    {
        anim.SetBool(B_Dashing, false);
    }

    public void NegateJumping()
    {
        anim.SetBool(B_Jumping, false);
    }

    #endregion


}
