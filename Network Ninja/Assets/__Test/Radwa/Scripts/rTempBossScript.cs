using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rTempBossScript : MonoBehaviour
{
    private void Start()
    {
        Die();
    }
    void Die()
    {
        StartCoroutine(WaitAndDie());
    }

    IEnumerator WaitAndDie()
    {
        yield return new WaitForSeconds(4.0f);

        /// invoking this event will start winning 7arakat <3 *Menna's ideas*
        GetComponentInParent<EnemySpawner>().OnBigBossKilled?.Invoke();
    }

}
