using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CS_CameraManager : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] CinemachineBrain cameraBrain;
    [SerializeField] CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lockVirtualCamera;


    [Header("Targets To Follow")]
    [SerializeField] Transform followTargetNormal;
    [SerializeField] Transform followTargetLock;

    [Header("Lock Function Vars")]
    [SerializeField] List<GameObject> listOfTargets = new List<GameObject>();
    [SerializeField] Transform lockTarget;
    [SerializeField] int targetIndex;
    [SerializeField] float lockRotationSpeed,lockTimer;
    [SerializeField] bool lockedOn;

    [Header("Camera Behavior Vars")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float /*targetRotationSmoothTime,*/ minAngle, maxAngle;


    [Header("Necessary For The Script")]
    [SerializeField] Vector3 currentRotation, rotationRef;
    [SerializeField] Vector2 deltaValues;
    [SerializeField] Quaternion QuaternionRotation;
    [SerializeField] float yaw, pitch;

    public Vector2 DeltaValues
    { set => deltaValues = value; }
    public bool LockedOn
    {
        get => lockedOn;

        set
        {
            lockedOn = value;
            lockTimer = 0;
            switch (lockedOn)
            {
                case true:
                    if (listOfTargets.Count > 0)
                    {
                        lockVirtualCamera.enabled = true;
                        mainVirtualCamera.enabled = false;

                        lockTarget = listOfTargets[targetIndex % listOfTargets.Count].transform;
                    }
                    else lockedOn= false;
                    break;

                case false:
                    mainVirtualCamera.enabled = true;
                    lockVirtualCamera.enabled = false;
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) { LockedOn = !LockedOn; }
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            targetIndex++;
            if(listOfTargets.Count>0)
            lockTarget = listOfTargets[targetIndex % listOfTargets.Count].transform;
        }

    }
    private void FixedUpdate()
    {

        followTargetLock.position = followTargetNormal.position = transform.position;

        if (LockedOn && listOfTargets.Count>0)
        {
            //followTargetLock.LookAt(lockTarget);
            followTargetLock.rotation= Quaternion.RotateTowards( 
                followTargetLock.rotation,
                Quaternion.LookRotation(lockTarget.position - followTargetLock.position),
                lockRotationSpeed*Time.deltaTime);

            if (lockTimer>cameraBrain.m_DefaultBlend.BlendTime)
                followTargetNormal.rotation = followTargetLock.rotation;
            lockTimer += Time.deltaTime;
        }
        else LockedOn= false;

        for(int i = listOfTargets.Count-1; i>=0; i--)
        {
            if (listOfTargets[i].activeInHierarchy==false) listOfTargets.Remove(listOfTargets[i]);

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

        float mouseX = mouseDelta.x * rotationSpeed * Time.deltaTime;
        float mouseY = mouseDelta.y * rotationSpeed * Time.deltaTime;

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

        transform.rotation = Quaternion.Euler(new Vector3(pitch, rotation.eulerAngles.y,0));
    }

    #endregion

    #region TriggerEnter/Exit

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy) && !listOfTargets.Contains(other.gameObject))
            listOfTargets.Add(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy) && listOfTargets.Contains(other.gameObject))
        {
            listOfTargets.Remove(other.gameObject);
            if (listOfTargets.Count < 1)
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
