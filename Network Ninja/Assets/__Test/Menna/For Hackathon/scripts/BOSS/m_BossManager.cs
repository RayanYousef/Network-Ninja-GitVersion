using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Random = UnityEngine.Random;
using static Unity.VisualScripting.Member;
using static UnityEngine.ParticleSystem;
using UnityEngine.Events;
using UnityEngine.AI;
using Unity.Mathematics;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine.Playables;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class m_BossManager : MonoBehaviour , IStopObject
{


    public GameObject[] trails;
    public Animator dragonAnim;
    public float IntervalBetweenBossAttacks = 1;
    public float AttackDuration;
    public bool LookAtPlyer = true;
    public bool startBossState = false;
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera BossCam1;
    public CinemachineVirtualCamera BossCam2;
    public CinemachineVirtualCamera BossCam3;
   // public CinemachineVirtualCamera EffectCam;
    public float BossAttackRange;
    public float BossChaseRange;

    private Animator DragonAnim;
    private float dragonSlowSpeed = 0.3f;
    private float dragonFastSpeed = 1.5f;
    private bool finishedAttack;
    private Transform player;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private ParticleSystem bloodVfx, bloodVfx2, bloodVfx3 ;


    [SerializeField] UnityEvent BossDie;
    [SerializeField] private UnityEngine.UI.Image bloodSplatter;
    [SerializeField] private Color transparentColor;
    [SerializeField] private Color color;
    [SerializeField] float frictionCoefficient = 2.0f;
    [SerializeField] CanvasGroup HP;
    [SerializeField] GameObject attackFromMouth;
    [SerializeField] GameObject attackOnLand;
    [SerializeField] GameObject attackFromHand;
    [SerializeField] GameObject AttackFrontOfBoss;

    
    private void Awake()
    {
        DragonAnim = GetComponent<Animator>();
        player = GameObjectsManager.Instance.Player.transform;
        DragonAnim = GetComponent<Animator>();
        bloodVfx = GetComponentsInChildren<ParticleSystem>()[0];
        bloodVfx2 = GetComponentsInChildren<ParticleSystem>()[1];
        bloodVfx3 =GetComponentsInChildren<ParticleSystem>()[2];
        color = new Color(1f, 1f, 1f, 1f);
        transparentColor = new Color(1f, 1f, 1f, 0f);
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //Attacks = attackFromMouth.GetComponentsInChildren<ParticleSystem>();
        HP.alpha = 0.0f;

    }

    private void Start()
    {
        player.GetComponent<StatsManager>().OnTakingDamage.AddListener(bloodPanelForPlayerDamage);
        trailDeactivate();
        // prevent sliding
        Vector3 frictionForce = -rb.velocity * frictionCoefficient;
        rb.AddForce(frictionForce, ForceMode.Acceleration);
        StartCoroutine(intervalBetCams());
    }
    void Update()
    {
        //if (dragonAnim.GetBool("isChasing") == true && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("die") && dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("IdleState") && LookAtPlyer == true)
        //{
        //    LookAtPlayer();
        //}

        //if (LookAtPlyer && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("die") && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackState")) 
        //{
        //    //Vector3 enemyToPlayer = new Vector3(player.position.x, transform.position.y, player.position.z);
        //    //transform.LookAt(enemyToPlayer);
        //    LookAtPlayer();
        //}
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
        finishedAttack = true;
    }
    public void Horn_fin()
    {
        finishedAttack = true;
    }
    public void Basic_fin()
    {
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
        rUIManager.instance.StartCoroutine(rUIManager.instance.FadeOutPanel(HP, 2));
        attackFromMouth.SetActive(false);

    }
    public bool death()
    {
        dragonAnim.speed = dragonFastSpeed;
        return true;
    }
    public void LookAtPlayer()
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

    public void PlayAttackOnLand()
    {
        //foreach (ParticleSystem p in attackOnLand)
        //{
        //    //p.Play();
        //}

        Vector3 effectPos = new Vector3();
        effectPos = transform.position + transform.forward * 5;
         GameObject LandEffect = Instantiate(attackOnLand, effectPos , Quaternion.identity);
    }  
    public void PlayAttackFromHand()
    {
         Vector3 effectPos = new Vector3();
         effectPos = transform.position + transform.forward * 3 + new Vector3 (5,0,0);
         GameObject FromHandEffect = Instantiate(attackFromHand, effectPos , Quaternion.identity);
    }


    public IEnumerator DoFade()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1.0f)
        {
            bloodSplatter.color = Color.Lerp(color, transparentColor, (elapsedTime / 1.0f));
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
    public void ObjectMovementEnabled(bool value)
    {
        agent.isStopped = !value;
        LookAtPlyer = value;
    }

    private IEnumerator intervalBetCams()
    {
        //yield return new WaitForSeconds(5);
        //EffectCam.enabled = false;
        //GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.5f;
        yield return new WaitForSeconds(5);
        BossCam1.enabled = false;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.5f;
        yield return new WaitForSeconds(5);
        BossCam2.enabled = false;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.5f;
        yield return new WaitForSeconds(5);
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInAfterCutScene());
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(HP, 2, false));
        BossCam3.enabled = false;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 2.5f;
        player.GetComponent<CS_PlayerManager>().ControllerState(true);
        yield return new WaitForSeconds(3);
        GameObjectsManager.Instance.DuringCutScene = false;
        startBossState = true;


    }
}
