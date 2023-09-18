using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum DirectionType { Movement, CameraRotation }
public class DynamicJoystick : Joystick
{
    [SerializeField] public DirectionType directionType;
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;


    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        switch (directionType)
        {
            case DirectionType.Movement:
                CS_InputManager.Instance.OnMove(Vector2.zero);
                break;
            case DirectionType.CameraRotation:
                CS_InputManager.Instance.OnCameraRotation(Vector2.zero);
                break;
        }

    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
        switch (directionType)
        {
            case DirectionType.Movement:
                CS_InputManager.Instance.OnMove(Direction);
                break;
            case DirectionType.CameraRotation:
                CS_InputManager.Instance.OnCameraRotation(Direction * 20);
                break;
        }
    }
}