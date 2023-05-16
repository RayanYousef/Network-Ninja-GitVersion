using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Random = UnityEngine.Random;
public class m_BossMovement : MonoBehaviour
{

     Transform player;
     Animator dragonAnim;

    private void Start()
    {
        player = GameObjectsManager.Instance.Player.transform;
        dragonAnim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (dragonAnim.GetBool("isChasing") == true && !dragonAnim.GetCurrentAnimatorStateInfo(0).IsName("die") )
        {
            LookAtPlayer();
        }
    }
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }
}
