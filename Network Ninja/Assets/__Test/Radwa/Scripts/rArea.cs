using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Cinemachine;

public enum AreaType { Base, Fight, Main };

public class rArea : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera areaCamera;
    [SerializeField] bool switchCamBack = true;

    [Header("Area Info")]
    [SerializeField] AreaType areaType;
    [SerializeField] float health, maxHealth;
    [SerializeField] string password;

    [SerializeField] bool playerInside = false;
    bool isFirst = true;

    [Header("Events")]
    [SerializeField] public UnityEvent OnEnteringFight;

    [Header("Area Components")]
    EnemySpawner enemySpawner;
    

    [SerializeField] private Transform[] alliesSpawnPos;
    private Formation alliesSpawnerPrefab;
    [SerializeField] private List<FormationAgent> alliesList = new List<FormationAgent>();
    Formation allies1, allies2, allies3;

    [Header("MiniMap Components")]
    [SerializeField] CS_ChangeObjectsColour meshColourChanger;
    [SerializeField] Image sharingPasswordWarningIcon;
    [SerializeField] bool isFlashing;


    [Header("Script Internal Variables")]
    float Timer;
    [SerializeField] float healthTimer = 5;


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
    public EnemySpawner EnemySpawner { get => enemySpawner; set => enemySpawner = value; }
    public CS_ChangeObjectsColour MeshColourChanger { get => meshColourChanger; }
    public Image SharingPasswordWarningIcon { get => sharingPasswordWarningIcon; }
    public bool IsFlashing { get => isFlashing; set => isFlashing = value; }
    public bool PlayerInside {
        get => playerInside;
        set
        {
            playerInside = value;
            if(!playerInside)
            {
                if (areaType == AreaType.Fight)
                {
                    enemySpawner.AddEnemiesInPool();
                    enemySpawner.DisableMiniBosses();
                }
                else
                {
                    for (int i = 0; i < alliesList.Count; i++)
                    {
                        alliesList[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }


    private void Awake()
    {
        if (areaCamera != null) { }
        else
        {
            areaCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnMiniBosses);
        OnEnteringFight.AddListener(GetComponentInChildren<EnemySpawner>().SpawnEnemies);

        enemySpawner = GetComponentInChildren<EnemySpawner>();

        alliesSpawnerPrefab = GameObjectsManager.Instance.AllyBatalionPrefab;

        meshColourChanger.MaxHealth = rAreasManager.Instance.MaxHealth;
        meshColourChanger.HalfHealth = rAreasManager.Instance.HalfHealth;
        meshColourChanger.LowHealth = rAreasManager.Instance.LowHealth;


        Renderer[] Renderers = new Renderer[1];


        Renderers[0] = GetComponentsInChildren<Renderer>()[1];
        if (rAreasManager.Instance.IsTutorial)
            meshColourChanger.MeshRenderers = Renderers;

        sharingPasswordWarningIcon.gameObject.GetComponent<Image>().enabled = false;
    }
    void Start()
    {
        Password = null;
        maxHealth = rAreasManager.Instance.MaxSoldiersNumber;

        if (areaType == AreaType.Fight)
        {
            meshColourChanger.ChangeToColour(Color.red);
        }
        else if (areaType == AreaType.Main)
        {
            meshColourChanger.ChangeToColour(rAreasManager.Instance.MaxHealth);
            health = maxHealth;
        }

        enemySpawner.OnBigBossKilled.AddListener(Winning);
    }

    private void FixedUpdate()
    {
        //if (playerInside == false && password != null && !GameManager.Instance.BossEntered)
        //    UpdateHealth();

        //if (!playerInside && (password != null || areaType == AreaType.Main) && !rAreasManager.Instance.IsTutorial && !GameManager.Instance.BossEntered)
        if (!playerInside && password != null && !rAreasManager.Instance.IsTutorial && !GameManager.Instance.BossEntered)
            UpdateHealth();
    }

    private void UpdateHealth()
    {
        Timer += Time.deltaTime;
        if (Timer > healthTimer)
        {
            Timer = 0;
            health = Mathf.Clamp(health - 1, 0, maxHealth);

            if (health > 0)
                meshColourChanger.LerpBetweenObjectColours(health / maxHealth);
            //else
                //meshColourChanger.ChangeToColour(Color.red);


            if (health == 0 && password != null)
            {
                rAreasManager.Instance.OnHealthZeroDestroyAreasWithSamePassword(this);
                LostArea();
            }
        }
    }

    public IEnumerator StartFlashing()
    {
        while(IsFlashing)
        {
            Debug.Log("flashing");
            meshColourChanger.ChangeToColour(rAreasManager.Instance.DarkColor);
            yield return new WaitForSecondsRealtime(0.5f);
            if (!IsFlashing)
                yield break;
            meshColourChanger.ChangeToColour(Color.white);
            yield return new WaitForSecondsRealtime(0.5f);
            if (!IsFlashing)
                yield break;
        }
    }


    public void LostArea()
    {
        areaType = AreaType.Fight;
        password = null;
        sharingPasswordWarningIcon.gameObject.GetComponent<Image>().enabled = false;
        meshColourChanger.ChangeToColour(Color.red);

        DestroyAllAllies();
    }

    public void Winning()
    {
        // current area minimap ..> max health color
        meshColourChanger.ChangeToColour(rAreasManager.Instance.MaxHealth);
        health = rAreasManager.Instance.MaxSoldiersNumber;

        // Allies Formation
        FormStrongArmy();

        //areaCamera.enabled = true;
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
        if (Health <= maxHealth / 3)
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

        enableCinemachine();
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

    void enableCinemachine()
    {
        if (GameObjectsManager.Instance.CameraBrain != null)
        {
            GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.0f;

            //areaCamera.enabled = true;
            //if (switchCamBack)
            //    StartCoroutine(WaitAndSwitchCameraBack());
        }
    }
    IEnumerator WaitAndSwitchCameraBack()
    {
        yield return new WaitForSeconds(4.5f);
        areaCamera.enabled = false;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.0f;
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
        if (collision.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = true;
            if (rAreasManager.Instance.CurrentArea == this)
            {
                return;
            }
            rAreasManager.Instance.CurrentArea = this;
            
            if (areaType == AreaType.Fight)
            {
                rAreasManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(false);
                rFaceExpressionManager.Instance.ChangeFacialExp(rFaceExpressionManager.FacialExp.Serious);
                /// raise event to spawn enemies
                OnEnteringFight?.Invoke();
                return;
            }

            rFaceExpressionManager.Instance.ChangeFacialExp(rFaceExpressionManager.FacialExp.Idle);
            if (areaType == AreaType.Base)
            {
                playerInside = true;
                rAreasManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(true);

                if (password == null)
                {
                    rUIManager.Instance.UiPassword.ShowCreatePasswordPanel();
                }
            }
            else if (isFirst)
            {
                // defualt area has max health
                this.Health = maxHealth;
                FormStrongArmy();
                // show intro panel
                if(!rAreasManager.Instance.IsTutorial)
                {
                    rUIManager.Instance.UiPassword.ShowIntroPanel();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                isFirst = false;
            }
            else /*AreaType == AreaType.Main && !isFirst*/
            {
                ShowAlliesBasedOnAreaHealth();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == GameObjectsManager.Instance.Player)
        {
            playerInside = false;
            if (rAreasManager.Instance.CurrentArea == this)
            {
                return;
            }
            rAreasManager.Instance.PasswordCanvas.ResetPasswordButtonInteractbility(false);
        }
    }

    #endregion

}
