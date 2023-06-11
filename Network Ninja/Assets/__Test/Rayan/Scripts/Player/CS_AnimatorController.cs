using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CS_AnimatorController : MonoBehaviour
{
    public enum DamageObjectDirection { ForwardUp,ForwardDown,BackwardUp,BackwardDown }

    [Header("GameObject Components")]
    [SerializeField] CS_PlayerManager playerManager;
    [SerializeField] Transform forwardUp,forwardDown,backwardUp,backwardDown;

    [Header("Applied Force To Animation")]
    [SerializeField] float appliedForce;

    [Header("Animator Parameters")]
    [SerializeField] string i_Combo_1;
    [SerializeField] string i_Combo_2;
    [SerializeField] string f_Direction, f_MotionTime, f_animSpeed;
    [SerializeField] string b_TakingDamage,b_Grounded, b_Attacking, b_Ultimate, b_Dashing, b_Jumping, b_canTransit, t_Dash, t_Jump,t_Ultimate;

    public CS_PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }
    public string I_Combo_1 { get => i_Combo_1; }
    public string I_Combo_2 { get => i_Combo_2; set => i_Combo_2 = value; }
    public string F_MotionTime { get => f_MotionTime; }
    public string F_Direction { get => f_Direction; }
    public string F_animSpeed { get => f_animSpeed; }
    public string B_canTransit { get => b_canTransit; }
    public string B_Attacking { get => b_Attacking; }
    public string B_Grounded { get => b_Grounded; }
    public string B_Jumping { get => b_Jumping; }
    public string B_Ultimate { get => b_Ultimate; }
    public string B_Dashing { get => b_Dashing; }
    public string B_TakingDamage { get => b_TakingDamage; }

    public string T_Jump { get => t_Jump; }
    public string T_Dash { get => t_Dash; }
    public string T_Ultimate { get => t_Ultimate; }


    public bool Grounded
    {
        get => PlayerManager.Anim.GetBool(b_Grounded);
    }

    public void SetGrounded(bool value)
    {
        PlayerManager.Anim.SetBool(b_Grounded, value);
    }

    #region public function
    public void PlayTakeDamageAnim()
    {
        if (PlayerManager.Anim.GetBool(B_TakingDamage) == false)
        {
            PlayerManager.Anim.SetBool(B_TakingDamage, true);
            PlayerManager.Anim.Play("Take Damage");
        }
    }
    #endregion



    #region Animation State Modifiers
    public void SetAnimationMotion(float motionTime)
    {
        PlayerManager.Anim.SetFloat(f_MotionTime, motionTime);
    }

    public void SetAnimationSpeed(float speed)
    {
        PlayerManager.Anim.SetFloat(f_animSpeed, speed);
    }


    #endregion

    #region Animation Events

    public void ResetCombo()
    {
            PlayerManager.Anim.SetInteger(I_Combo_1, 0);
            PlayerManager.Anim.SetInteger(I_Combo_2, 0);
            PlayerManager.Anim.SetBool(b_Attacking, false);
    }

    public void CanTransit()
    {
            PlayerManager.Anim.SetBool(b_canTransit, true);
        PlayerManager.PStatsManager.DisableAllWeapons();
    }

    #region Damage Object Functions 

    public void EnableWeapon(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in PlayerManager.PStatsManager.DamageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
            {
                damageObject.gameObject.SetActive(true);
            }
        }
    }
    public void EnableWeaponForwardUp(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in PlayerManager.PStatsManager.DamageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
            {

                damageObject.gameObject.SetActive(true);
                damageObject.gameObject.transform.position = forwardUp.position;
                damageObject.gameObject.transform.rotation = forwardUp.rotation;
            }
        }
    }
    public void EnableWeaponForwardDown(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in PlayerManager.PStatsManager.DamageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
            {

                damageObject.gameObject.SetActive(true);
                damageObject.gameObject.transform.position = forwardDown.position;
                damageObject.gameObject.transform.rotation = forwardDown.rotation;
            }
        }
    }

    public void EnableWeaponBackwardUp(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in PlayerManager.PStatsManager.DamageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
            {

                damageObject.gameObject.SetActive(true);
                damageObject.gameObject.transform.position = backwardUp.position;
                damageObject.gameObject.transform.rotation = backwardUp.rotation;
            }
        }
    }

    public void EnableWeaponBackwardDown(string WeaponName)
    {
        foreach (CS_DamageObject damageObject in PlayerManager.PStatsManager.DamageObjects)
        {
            if (damageObject.WeaponName == WeaponName)
            {

                damageObject.gameObject.SetActive(true);
                damageObject.gameObject.transform.position = backwardDown.position;
                damageObject.gameObject.transform.rotation = backwardDown.rotation;
            }
        }
    }

    #endregion
    public void NegateDashing()
    {
        PlayerManager.Anim.SetBool(B_Dashing, false);
    }

    public void NegateTakingDamage()
    {
        PlayerManager.Anim.SetBool(B_TakingDamage, false);
    }

    public void NegateJumping()
    {
        PlayerManager.Anim.SetBool(B_Jumping, false);
    }

    public void PlayFootstepsAudio()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayVariedPitcheAudio(AudioManager.instance.Footsteps);
    }

    public void ApplyForwardForce(float force)
    {
        GetComponent<Rigidbody>().
            AddForce(transform.forward * (appliedForce + force), ForceMode.Impulse);
  
    }

    #endregion
}
