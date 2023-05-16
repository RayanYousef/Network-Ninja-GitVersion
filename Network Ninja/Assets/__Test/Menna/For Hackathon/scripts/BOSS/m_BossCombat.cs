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


    //public Animator PlayerAnim;
  
    private void Start()
    {
        uimanager = GameObjectsManager.Instance.BossUiManager;
        combatManager = GameObjectsManager.Instance.CombatManager;
        DragonMovement = GameObjectsManager.Instance.Boss.GetComponent<m_BossMovement>();


    }
    private void OnTriggerEnter(Collider other)
    {
        if (DragonMovement.ClawAttackFast() || DragonMovement.BasicAttackFast() || DragonMovement.HornAttackFast())
        {
            if (!DragonMovement.returnFinishedAttack())
            {
                combatManager.PlayerTakeDamage();
                //PlayerAnim.SetTrigger("getHit");
                uimanager.StartCoroutine(uimanager.DoFade());
            }
        }
    }
}
