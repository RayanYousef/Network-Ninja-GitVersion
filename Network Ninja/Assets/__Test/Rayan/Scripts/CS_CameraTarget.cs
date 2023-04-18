using Cinemachine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_CameraTarget : MonoBehaviour
{

    [Header("Camera Behavior Vars")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float targetRotationSmoothTime, minAngle, maxAngle;


    [Header("Necessary For The Script")]
    [SerializeField] CinemachineVirtualCamera playerVirtualCamera;
    Vector3 currentRotation, rotationRef;
    Vector2 deltaValues;
    float yaw, pitch;

    public Vector2 DeltaValues
    {
        //get => deltaValues;
        set => deltaValues = value;
    }

    private void LateUpdate()
    {
        RotateObject(deltaValues);
    }

    public void RotateObject(Vector2 mouseDelta)
    {

        // input info (Yaw, Pitch, Roll) XYZ
        pitch += mouseDelta.x * rotationSpeed * Time.deltaTime;
        yaw -= mouseDelta.y * rotationSpeed * Time.deltaTime;

        yaw = Mathf.Clamp(yaw, minAngle, maxAngle);

        currentRotation = Vector3.SmoothDamp(
            currentRotation,
            new Vector3(yaw, pitch, 0),
            ref rotationRef,
            targetRotationSmoothTime);

        //currentRotation = new Vector3(yaw, pitch, 0);

        transform.eulerAngles = currentRotation;
    }
}
