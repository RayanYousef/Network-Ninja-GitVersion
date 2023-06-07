using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CS_CameraManager : MonoBehaviour
{
    [Header("Player Manager")]
    public CS_PlayerManager PlayerManager;

    [Header("Cinemachine")]
    [SerializeField] CinemachineBrain cameraBrain;
    [SerializeField] List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();
    [SerializeField] CinemachineVirtualCamera mainVirtualCamera, lockVirtualCamera, ultimateCamera;
    [SerializeField] CinemachineImpulseSource _impulseSource;


    [Header("Targets To Follow")]
    [SerializeField] Transform followTargetNormal;
    [SerializeField] Transform followTargetLock;

    [Header("Lock Function Vars")]
    [SerializeField] List<GameObject> listOfTargetsInRange = new List<GameObject>();
    [SerializeField] Transform lockedTarget;
    [SerializeField] int targetIndex;
    [SerializeField] float lockRotationSpeed,lockTimer;
    [SerializeField] bool lockedOn;

    [Header("Camera Behavior Vars")]
    [SerializeField] float cinemachineLerpTime;
    [SerializeField, Range(0,300)] float rotationSpeed;
    [SerializeField] float minAngle, maxAngle;


    [Header("Necessary For The Script")]
    [SerializeField] Vector3 currentRotation, rotationRef;
    [SerializeField] Vector2 deltaValues;
    [SerializeField] Quaternion QuaternionRotation;
    [SerializeField] float yaw, pitch;

    #region Setters and Getters
    public Vector2 DeltaValues { set => deltaValues = value; }
    public Transform LockedTarget { get => lockedTarget; }
    public List<GameObject> ListOfTargetsInRange { get => listOfTargetsInRange; }
    public CinemachineVirtualCamera MainVirtualCamera { get => mainVirtualCamera; }
    public CinemachineVirtualCamera LockVirtualCamera { get => lockVirtualCamera; }
    public CinemachineVirtualCamera UltimateCamera { get => ultimateCamera; }

    public bool LockedOn
    {
        get => lockedOn;

        set
        {
            lockedOn = value;
            lockTimer = 0;

            switch (PlayerManager.UltimateOn) 
            {
                case true:

                    if (value == true && listOfTargetsInRange.Count > 0)
                        ultimateCamera.Follow = followTargetLock;
                    else
                        ultimateCamera.Follow = followTargetNormal;
                    break;

                case false:
                    SwitchCamerasBasedOnLockState();
                        break;


            }

        }
    }


    #endregion

    private void Awake()
    {
        _impulseSource= GetComponentInChildren<CinemachineImpulseSource>();
    }
    private void Start()
    {
        foreach(CinemachineVirtualCamera camera in PlayerManager.PlayerTopMostParent.GetComponentsInChildren<CinemachineVirtualCamera>())
            virtualCameras.Add(camera);
    }

    private void Update()   
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            LockedOn = !LockedOn; 
        }
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            targetIndex++;
            SetTarget();
        }

    }
    private void FixedUpdate()
    {

        followTargetLock.position = followTargetNormal.position = transform.position;
        
        if (LockedOn && listOfTargetsInRange.Count>0)
        {

            if (lockedTarget == null)
                SetTarget();

            if(lockedTarget.gameObject.activeInHierarchy==false &&listOfTargetsInRange.Contains(lockedTarget.gameObject))
            {
                listOfTargetsInRange.Remove(lockedTarget.gameObject);
                if (listOfTargetsInRange.Count < 1)
                    LockedOn = false;
                else SetTarget();
            }

            followTargetLock.rotation= Quaternion.RotateTowards( 
                followTargetLock.rotation,
                Quaternion.LookRotation(lockedTarget.position - followTargetLock.position),
                lockRotationSpeed*Time.deltaTime);

            if (lockTimer>cameraBrain.m_DefaultBlend.BlendTime)
                followTargetNormal.rotation = followTargetLock.rotation;
            lockTimer += Time.deltaTime;
        }
        else LockedOn= false;

        for(int i = listOfTargetsInRange.Count-1; i>=0; i--)
        {
            if (listOfTargetsInRange[i].activeInHierarchy==false) listOfTargetsInRange.Remove(listOfTargetsInRange[i]);

        }

        #region Old rotation using euler
        //if (lockTarget)
        //{
        //    rotateTowardsProgress = rotateTowardsProgress + Time.deltaTime;
        //    rotateTowardsProgress *= lookAtSpeed;
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookAtTarget.position - transform.position), rotateTowardsProgress);

        //    pitch = Mathf.Abs(transform.eulerAngles.x);
        //    yaw = transform.eulerAngles.y;

        //    if (pitch > 360 + minAngle && pitch < 360 + maxAngle)
        //        pitch = Mathf.Abs(transform.eulerAngles.x) - 360;

        //}
        //else
        //    RotateObjectQuaternion(deltaValues); 

        ////////////////////////////////////////////
        //if (!LockedAtTarget)
        //{
        //    AdditionRotationToPitchAndYaw(deltaValues);
        //}
        //else
        //{
        //if (targetToLockAt != null)
        //    {
        //        followTargetLock.rotation = Quaternion.RotateTowards(followTargetLock.rotation,
        //            Quaternion.LookRotation(targetToLockAt.position - followTargetLock.position),
        //            lockSpeed * Time.deltaTime);
        //    }
        //}
        /////////////////////////////////////////////
        #endregion

    }
    private void LateUpdate()
    {
        if (lockedOn) return;
            RotateObjectQuaternionClamping(deltaValues, followTargetNormal);
    }

 
    #region Main Rotation Function

    public void RotateObjectQuaternionClamping(Vector2 mouseDelta, Transform transform)
    {

        //float mouseX = mouseDelta.x * rotationSpeed * Time.deltaTime;
        //float mouseY = mouseDelta.y * rotationSpeed * Time.deltaTime;

        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        float pitch = -mouseY;
        float yaw = mouseX;

        Quaternion rotation = transform.rotation * Quaternion.Euler(pitch, yaw, 0f);

        switch (rotation.eulerAngles.x > 300)
        {
            case true:
                pitch = Mathf.Clamp(rotation.eulerAngles.x, 360 + minAngle, 360);
                break;
            case false:
                pitch = Mathf.Clamp(rotation.eulerAngles.x, 0, maxAngle);
                break;
        }

        //transform.rotation = Quaternion.Euler(new Vector3(pitch, rotation.eulerAngles.y,0));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(pitch, rotation.eulerAngles.y, 0)), Time.deltaTime * rotationSpeed);
    }

    #endregion

    #region Other Functions

    private void SetTarget()
    {

        // On Destroy Remove Target from the list or else a null reference will find his way to you.
        if (listOfTargetsInRange.Count > 0 && listOfTargetsInRange[targetIndex % listOfTargetsInRange.Count] != null)
            lockedTarget = listOfTargetsInRange[targetIndex % listOfTargetsInRange.Count].transform;
    }

    public void DisableAllCamerasExceptParam(CinemachineVirtualCamera ExcludedCamera)
    {

        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            camera.enabled = false;
        }
        ExcludedCamera.enabled = true;
    }

    public void SwitchCamerasBasedOnLockState()
    {
        switch (lockedOn)
        {
            case true:

                if (listOfTargetsInRange.Count > 0)
                {
                    cameraBrain.m_DefaultBlend.m_Time = cinemachineLerpTime;
                    DisableAllCamerasExceptParam(lockVirtualCamera);
                    SetTarget();
                }
                else
                {
                    lockedOn = false;
                    if (mainVirtualCamera.enabled == false)
                        DisableAllCamerasExceptParam(mainVirtualCamera);
                }
                break;


            case false:
                cameraBrain.m_DefaultBlend.m_Time = cinemachineLerpTime;
                if(mainVirtualCamera.enabled==false)
                DisableAllCamerasExceptParam(mainVirtualCamera);
                break;
        }
    }

    public void ApplyShakeOnTakingDamage()
    {
        _impulseSource.GenerateImpulseWithVelocity(new Vector3(0,-1,0));
    }

    public void ApplyShakeOnDealingDamage()
    {
        _impulseSource.GenerateImpulseWithVelocity(new Vector3(0, 0.2f, 0));
    }

    public void SetDistanceOfCamera(float value)
    {
        mainVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = value;
    }
    public void RotationSpeed( float value)
    {
        rotationSpeed = value * 300;
    }
    #endregion

    #region TriggerEnter/Exit

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy) && !listOfTargetsInRange.Contains(other.gameObject))
        {
            if (enemy.Team == CharacterTeam.Enemy && enemy.Targetable)
            {
                listOfTargetsInRange.Add(other.gameObject);
                SetTarget();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy) && listOfTargetsInRange.Contains(other.gameObject))
        {
            listOfTargetsInRange.Remove(other.gameObject);
            if (listOfTargetsInRange.Count < 1)
                LockedOn = false;
        }
    }

    #endregion
        
    //#region Old Rotation Functions
    //public void RotateAround(Vector2 mouseDelta)
    //{

    //    // input info (Yaw, Pitch, Roll) XYZ
    //    float pitch = mouseDelta.x * rotationSpeed * Time.deltaTime;
    //    float yaw = mouseDelta.y * rotationSpeed * Time.deltaTime;

    //    yaw = Mathf.Clamp(yaw, minAngle, maxAngle);

    //    currentRotation = Vector3.SmoothDamp(
    //        currentRotation,
    //        new Vector3(yaw, pitch, 0),
    //        ref rotationRef,
    //        targetRotationSmoothTime);

    //    transform.RotateAround(transform.position, transform.up, pitch * rotationSpeed * Time.deltaTime); ;
    //    transform.RotateAround(transform.position, transform.right, -yaw * rotationSpeed * Time.deltaTime);
    //}
    //public void RotateObject(Vector2 mouseDelta)
    //{

    //    // input info (Yaw, Pitch, Roll) XYZ
    //    pitch += mouseDelta.x * rotationSpeed * Time.deltaTime;
    //    yaw -= mouseDelta.y * rotationSpeed * Time.deltaTime;

    //    yaw = Mathf.Clamp(yaw, minAngle, maxAngle);

    //    currentRotation = Vector3.SmoothDamp(
    //        currentRotation,
    //        new Vector3(yaw, pitch, 0),
    //        ref rotationRef,
    //        targetRotationSmoothTime);

    //    //currentRotation = new Vector3(yaw, pitch, 0);

    //    transform.eulerAngles = currentRotation;
    //}

    //public void RotateObjectPitchYaw(Vector2 mouseDelta, Transform transform)
    //{

    //    // input info (Pitch,Yaw, Roll) XYZ
    //    pitch -= mouseDelta.y * rotationSpeed * Time.deltaTime;
    //    yaw += mouseDelta.x * rotationSpeed * Time.deltaTime;

    //    pitch = Mathf.Clamp(pitch, minAngle, maxAngle);

    //    currentRotation = Vector3.SmoothDamp(
    //        currentRotation,
    //        new Vector3(pitch, yaw, 0),
    //        ref rotationRef,
    //        targetRotationSmoothTime);

    //    transform.rotation = Quaternion.Euler(currentRotation);
    //}

    //public void RotateObjectPitchYaw(Transform transform)
    //{
    //    pitch = Mathf.Clamp(pitch, minAngle, maxAngle);

    //    currentRotation = new Vector3(yaw, pitch, 0);
    //    transform.rotation = Quaternion.Euler(currentRotation);
    //}

    //#region Rotation Times
    ////public void MouseDeltaQuaternionRotation(Vector2 mouseDelta)
    ////{

    ////    // input info (Pitch,Yaw, Roll) XYZ
    ////    pitch = mouseDelta.y * rotationSpeed * Time.deltaTime;
    ////    yaw = mouseDelta.x * rotationSpeed * Time.deltaTime;

    ////    pitch = Mathf.Clamp(pitch, minAngle, maxAngle);

    ////    currentRotation = Vector3.SmoothDamp(
    ////        currentRotation,
    ////        new Vector3(pitch, yaw, 0),
    ////        ref rotationRef,
    ////        targetRotationSmoothTime);

    ////    //currentRotation = new Vector3(yaw, pitch, 0);
    ////    Debug.Log(Quaternion.Euler(currentRotation));
    ////    transform.rotation *= Quaternion.Euler(currentRotation);
    ////} 
    //#endregion

    //#endregion


}
