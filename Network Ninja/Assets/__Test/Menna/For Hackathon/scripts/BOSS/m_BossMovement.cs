using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Random = UnityEngine.Random;
<<<<<<< Updated upstream
using static Unity.VisualScripting.Member;
using static UnityEngine.ParticleSystem;

public class m_BossMovement : MonoBehaviour
{

    Transform player;
    GameObject bossHP;

    public Animator dragonAnim;


    private float dragonSlowSpeed = 0.3f;
    private float dragonFastSpeed = 1.5f;
    private bool finishedAttack;
    public GameObject[] trails;
    private void Awake()
    {
        bossHP = GameObjectsManager.Instance.BossHP;
        player = GameObjectsManager.Instance.Player.transform;
        //dragonAnim =gameObject.GetComponent<Animator>();

    }

    private void Start()
    {      
        bossHP.SetActive(true);
=======
public class m_BossMovement : MonoBehaviour
{

     Transform player;
     Animator dragonAnim;

    private void Start()
    {
        player = GameObjectsManager.Instance.Player.transform;
        dragonAnim = gameObject.GetComponent<Animator>();
>>>>>>> Stashed changes
    }
    void Update()
    {
        if (dragonAnim.GetBool("isChasing") == true && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("die") )
        {
            LookAtPlayer();
        }
    }
<<<<<<< Updated upstream

    void trailActivate()
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(true);
        }
    }
    void trailDeactivate()
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
    public bool death()
    {
        dragonAnim.speed = dragonFastSpeed;
        this.GetComponent<RigBuilder>().enabled = false;
        return true;
    }
=======
>>>>>>> Stashed changes
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }
}
