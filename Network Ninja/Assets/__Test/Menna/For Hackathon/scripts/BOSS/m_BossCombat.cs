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

  
    [SerializeField] Animator DragonAnim;
    //public Animator PlayerAnim;
  
    private void Start()
    {
        uimanager = GameObjectsManager.Instance.GetComponent<m_BossUI_Manager>();
        combatManager = GameObjectsManager.Instance.gameObject.AddComponent<m_CombatManager>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && DragonAnim.GetCurrentAnimatorStateInfo(0).IsName("AttackState"))
        {
          
                    combatManager.PlayerTakeDamage();
                    //PlayerAnim.SetTrigger("getHit");
                    uimanager.StartCoroutine(uimanager.DoFade());
                
            
        }
    }
}
