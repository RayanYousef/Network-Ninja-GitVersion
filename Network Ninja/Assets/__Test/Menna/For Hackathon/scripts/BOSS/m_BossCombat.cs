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
<<<<<<< Updated upstream
     m_BossMovement DragonMovement;
     StatsManager statsManager;
    
    


=======

  
    [SerializeField] Animator DragonAnim;
>>>>>>> Stashed changes
    //public Animator PlayerAnim;
  
    private void Start()
    {
<<<<<<< Updated upstream
        uimanager = GameObjectsManager.Instance.BossUiManager;
        combatManager = GameObjectsManager.Instance.CombatManager;
        DragonMovement = GameObjectsManager.Instance.Boss.GetComponentInChildren<m_BossMovement>();
        statsManager = GameObjectsManager.Instance.Player.GetComponent<StatsManager>();

=======
        uimanager = GameObjectsManager.Instance.GetComponent<m_BossUI_Manager>();
        combatManager = GameObjectsManager.Instance.gameObject.AddComponent<m_CombatManager>();
>>>>>>> Stashed changes

    }
    private void OnTriggerEnter(Collider other)
    {
<<<<<<< Updated upstream
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
=======
        if (other.gameObject.tag == "Player" && DragonAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackState"))
        {
          
                    combatManager.PlayerTakeDamage();
                    //PlayerAnim.SetTrigger("getHit");
                    uimanager.StartCoroutine(uimanager.DoFade());
                
            
>>>>>>> Stashed changes
        }
    }
}
