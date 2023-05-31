using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Random = UnityEngine.Random;
using static Unity.VisualScripting.Member;
using static UnityEngine.ParticleSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;



public class m_BossMovement : MonoBehaviour
{


    //[SerializeField] GameObject bossHP;


    public GameObject[] trails;
    public Animator dragonAnim;

   // private GameObject Dragon;
    private Animator DragonAnim;

    private float dragonSlowSpeed = 0.3f;
    private float dragonFastSpeed = 1.5f;
    private bool finishedAttack;
    private Transform player;
    private Rigidbody rb;
    private NavMeshAgent agent;
    bool LookAtPlyer = true;

    ParticleSystem bloodVfx, bloodVfx2, bloodVfx3;


    [SerializeField] UnityEvent BossDie;

    [SerializeField] private Image bloodSplatter;
    [SerializeField] private Color transparentColor;
    [SerializeField] private Color color;
    [SerializeField] float frictionCoefficient = 2.0f;





    private void Awake()
    {
       // Dragon = GameObjectsManager.Instance.Boss;
        DragonAnim = GetComponent<Animator>();

        //bossHP = GameObjectsManager.Instance.BossHP;
        player = GameObjectsManager.Instance.Player.transform;
        //dragonAnim =gameObject.GetComponent<Animator>();

        DragonAnim = GetComponent<Animator>();
        bloodVfx = GetComponentsInChildren<ParticleSystem>()[0];
        bloodVfx2 = GetComponentsInChildren<ParticleSystem>()[1];
        bloodVfx3 =GetComponentsInChildren<ParticleSystem>()[2];

        color = new Color(188f, 0f, 0f, 1f);
        transparentColor = new Color(0f, 0f, 0f, 0f);
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();


    }

    private void Start()
    {
        //bossHP.SetActive(true);

        player.GetComponent<StatsManager>().onTakingDamage.AddListener(bloodPanelForPlayerDamage);
        trailDeactivate();
        // prevent sliding
        Vector3 frictionForce = -rb.velocity * frictionCoefficient;
        rb.AddForce(frictionForce, ForceMode.Acceleration);



    }
    void Update()
    {
        if (dragonAnim.GetBool("isChasing") == true && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("die") && LookAtPlyer ==true)
        {
            LookAtPlayer();
        }

    }

    public void trailActivate()
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(true);
        }
    }
    public void trailDeactivate()
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(false);
        }
    }

    public void ClawAttackSlow()
    {
        finishedAttack = false;
        dragonAnim.speed = dragonSlowSpeed;
        trailActivate();
    }
    public bool ClawAttackFast()
    {
        dragonAnim.speed = dragonFastSpeed;
        return true;
    }
    private void claw_end()
    {
        //Debug.Log("claw_end");
        trailDeactivate();
    }
    public void BasicAttackSlow()
    {
        trailDeactivate();
        finishedAttack = false;


        dragonAnim.speed = dragonSlowSpeed;
    }
    public bool BasicAttackFast()
    {
        dragonAnim.speed = dragonFastSpeed;

        return true;
    }
    public void HornAttackSlow()
    {
        trailDeactivate();
        finishedAttack = false;
        dragonAnim.speed = dragonSlowSpeed;
    }
    public bool HornAttackFast()
    {
        dragonAnim.speed = dragonFastSpeed;
        return true;
    }

    public void Claw_fin()
    {
        //Debug.Log("claw_fin");
        finishedAttack = true;
    }
    public void Horn_fin()
    {
        //Debug.Log("horn_fin");
        finishedAttack = true;
    }
    public void Basic_fin()
    {
        //Debug.Log("Basic_fin");
        finishedAttack = true;
    }

    public bool returnFinishedAttack()
    {
        return finishedAttack;
    }

    public void OnHealthUpdatedFunction()
    {
        if (GetComponent<StatsManager>().Stats.CurrentHealth == 0)
        {
            Die();

        }
    }

    public void Die()
    {
        DragonAnim.SetBool("dead", true);
        DragonAnim.SetBool("isAttacking", false);
        DragonAnim.SetBool("isChasing", false);
        GameManager.Instance.CurrentGameState = GameState.Won;
        BossDie?.Invoke();
    }
    public bool death()
    {
        dragonAnim.speed = dragonFastSpeed;
        //this.GetComponent<RigBuilder>().enabled = false;
        return true;
    }
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }

    public void bleed()
    {
        int bloodNumber = Random.Range(0, 2);

        switch (bloodNumber)
        {
            case 0:
                bloodVfx.Play();
                Debug.Log(bloodVfx);
                break;
            case 1:
                bloodVfx2.Play();
                Debug.Log(bloodVfx2);
                break;
            case 3:
                bloodVfx3.Play();
                Debug.Log(bloodVfx3);
                break;
        }
    }

    public IEnumerator DoFade()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            bloodSplatter.color = Color.Lerp(color, transparentColor, (elapsedTime / 5));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public void bloodPanelForPlayerDamage()
    {
        if(gameObject)
        {
            StartCoroutine(DoFade());
        }
    }
    public void StopMovementAndRotation()
    {
        agent.isStopped = true;
        LookAtPlyer = false;
    }
    public void BackMovementAndRotation()
    {
        agent.isStopped = false;
        LookAtPlyer = true;
    }
}
