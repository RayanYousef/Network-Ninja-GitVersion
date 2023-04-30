using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.Rendering.RendererUtils;
using Unity.VisualScripting;

public enum AreaType { Base, Fight };

public class rArea : MonoBehaviour
{
    [Header("Area Info")]
    [SerializeField] AreaType areaType;
    [SerializeField] float health, maxHealth;
    [SerializeField] string password;
    [SerializeField] bool mine = false;
    [SerializeField] bool playerInside = false;

    [Header("Events")]
    [SerializeField] UnityEvent OnEnteringArea, OnEnteringFight;

    [Header("Area Components")]
    [SerializeField] Collider areaCollider;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ExampleArmy friendSpawner;

    [Header("MiniMap Components")]
    [SerializeField] CS_ChangeObjectsColour meshColourChanger;
    [SerializeField] SpriteRenderer sharingPasswordWarningIcon;


    [Header("Script Internal Variables")]
    [SerializeField] float healthTimer;

    public string Password { get => password; set => password = value; }
    public float Health
    {
        get => health;
        set
        {
            health = value;
            if (health == 0) LostArea();
        }
    }
    public bool Mine { get => mine; set => mine = value; }
    public AreaType AreaType { get => areaType; set => areaType = value; }
    public CS_ChangeObjectsColour MeshColourChanger { get => meshColourChanger; }
    public SpriteRenderer SharingPasswordWarningIcon { get => sharingPasswordWarningIcon; }

    private void Awake()
    {
        areaCollider = GetComponent<Collider>();
        sharingPasswordWarningIcon = GetComponentsInChildren<SpriteRenderer>()[0];
        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnEnemies);

        enemySpawner = GetComponentInChildren<EnemySpawner>();
        friendSpawner = GetComponentInChildren<ExampleArmy>();

        meshColourChanger.MaxHealth = rPasswordManager.Instance.MaxHealth;
        meshColourChanger.HalfHealth = rPasswordManager.Instance.HalfHealth;
        meshColourChanger.LowHealth = rPasswordManager.Instance.LowHealth;

        Renderer[] Renderers = new Renderer[1];
        Renderers[0] = GetComponentsInChildren<Renderer>()[1];
        meshColourChanger.MeshRenderers = Renderers;

        sharingPasswordWarningIcon.gameObject.SetActive(false);

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
            meshColourChanger.ChangeToColour(Color.red);
        }

        maxHealth = rPasswordManager.Instance.MaxSoldiersNumber;
    }

    private void FixedUpdate()
    {
        if (playerInside == false)
            UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthTimer += Time.deltaTime;
        if (healthTimer > 3)
        {
            healthTimer = 0;
            health = Mathf.Clamp(health - 1, 0, maxHealth);

            if (health > 0)
                meshColourChanger.LerpBetweenObjectColours(health / maxHealth);
            else
                meshColourChanger.ChangeToColour(Color.red);


            if (health == 0 && password != null)
            {
                rPasswordManager.Instance.OnHealthZeroDestroyAreasWithSamePassword(this);
                LostArea();
            }
        }
    }

    public void LostArea()
    {
        areaType = AreaType.Fight;
        areaCollider.isTrigger = true;
        password = null;
        sharingPasswordWarningIcon.gameObject.SetActive(false);
    }


    #region Collision and Trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<rTempFriendScript>(out rTempFriendScript friend))
        {
            Destroy(collision.gameObject);
        }

        if (areaType == AreaType.Base && collision.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = true;
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
        if (areaType == AreaType.Base)
            rPasswordManager.Instance.ResetPasswordButtonInteractbility(true);
        else
            rPasswordManager.Instance.ResetPasswordButtonInteractbility(false);

        if (areaType == AreaType.Fight && other.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = true;
            // raise event to spawn enemies
            rPasswordManager.Instance.CurrentArea = this;
            OnEnteringFight?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            rPasswordManager.Instance.ResetPasswordButtonInteractbility(false);
            foreach (GameObject friend in friendSpawner._spawnedUnits)
            {
                Destroy(friend.gameObject);
            }

            foreach (GameObject enemy in enemySpawner.enemies)
            {
                Destroy(enemy);
            }
            friendSpawner._spawnedUnits.Clear();
            enemySpawner.enemies.Clear();
        }

        if (areaType == AreaType.Base && other.gameObject == GameObjectsManager.Instance.Player)
        {
            areaCollider.isTrigger = false;
            playerInside = false;
            GetComponentInChildren<ExampleArmy>().enabled = false;
        }
    }
    #endregion
}
