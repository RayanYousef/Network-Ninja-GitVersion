using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public enum AreaType { Base, Fight };

public class rArea : MonoBehaviour
{
    [SerializeField] public AreaType areaType;
    [SerializeField] public int areaID;

    [SerializeField] public string areaPassword;

    [SerializeField] private bool fightCompleted = false;

    public UnityEvent OnBaseFirstVisitOrFightCompleted;
    public UnityEvent OnAreaRevisited;

    public Collider areaCollider;

    private void Awake()
    {
        areaCollider = GetComponent<Collider>();
    }
    void Start()
    {
        areaID = GetInstanceID();
        areaPassword = null;
        areaCollider.isTrigger = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<FriendStates>(out FriendStates friend))
        {
            Destroy(friend.gameObject);
        }
            if (collision.gameObject == GameObjectsManager.Instance.Player)
        {
            rPasswordManager.Instance.CurrentArea = this;

            if (areaType == AreaType.Base)
            {
                if(areaPassword != null)
                {
                    // UI CheckPasswordPanel
                    OnAreaRevisited?.Invoke();
                }
                else
                {
                    // UI CreatePasswordPanel
                    OnBaseFirstVisitOrFightCompleted?.Invoke();
                }
            }
            else
            {
                if (fightCompleted)
                {
                    /// prompt the user to check the previously set password
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            areaCollider.isTrigger = false;

            //isInside = false;
            // TO DO
            // Disable the army in the area the player just left
        }
    }
}
