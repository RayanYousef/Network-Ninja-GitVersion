using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class m_BossCombat : MonoBehaviour
{


    [Header("Dragon Animataions")]

     m_CombatManager combatManager;
     m_BossUI_Manager uimanager;
     m_BossMovement DragonMovement;
     StatsManager statsManager;
    
    


    //public Animator PlayerAnim;
  
    private void Start()
    {
        uimanager = GameObjectsManager.Instance.BossUiManager;
        combatManager = GameObjectsManager.Instance.CombatManager;
        DragonMovement = GameObjectsManager.Instance.Boss.GetComponentInChildren<m_BossMovement>();
        statsManager = GameObjectsManager.Instance.Player.GetComponent<StatsManager>();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (DragonMovement.ClawAttackFast() || DragonMovement.BasicAttackFast() || DragonMovement.HornAttackFast())
            {
                if (!DragonMovement.returnFinishedAttack())
                {
                    statsManager.ApplyDamage();
                   // combatManager.PlayerTakeDamage();

                    //PlayerAnim.SetTrigger("getHit");
                    uimanager.StartCoroutine(uimanager.DoFade());
                }
            }
        }
    }
}
