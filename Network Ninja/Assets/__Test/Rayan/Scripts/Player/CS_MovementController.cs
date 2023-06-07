
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class CS_MovementController : MonoBehaviour
{
    [Header("Components on this GameObject")]
    [SerializeField] Rigidbody rb;
    [SerializeField] CS_PlayerManager playerManager;

    [Header("Components on Other GameObjects")]
    [SerializeField] Camera playerCam;

    [Header("Dash Variables")]
    [SerializeField] float dashDuration;
    [SerializeField] float defaultDashForce, dashAttenuationRate;

    [Header("Jump Variables")]
    [SerializeField] float jumpDuration;
    [SerializeField] float defaultJumpForce, jumpAttenuation;

    [Header("Gravity Variables")]
    [SerializeField] float gravityAccumiliationForce;
    [SerializeField] float maxGravity,minGravity;

    [Header("Character Movement and Rotation Speed")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;


    [Header("Script Variables")]
    [SerializeField] Vector3 inputDirection;
    [SerializeField] float appliedGravityForce, appliedDashForce, appliedJumpForce, dashTimer, jumpTimer;


    public Vector3 InputDirection { get => inputDirection; set => inputDirection = value; }
    public float DashDuration { get => dashDuration; }
    public float AppliedGravityForce { get => appliedGravityForce; set => appliedGravityForce = value; }
    public CS_PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        // NOTE THAT THE Camera SHOULD BE UNDER THE SAME PARENT!
        playerCam = transform.parent.GetComponentInChildren<Camera>();

        playerManager = GetComponentInChildren<CS_PlayerManager>();

        dashTimer = jumpTimer = float.PositiveInfinity;
    }


    private void FixedUpdate()
    {
        switch (playerManager.CurrentState)
        {
            case CharacterState.Idling:
                break;

            case CharacterState.Running:
                MoveTowardsDirection(movementSpeed);
                break;
            case CharacterState.Attacking:
                MoveTowardsDirection(movementSpeed/8);
                break;

            case CharacterState.Dashing:
                MoveTowardsDirection(movementSpeed);
                DashUpdate();
                break;

            case CharacterState.Jumping:
                JumpUpdate();
                break;

            case CharacterState.Falling:
                MoveTowardsDirection(movementSpeed);
                break;

        }

        if (PlayerManager.enabled && playerManager.AnimController.Grounded == false && playerManager.CurrentState!= CharacterState.Dashing)
            FallingUpdate();

    }

    private void LateUpdate()
    {
        switch (playerManager.CurrentState == CharacterState.Ultimate && playerManager.CameraManager.LockedOn)
        {
            case true:
                RotateTowardsDirection(playerManager.CameraManager.LockedTarget);

                break;

            case false:
                if(playerManager.CurrentState != CharacterState.Ultimate)
                RotateTowardsDirection();
                break;
        }
    }

    public void FallingUpdate()
    {
        appliedGravityForce *=gravityAccumiliationForce;
        appliedGravityForce = Mathf.Clamp(appliedGravityForce,minGravity,maxGravity);
        rb.AddForce(-transform.up * appliedGravityForce * Time.deltaTime, ForceMode.Impulse);
    }

    #region Movement and Rotation
    void MoveTowardsDirection( float speed)
    {
        Vector3 newDirection = playerCam.transform.TransformDirection(inputDirection);
        newDirection.y = 0; newDirection.Normalize();
        rb.AddForce(newDirection * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void RotateTowardsDirection()
    {

        Vector3 newDirection = playerCam.transform.TransformDirection(inputDirection);
        newDirection.y = 0; newDirection.Normalize()    ;
        if (newDirection != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(newDirection), Time.deltaTime * rotationSpeed);
    }

    private void RotateTowardsDirection(Transform TargetObject)
    {

        if (playerManager.CameraManager.ListOfTargetsInRange.Count > 0)
        {
            Vector3 direction = TargetObject.position - transform.position;
            direction.y = 0; direction.Normalize();
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    #endregion

    #region Dash and Jump
    public void Dash()
    {
        // Reset Variables
        appliedDashForce = defaultDashForce;
        dashTimer = 0;

    }
    public void DashUpdate()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer < dashDuration)
        {
            rb.AddForce(transform.forward * appliedDashForce * Time.deltaTime, ForceMode.Impulse);
            playerManager.AnimController.SetAnimationMotion(Mathf.Clamp(dashTimer / dashDuration, 0, 0.3f));
        }
        else
        {
            playerManager.AnimController.NegateDashing();
            return;
        }
        if (appliedDashForce > 0)
            appliedDashForce -= dashAttenuationRate;
    }


    public void Jump()
    {
        // Reset Variables
        appliedJumpForce = defaultJumpForce;
        jumpTimer = 0;
        rb.drag = 0;
        appliedGravityForce = 1;
    }
    public void JumpUpdate()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer < jumpDuration)
        {
            rb.AddForce(transform.up * appliedJumpForce * Time.deltaTime, ForceMode.Impulse);
            playerManager.AnimController.SetAnimationMotion(Mathf.Clamp(jumpTimer / jumpDuration, 0, 1));

        }
        else
        {
            playerManager.AnimController.NegateJumping();
            return;
        }
        if (appliedJumpForce > 0)
            appliedJumpForce -= jumpAttenuation;
    }
    #endregion


  
}
