using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CS_PathButton : MonoBehaviour   , IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] static bool isPressed;
    [SerializeField] static GameObject pathButton;


    public static bool IsPressed => isPressed;
    public static GameObject PathButton => pathButton;

    private void Awake()
    {
        pathButton = gameObject;
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
