using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public enum AreaType { Base, Fight };



public class rArea : MonoBehaviour
{
    [Header("Area Info")]
    [SerializeField] public AreaType areaType;
    [SerializeField] public int areaID;
    [SerializeField] public string Password;

    [SerializeField] private bool fightCompleted = false;

    [SerializeField] UnityEvent OnEnteringArea;

    public Collider areaCollider;

    [Header("MiniMap Components")]
    SpriteRenderer areaMapUI, passwordSharedUI;


    private void Awake()
    {
        areaCollider = GetComponent<Collider>();
        areaMapUI = GetComponentsInChildren<SpriteRenderer>()[0];
        passwordSharedUI = GetComponentsInChildren<SpriteRenderer>()[1];
    }
    void Start()
    {
        areaID = GetInstanceID();
        Password = null;
        areaCollider.isTrigger = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<FriendStates>(out FriendStates friend))
        {
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject == GameObjectsManager.Instance.Player)
        {
            rPasswordManager.Instance.CurrentArea = this;

            if (areaType == AreaType.Base)
            {
                /// On Entering Area call On Entering Area in UIPassword
                OnEnteringArea?.Invoke();
            }
            else
            {
                if (fightCompleted)
                {
                    /// prompt the user to create a password
                }
                else
                {
                    areaCollider.isTrigger = true;
                    // spawn enemies
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            areaCollider.isTrigger = false;

            if(areaType == AreaType.Base)
            {
                GetComponentInChildren<ExampleArmy>().enabled = false;
            }
        }
    }
}
