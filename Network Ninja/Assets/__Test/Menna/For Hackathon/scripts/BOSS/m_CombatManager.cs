using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class m_CombatManager : MonoBehaviour
{
    [Header("Dragon Stats")]

    [SerializeField] private int DragonHP = 1500;
    [SerializeField] GameObject Dragon;


    private Animator DragonAnim;
    ParticleSystem bloodVfx, bloodVfx2, bloodVfx3;
    bool waiting;

    [Header("Player Stats")]
    [SerializeField] private int PlayerHP = 500;
    [SerializeField] private int PlayerDamage = 75;


    //Script References
    m_BossUI_Manager ui;

   [SerializeField] UnityEvent BossDie;

    private void Awake()
    {

    }
    private void Start()
    {


        DragonAnim = Dragon.GetComponent<Animator>();
        bloodVfx = Dragon.GetComponentsInChildren<ParticleSystem>()[0];
        bloodVfx2 = Dragon.GetComponentsInChildren<ParticleSystem>()[1];
        bloodVfx3 = Dragon.GetComponentsInChildren<ParticleSystem>()[2];
        ui = GameObjectsManager.Instance.BossUiManager;

        bloodVfx.Stop();
        bloodVfx2.Stop();
        bloodVfx3.Stop();


    }

    private void bleed()
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

    private void wait(float duration)
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0f;
        StartCoroutine(HitStop(duration));
    }
    IEnumerator HitStop(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        waiting= false;
    }


    public void DragonTakeDamage()
    {
        if (DragonHP > 0)
        {
            //Dragon.GetComponent<StatsManager>().ApplyDamage(PlayerDamage);
            DragonHP -= PlayerDamage;
            ui.UpdateHealthBar(1500, getDragonHP());
            bleed();
            setDragonHP(DragonHP);

        }
        if(getDragonHP() <= 0)
        {
            DragonAnim.SetBool("dead", true);
            DragonAnim.SetBool("isAttacking", false);
            DragonAnim.SetBool("isChasing", false);
            BossDie?.Invoke();
        }

    }
    private void setDragonHP(int dragonHealth)
    {
        DragonHP= dragonHealth;
    }
    public int getDragonHP()
    {
        return DragonHP;
    }


    public void PlayerTakeDamage()
    {
        getHealth();

        if (getHealth() > 0f)
        {
            PlayerHP -= 95;
            wait(0.08f);
            setHealth(PlayerHP);
        }
        if (getHealth() <= 0f)
        {
            //player die

        }

    }
    void setHealth(int playerHealth)
    {
        PlayerHP = playerHealth;
    }
    public int getHealth()
    {
        return PlayerHP;
    }

















}
