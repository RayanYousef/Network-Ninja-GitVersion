using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CS_PlayerManager : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] GameObject playerTopMostParent;
    [SerializeField] Animator anim;
    [SerializeField] CS_MovementController moveController;
    [SerializeField] CS_AnimatorController animController;
    [SerializeField] CS_LookAtClosestTarget lookAtClosestTarget;
    [SerializeField] CS_CameraManager camTarget;
    [SerializeField] StatsManager pStatsManager;
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerInput PlayerInputs;
    [SerializeField] FixedJoystick joyStick;

    [Header("Variables")]
    [SerializeField] float drag;
    [SerializeField] float clicksIntervalTime;

    [Header("Script Variables")]
    [SerializeField] CharacterState currentState;
    float clicksIntervalTimer;
    bool AndroidBuild = false;

    public CharacterState AnimatorCurrentState { get => currentState; }
    public Animator Anim { get => anim; }
    public CS_MovementController MoveController { get => moveController; }
    public CS_AnimatorController AnimController { get => animController; }
    public CS_CameraManager CamTarget { get => camTarget; }
    public Rigidbody Rb { get => rb; }
    public StatsManager PStatsManager { get => pStatsManager;}

    private void Awake()
    {
        if (playerTopMostParent == null)
            playerTopMostParent = gameObject;
        if(pStatsManager==null) pStatsManager = GetComponentInChildren<StatsManager>();

        anim = playerTopMostParent.GetComponentInChildren<Animator>();
        moveController = playerTopMostParent.GetComponentInChildren<CS_MovementController>();
        animController = playerTopMostParent.GetComponentInChildren<CS_AnimatorController>();
        lookAtClosestTarget = playerTopMostParent.GetComponentInChildren<CS_LookAtClosestTarget>();

        if(camTarget== null)    
        camTarget = playerTopMostParent.GetComponentInChildren<CS_CameraManager>();

        rb = playerTopMostParent.GetComponentInChildren<Rigidbody>();
        moveController.PlayerManager = this;
        animController.PlayerManager = this;

        PlayerInputs = GetComponent<PlayerInput>();


    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        clicksIntervalTimer += Time.deltaTime;

        // Old Input System 
        //SendInputDirection(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        //SendInputRotation(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
        //SendJumpInputState(Input.GetKeyDown(KeyCode.Space));
        //SendAttackInputState(Input.GetMouseButton(0));
        //SendDashInputState(Input.GetKeyDown(KeyCode.LeftShift));

        if (joyStick != null)
            SendInputDirection(joyStick.Direction);


        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyUp(KeyCode.LeftAlt))
            Cursor.lockState = CursorLockMode.Locked;

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
        else anim.ResetTrigger(animController.T_Jump);

    }

    public void SendDashInputState(bool value)
    {
        if (value == true && currentState != CharacterState.Dashing)
        {
            anim.SetTrigger(animController.T_Dash);
        }
    }
    public void SendCombo_1(bool value)
    {
        if (currentState != CharacterState.Dashing && value == true && animController.Grounded && clicksIntervalTimer > clicksIntervalTime)
        {
            Debug.Log("Attack Clicked");
            clicksIntervalTimer = 0;
            anim.SetInteger(animController.I_Combo_1, anim.GetInteger(animController.I_Combo_1) + 1);
        }
    }

    public void SendCombo_2(bool value)
    {
        if (currentState != CharacterState.Dashing && value == true && animController.Grounded && clicksIntervalTimer > clicksIntervalTime)
        {
            Debug.Log("Attack Clicked");
            clicksIntervalTimer = 0;
            anim.SetInteger(animController.I_Combo_2, anim.GetInteger(animController.I_Combo_2) + 1);
        }
    }
    #endregion

    #region Animator States
    public void OnStateEnter(CharacterState enteredState)
    {
        currentState = enteredState;
        if (enteredState != CharacterState.Attacking)
            animController.ResetCombo();

        ResetParameters();

        switch (enteredState)
        {
            case CharacterState.Running:
                break;

            case CharacterState.Jumping:
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                moveController.Jump();
                anim.SetBool(animController.B_Jumping, true);
                break;

            case CharacterState.Dashing:
                anim.SetBool(animController.B_Dashing, true);
                moveController.Dash();
                break;

            case CharacterState.Attacking:
                anim.SetBool(animController.B_Attacking, true);
                anim.applyRootMotion = true;
                lookAtClosestTarget.RotateTowardsClosestEnemy();
                break;

            case CharacterState.Falling:
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                break;

            case CharacterState.Idling:
                break;

        }
    }

    public void OnStateExit(CharacterState exitedState)
    {
        //Debug.Log("Exited State:" + exitedState);

        //switch (exitedState)
        //{

        //}
    }
    public void ResetParameters()
    {
        anim.applyRootMotion = false;

        anim.SetBool(animController.B_Dashing, false);
        anim.SetBool(animController.B_Jumping, false);
        anim.SetBool(animController.B_Attacking, false);
        anim.SetBool(animController.B_canTransit, false);
        //
        rb.drag = drag;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var contactPoints = collision.contacts;
            foreach (var contact in contactPoints)
            {
                var yLength = GetComponent<CapsuleCollider>().bounds.center.y - contact.point.y;
                if (yLength > GetComponent<CapsuleCollider>().height / 2 - 0.01f)
                {
                    Debug.Log("happened");
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                }
            }
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

    public void OnCombo_1(InputValue value)
    {
        SendCombo_1(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

    public void OnCombo_2(InputValue value)
    {
        SendCombo_2(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

    #endregion

    #region Public Functions

    public void ControllerState(bool value)
    {
        PlayerInputs.enabled = value;
    }
    public void ColliderState(bool value)
    {
        this.enabled = value;
        GetComponent<Collider>().enabled = value;
    }

    #endregion
}
