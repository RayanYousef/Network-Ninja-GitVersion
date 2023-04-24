using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public enum AreaType { Base, Fight };

public class rArea : MonoBehaviour
{
    [Header("Area Info")]
    [SerializeField] AreaType areaType;
    [SerializeField] int health, maxHealth;
    [SerializeField] string password;
    [SerializeField] bool mine = false;

    [Header("Events")]
    [SerializeField] UnityEvent OnEnteringArea, OnEnteringFight;

    [Header("Area Components")]
    [SerializeField] Collider areaCollider;

    [Header("MiniMap Components")]
    [SerializeField] CS_ChangeObjectsColour areaMiniMap;
    [SerializeField] SpriteRenderer passwordSharedUI;


    [Header("Script Internal Variables")]
    [SerializeField] float healthTimer;

    public string Password { get => password; set => password = value; }
    public int Health { get => health; set => health = value; }
    public bool Mine { get => mine; set => mine = value; }
    public AreaType AreaType { get => areaType; set => areaType = value; }

    private void Awake()
    {
        areaCollider = GetComponent<Collider>();
        passwordSharedUI = GetComponentsInChildren<SpriteRenderer>()[0];

        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnEnemies);
    }
    void Start()
    {
        Password = null;
        if (areaType == AreaType.Base)
        {
            areaCollider.isTrigger = false;
        }
        else
        {
            areaCollider.isTrigger = true;
        }
  
        maxHealth = rPasswordManager.Instance.MaxSoldiersNumber;
    }

    private void FixedUpdate()
    {
        if(rPasswordManager.Instance.CurrentArea!=this)
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthTimer += Time.deltaTime;
        if (healthTimer > 1.5)
        {
            healthTimer = 0;
            health = Mathf.Clamp(health - 1, 0, maxHealth);
            if (health == 0 && password!=null)
            {
                areaType = AreaType.Fight;
                areaCollider.isTrigger = true;

                password = null;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<FriendStates>(out FriendStates friend))
        {
            Destroy(collision.gameObject);
        }

        if (areaType == AreaType.Base && collision.gameObject == GameObjectsManager.Instance.Player)
        {
            rPasswordManager.Instance.CurrentArea = this;

            /// On Entering Area call On Entering Area in UIPassword
            OnEnteringArea?.Invoke();

            //if (areaType == AreaType.Base)
            //{
            //    /// On Entering Area call On Entering Area in UIPassword
            //    OnEnteringArea?.Invoke();
            //}
            //else
            //{
            //    if (mine)
            //    {
            //        areaCollider.isTrigger = true;
            //    }
            //    else
            //    {
    
            //        // spawn enemies
            //    }
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(areaType == AreaType.Fight && other.gameObject == GameObjectsManager.Instance.Player)
        {
            // raise event to spawn enemies
            rPasswordManager.Instance.CurrentArea = this;
            OnEnteringFight?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (areaType == AreaType.Base && other.gameObject == GameObjectsManager.Instance.Player)
        {
            areaCollider.isTrigger = false;

            GetComponentInChildren<ExampleArmy>().enabled = false;
        }
    }
}
