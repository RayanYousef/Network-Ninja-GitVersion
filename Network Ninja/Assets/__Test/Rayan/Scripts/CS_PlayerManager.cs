using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_PlayerManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] CS_MovementController moveController;
    [SerializeField] CS_AnimatorController animController;
    [SerializeField] CS_CameraTarget camTarget;
    [SerializeField] Rigidbody rb;

    [Header("Variables")]
    [SerializeField] float drag;
    [SerializeField] float clicksIntervalTime;

    [Header("Script Variables")]
    [SerializeField] CharacterState currentState;
    float clicksIntervalTimer;
    bool AndroidBuild = false;

    public CharacterState AnimatorCurrentState { get => currentState; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        moveController = GetComponentInChildren<CS_MovementController>();
        animController = GetComponentInChildren<CS_AnimatorController>();
        camTarget = GetComponentInChildren<CS_CameraTarget>();
        rb = GetComponentInChildren<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
            clicksIntervalTimer += Time.deltaTime;
    }

    #region Main Input Functions
    public void SendInputDirection(Vector2 value)
    {
        // Setting the value of the direction on Movement Controller
        moveController.InputDirection = new Vector3(value.x, 0, value.y);
        anim.SetFloat(animController.F_Direction, value.sqrMagnitude);
    }

    public void SendInputRotation(Vector2 value)
    {
        // Setting the value of the DeltaValues on Camera Target
        //if(Mouse.current.leftButton.isPressed) 

        //camTarget.DeltaValues = Mouse.current.rightButton.isPressed || AndroidBuild ? value : Vector2.zero;
        camTarget.DeltaValues = value;
    }

    public void SendJumpInputState(bool value)
    {
        if (value == true && animController.Grounded)
        {
            anim.SetTrigger(animController.T_Jump);
        }
    }

    public void SendDashInputState(bool value)
    {
        if (value == true && currentState!= CharacterState.Dashing)
        {
            anim.SetTrigger(animController.T_Dash);
        }
    }
    public void SendAttackInputState(bool value)
    {
        if (value == true && animController.Grounded && clicksIntervalTimer>clicksIntervalTime)
        {
            Debug.Log("Attack Clicked");
            clicksIntervalTimer= 0;
            anim.SetInteger(animController.I_Combo, anim.GetInteger(animController.I_Combo) + 1);
        }
    }
    #endregion

    #region Messages from Player Inputs
    public void OnMove(InputValue value)
    {
        SendInputDirection(value.Get<Vector2>());
        //Debug.Log("MoveTowardsDirection:" + value.Get<Vector2>());
    }

    public void OnCameraRotation(InputValue value)
    {
        SendInputRotation(value.Get<Vector2>());
        //Debug.Log("CamRotation:" + value.Get<Vector2>());
    }
    public void OnJump(InputValue value)
    {
        SendJumpInputState(value.isPressed);
        //Debug.Log("SendJumpInputState:" + value.isPressed);
    }

    public void OnDash(InputValue value)
    {
        SendDashInputState(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

    public void OnAttack(InputValue value)
    {
        SendAttackInputState(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

    #endregion

    #region Animator States
    public void OnStateEnter(CharacterState enteredState)
    {
        //Debug.Log("Entered State:" + enteredState);
        currentState = enteredState;
        if (enteredState != CharacterState.Attacking)
            animController.ResetCombo();

        ResetParameters();

        switch (enteredState)
        {
            case CharacterState.Running:
                break;

            case CharacterState.Jumping:
                anim.SetBool(animController.B_Jumping, true);
                moveController.Jump();
                break;

            case CharacterState.Dashing:
                anim.SetBool(animController.B_Dashing, true);
                moveController.Dash();
                break;

            case CharacterState.Attacking:
                anim.SetBool(animController.B_Attacking, true);
                break;

            case CharacterState.Falling:
                break;

            case CharacterState.Idling:
                break;

        }
    }

    public void OnStateExit(CharacterState exitedState)
    {
        //Debug.Log("Exited State:" + exitedState);

        switch (exitedState)
        {

        }
    }
    public void ResetParameters()
    {
        anim.SetBool(animController.B_Dashing, false);
        anim.SetBool(animController.B_Jumping, false);
        anim.SetBool(animController.B_Attacking, false);
        anim.SetBool(animController.B_canTransit,false);
        rb.drag = drag;
        
    }
    #endregion



}
