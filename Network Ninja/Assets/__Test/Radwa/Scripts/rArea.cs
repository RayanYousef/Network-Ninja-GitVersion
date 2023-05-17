using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Cinemachine;

public enum AreaType { Base, Fight };

public class rArea : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera areaCamera;
    [SerializeField] bool switchCamBack = true;

    [Header("Area Info")]
    [SerializeField] AreaType areaType;
    [SerializeField] float health, maxHealth;
    bool isWinningConditionMet = false;
    [SerializeField] string password;
    [SerializeField] bool playerInside = false;

    [Header("Events")]
    [SerializeField] UnityEvent OnEnteringArea, OnEnteringFight, OnWinningLvl;

    [Header("Area Components")]
    [SerializeField] Collider areaCollider;
    EnemySpawner enemySpawner;

    [SerializeField] private Transform[] alliesSpawnPos;
    private Formation alliesSpawnerPrefab;
    [SerializeField] private List<FormationAgent> alliesList = new List<FormationAgent>();

    private int numbOfAlliesInBattalion = 25;

    [Header("MiniMap Components")]
    [SerializeField] CS_ChangeObjectsColour meshColourChanger;
    [SerializeField] SpriteRenderer sharingPasswordWarningIcon;


    [Header("Script Internal Variables")]
    [SerializeField] float healthTimer;
    [SerializeField] float formationRemovingTimer;

    Formation allies1, allies2, allies3;

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
    public AreaType AreaType { get => areaType; set => areaType = value; }
    public CS_ChangeObjectsColour MeshColourChanger { get => meshColourChanger; }
    public SpriteRenderer SharingPasswordWarningIcon { get => sharingPasswordWarningIcon; }

    private void Awake()
    {
        if (areaCamera != null) { }
        else
        {
            areaCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        areaCollider = GetComponent<Collider>();
        sharingPasswordWarningIcon = GetComponentsInChildren<SpriteRenderer>()[0];
        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnMiniBosses);
        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnEnemies);

        enemySpawner = GetComponentInChildren<EnemySpawner>();

        alliesSpawnerPrefab = GameObjectsManager.Instance.AllyBatalionPrefab;

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

        enemySpawner.OnBigBossKilled.AddListener(Winning);
    }

    private void FixedUpdate()
    {
        if (playerInside == false && password != null && !isWinningConditionMet)
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

        DestroyAllAllies();
    }

    public void Winning()
    {
        // stop update health
        isWinningConditionMet = true;

        // current area minimap ..> max health color
        meshColourChanger.ChangeToColour(rPasswordManager.Instance.MaxHealth);
        health = rPasswordManager.Instance.MaxSoldiersNumber;

        // Allies Formation
        FormStrongArmy();

        areaCamera.enabled = true;
        //StartCoroutine(WinningCutScene());
    }

    #region Allies Handling Functions
    public void DestroyAllAllies()
    {
        foreach (Transform asp in alliesSpawnPos)
        {
            Formation battalion = asp.GetComponentInChildren<Formation>();
            if (battalion != null)
            {
                Destroy(battalion.gameObject);
            }
        }
    }
    public void ShowAlliesBasedOnAreaHealth()
    {
        for (int i = 0; i < health; i++)
        {
            alliesList[i].gameObject.SetActive(true);
        }
    }
    public void FormArmyBasedOnAreaHealth()
    {
        if (Health <= maxHealth / 4)
        {
            allies1 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[0].position, Quaternion.identity);
            allies1.transform.parent = alliesSpawnPos[0];

            alliesList = allies1.AgentsList;
        }

        else if (Health <= maxHealth / 2)
        {
            allies1 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[0].position, Quaternion.identity);
            allies1.transform.parent = alliesSpawnPos[0];

            allies2 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[1].position, Quaternion.identity);
            allies2.transform.parent = alliesSpawnPos[1];

            alliesList = allies1.AgentsList.Concat(allies2.AgentsList).ToList();

        }

        else if (Health <= maxHealth)
        {
            FormStrongArmy();
        }

        areaCamera.enabled = true;
        if(switchCamBack)
            StartCoroutine(WaitAndSwitchCameraBack());
    } 

    private void FormStrongArmy()
    {
        allies1 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[0].position, Quaternion.identity);
        allies1.transform.parent = alliesSpawnPos[0];

        allies2 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[1].position, Quaternion.identity);
        allies2.transform.parent = alliesSpawnPos[1];

        allies3 = Instantiate(alliesSpawnerPrefab, alliesSpawnPos[2].position, Quaternion.identity);
        allies3.transform.parent = alliesSpawnPos[2];

        alliesList = allies1.AgentsList.Concat(allies2.AgentsList.Concat(allies3.AgentsList)).ToList();
    }
    #endregion

    #region Cinemachine Cut Scene
    IEnumerator WaitAndSwitchCameraBack()
    {
        yield return new WaitForSeconds(5.0f);
        areaCamera.enabled = false;
    }

    IEnumerator WinningCutScene()
    {
        yield return new WaitForSeconds(5.0f);

        //areaCamera.enabled = false;
        //foreach (rArea area in GameObjectsManager.Instance.ListOfLevelAreas)
        //{
        //    if (area == rPasswordManager.Instance.CurrentArea)
        //    {
        //        continue;
        //    }
        //    area.areaCamera.enabled = true;
        //    yield return new WaitForSeconds(3.0f);
        //}
        //    this.areaCamera.enabled = true;
    }

    #endregion

    #region Collision and Trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (areaType == AreaType.Base && collision.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = true;
            rPasswordManager.Instance.CurrentArea = this;

            /// On Entering Area call, invoke OnEnteringArea that UIPassword listens to
            OnEnteringArea?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Area");
        if (areaType == AreaType.Base)
            rPasswordManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(true);
        else
            rPasswordManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(false);

        if (areaType == AreaType.Fight && other.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = true;
            rPasswordManager.Instance.CurrentArea = this;
            /// raise event to spawn enemies
            OnEnteringFight?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            rPasswordManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(false);
        }

        if (areaType == AreaType.Fight && other.gameObject == GameObjectsManager.Instance.Player)
        {
            enemySpawner.AddEnemiesInPool();
            enemySpawner.DisableMiniBosses();
        }


        if (areaType == AreaType.Base && other.gameObject == GameObjectsManager.Instance.Player)
        {
            areaCollider.isTrigger = false;
            playerInside = false;

            for (int i = 0; i < alliesList.Count; i++)
            {
                alliesList[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
