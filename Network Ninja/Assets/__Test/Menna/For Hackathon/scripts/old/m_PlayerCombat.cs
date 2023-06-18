using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_PlayerCombat : MonoBehaviour
{
  //  public Animator PlayerAnim;
  //  public GameObject playerGameObject;

    private bool canAttack = false;
    m_CombatManager combatManager;



    private void Start()
    {

        combatManager = GameObjectsManager.Instance.GetComponent<m_CombatManager>();
    }

    
    private void Update()
    {
        PlayerAttack();
    }
    //private bool CheckAnimation()
    //{
    //    //Checking whether the player is in Attack animation state
    //    if(playermovement.PlayerAttack() == true && (!PlayerAnim.GetCurrentAnimatorStateInfo(1).IsName("attack") && !PlayerAnim.GetCurrentAnimatorStateInfo(1).IsName("attack_2")))
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BOSS"))
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BOSS"))
        {
            canAttack = false;
        }
    }

    public void PlayerAttack()
    {
        if (canAttack == true)
        {
            combatManager.DragonTakeDamage();
        }
    }
}
