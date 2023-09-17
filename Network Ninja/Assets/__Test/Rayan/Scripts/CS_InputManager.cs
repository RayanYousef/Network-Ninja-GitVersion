using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_InputManager : MonoBehaviour
{


    public static CS_InputManager Instance;
    public Action<Vector2> Move, RotateCamera;
    public Action<bool> Jump, Dash, Combo_1, Combo_2, Ultimate;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    #region Messages from Player Inputs
     void OnMove(InputValue value)
    {
        //SendInputDirection(value.Get<Vector2>());
        Move?.Invoke(value.Get<Vector2>());
        //Debug.Log("MoveTowardsDirection:" + value.Get<Vector2>());
    }

     void OnCameraRotation(InputValue value)
    {
        //SendInputRotation(value.Get<Vector2>());
        RotateCamera?.Invoke(value.Get<Vector2>());
        //Debug.Log("CamRotation:" + value.Get<Vector2>());
    }
     void OnJump(InputValue value)
    {
        //SendJumpInputState(value.isPressed);
        Jump?.Invoke(value.isPressed);
        //Debug.Log("SendJumpInputState:" + value.isPressed);
    }

     void OnDash(InputValue value)
    {
        //SendDashInputState(value.isPressed);
        Dash?.Invoke(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

     void OnCombo_1(InputValue value)
    {
        //SendCombo_1(value.isPressed);
        Combo_1?.Invoke(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

     void OnCombo_2(InputValue value)
    {
        //SendCombo_2(value.isPressed);
        Combo_2?.Invoke(value.isPressed);
        //Debug.Log("SendDashInputState:" + value.isPressed);
    }

     void OnUltimate(InputValue value)
    {
        Ultimate?.Invoke(value.isPressed);
        //ActivateUltimate(value.isPressed);
    }
    #endregion

}
