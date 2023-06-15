using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CS_PlayerManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent FirstButtonClicked,SecondButtonClicked;

    [Header("Components")]
    public GameObject PlayerTopMostParent;
    public CS_HitEffect[] HitEffects;
    [SerializeField] GameObject particleForTransition,playerModel;
    [SerializeField] Animator anim;
    [SerializeField] CS_MovementController moveController;
    [SerializeField] CS_AnimatorController animController;
    [SerializeField] CS_RotateTowardTarget lookAtClosestTarget;
    [SerializeField] CS_CameraManager cameraManager;
    [SerializeField] CS_FreezeObjectsInRange _freezeObjectsInRange;
    [SerializeField] StatsManager pStatsManager;
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerInput PlayerInputs;
    [SerializeField] FixedJoystick joyStick;

    [Header("Ultimate")]
    [SerializeField] bool ultimateOn;
    [SerializeField] float ultimateAttenuationRate;
    [SerializeField] float energyRecoveryOnHit, energyRecoveryOnKill, ultimateCoolDown, ultimateAttackSpeed;

    [Header("Other Variables")]
    [SerializeField] float drag;
    [SerializeField] float clicksIntervalTime;
    [Header("Take Damage Variables")]
    // Controlls the damage taken so it wont happen a lot, but rather every interval
    [SerializeField] float intervalsBetweenDamage=0.5f;
    [SerializeField] float damageTimer;

    [Header("Script Variables")]
    [SerializeField] CharacterState currentState;
    float clicksIntervalTimer;
    bool AndroidBuild = false;

    public CharacterState CurrentState { get => currentState; }
    public Animator Anim { get => anim; }
    public CS_MovementController MoveController { get => moveController; }
    public CS_AnimatorController AnimController { get => animController; }
    public CS_CameraManager CameraManager { get => cameraManager; }
    public Rigidbody Rb { get => rb; }
    public StatsManager PStatsManager { get => pStatsManager; set => pStatsManager = value; }
    public bool UltimateOn
    {
        get => ultimateOn;
        set
        {
                switch (value)
                {
                case true:
                    EnableUltimate();
                    break;

                case false:
                    DisableUltimate();
                    break;
                }
        }
    }

    private void Awake()
    {
        if (GameObjectsManager.Instance != null)
        {
            GameObjectsManager.Instance.Player = this.gameObject;
        }

        // Top Most parent of Player
        if (PlayerTopMostParent == null)
            PlayerTopMostParent = gameObject;
        // Stats Manager
        if (pStatsManager == null) pStatsManager = GetComponentInChildren<StatsManager>();

        // Get components
        anim = PlayerTopMostParent.GetComponentInChildren<Animator>();
        moveController = PlayerTopMostParent.GetComponentInChildren<CS_MovementController>();
        animController = PlayerTopMostParent.GetComponentInChildren<CS_AnimatorController>();
        lookAtClosestTarget = PlayerTopMostParent.GetComponentInChildren<CS_RotateTowardTarget>();
        _freezeObjectsInRange = PlayerTopMostParent.GetComponentInChildren<CS_FreezeObjectsInRange>();

        // Camera Manager
        if (cameraManager == null)
            cameraManager = PlayerTopMostParent.GetComponentInChildren<CS_CameraManager>();
        cameraManager.PlayerManager = this;

        // Get Rigidbody
        rb = PlayerTopMostParent.GetComponentInChildren<Rigidbody>();
        moveController.PlayerManager = this;
        animController.PlayerManager = this;
        // Get Player Inputs
        PlayerInputs = GetComponent<PlayerInput>();

        HitEffects = PlayerTopMostParent.GetComponentsInChildren<CS_HitEffect>();


    }

    private void Start()
    {
        if (GameObjectsManager.Instance!=null)
            GameObjectsManager.Instance.Player = gameObject;

        pStatsManager.Stats.OnHealthUpdated.AddListener(LostGameHealthZero);
        pStatsManager.Stats.OnEnergyUpdated.AddListener(DisableUltimate);
        pStatsManager.OnTakingDamage.AddListener(ResetDamageTimerAndCanNotTakeDamage);
        pStatsManager.OnTakingDamage.AddListener(AnimController.PlayTakeDamageAnim);
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


        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //    Cursor.lockState = CursorLockMode.None;
        //if (Input.GetKeyUp(KeyCode.LeftAlt))
        //    Cursor.lockState = CursorLockMode.Locked;

    }

    private void FixedUpdate()
    {
        if (ultimateOn == true)
            pStatsManager.AddtoEnergy(-Time.deltaTime * ultimateAttenuationRate);

        DamageIntervalFunction();

    }

 

    #region Animator States
    public void OnStateEnter(CharacterState enteredState)
    {
        currentState = enteredState;

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
                moveController.RotateTowardsDirectionFaster();
                lookAtClosestTarget.RotateTowardsClosestEnemy();
                break;

            case CharacterState.Falling:
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                break;

            case CharacterState.Idling:
                break;

            case CharacterState.Ultimate:
                anim.SetBool(animController.B_Attacking, true);
                anim.applyRootMotion = false;
                if(cameraManager.LockedOn)
                lookAtClosestTarget.RotateTowardsClosestEnemy();
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

        if (CurrentState != CharacterState.Attacking)
            animController.ResetCombo();

        anim.applyRootMotion = false;

        anim.SetBool(animController.B_Dashing, false);
        anim.SetBool(animController.B_Jumping, false);
        anim.SetBool(animController.B_Attacking, false);
        anim.SetBool(animController.B_canTransit, false);

        anim.ResetTrigger(animController.T_Ultimate);
        anim.ResetTrigger(animController.T_Dash);

        pStatsManager.DisableAllWeapons();
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
                    //  rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                }
            }
        }
    }
    #endregion

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
        cameraManager.DeltaValues = value;
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
            if (ultimateOn) anim.SetTrigger(animController.T_Ultimate);
        }
    }

    public void ActivateUltimate(bool value)
    {
        
        if (value == true && ultimateOn == false && pStatsManager.Stats.Energy>0)
            UltimateOn = true;
        else if(value == true && ultimateOn) 
            UltimateOn = false;
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

    public void OnUltimate(InputValue value)
    {
        ActivateUltimate(value.isPressed);
    }
    #endregion

    #region Public Functions

    public void ParticleModeEnabled()
    {
        playerModel.SetActive(false);
        particleForTransition.SetActive(true);
    }
    public void ParticleModeDisabled()
    {
        playerModel.SetActive(true);
        particleForTransition.SetActive(false);
    }
    public void LostGameHealthZero(float value)
    {
        if (value <= 0 && GameManager.Instance.CurrentGameState == GameState.InProgress)
            GameManager.Instance.CurrentGameState = GameState.Lost;
    }
    public void RecoverEnergy(Collider other)
    {
        other.TryGetComponent<StatsManager>(out StatsManager otherStatsManager);

        if (otherStatsManager != null && ultimateOn==false)
        {
            switch (otherStatsManager.Stats.CurrentHealth)
            {
                case 0:
                    pStatsManager.AddtoEnergy(energyRecoveryOnKill);
                    break;

                default:
                    pStatsManager.AddtoEnergy(energyRecoveryOnHit);
                    break;

            }
        }

    }
    public void ControllerState(bool value)
    {
        PlayerInputs.enabled = value;
    }
    public void UltimateDisabled(bool value)
    {
        ultimateOn = true;
        anim.SetBool(animController.B_Ultimate, value);
        anim.SetFloat(animController.F_animSpeed, 0.9f);
        cameraManager.SwitchCamerasBasedOnLockState();
        if (_freezeObjectsInRange != null)
        {
            _freezeObjectsInRange.CheckInActiveObjectsAndRemoveIt();
            _freezeObjectsInRange.ObjectsMovementEnabled(true, 1);
        }
        if (AudioManager.instance.BossMusic != null)
            AudioManager.instance.BossMusic.InCombat = value;
    }
    private void EnableUltimate()
    {
        ultimateOn = true;
        anim.SetBool(animController.B_Ultimate, true);
        anim.SetFloat(animController.F_animSpeed, ultimateAttackSpeed);
        cameraManager.DisableAllCamerasExceptParam(cameraManager.UltimateCamera);
        if (_freezeObjectsInRange != null)
        {
            _freezeObjectsInRange.CheckInActiveObjectsAndRemoveIt();
            _freezeObjectsInRange.ObjectsMovementEnabled(false, 0.05f);
        }
        if (AudioManager.instance.BossMusic != null)
            AudioManager.instance.BossMusic.InCombat = true;
    }
    public void DisableUltimate()
    {
        ultimateOn= false;
        anim.SetBool(animController.B_Ultimate, false);
        anim.SetFloat(animController.F_animSpeed, 0.9f);
        cameraManager.SwitchCamerasBasedOnLockState();
        if (_freezeObjectsInRange != null)
        {
            _freezeObjectsInRange.CheckInActiveObjectsAndRemoveIt();
            _freezeObjectsInRange.ObjectsMovementEnabled(true, 1);
        }
        if (AudioManager.instance.BossMusic != null)
            AudioManager.instance.BossMusic.InCombat = false;
    }
    public void ColliderState(bool value)
    {
        this.enabled = value;
        GetComponent<Collider>().enabled = value;
    }
    public void GravityState(bool value)
    {
        MoveController.Gravity= value;
    }
    public void ResetDamageTimerAndCanNotTakeDamage()
    {
        damageTimer= 0;
        pStatsManager.Damagable = false;

    }
    #endregion

    #region Private Functions
  
    private void DisableUltimate(float energyValue)
    {
        if (energyValue == 0)
            UltimateOn = false;
    }


    private void DamageIntervalFunction()
    {
        if (intervalsBetweenDamage > damageTimer)
            damageTimer += Time.deltaTime;
        else PStatsManager.Damagable = true;
    }
    #endregion
}
