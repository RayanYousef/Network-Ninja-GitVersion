using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FreezeObjectsInRange : MonoBehaviour
{
    [SerializeField] CS_PlayerManager _playerManager;
    [SerializeField] List<Transform> objectsInRange;


    public void ObjectsStopped(bool value)
    {
        foreach(var enemy in objectsInRange)
        {
            enemy.TryGetComponent<m_interface>(out m_interface enemySlowInterface);
            enemySlowInterface.MovementAndRotation(!value);

        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<m_interface>(out m_interface slowInterface)
            && !objectsInRange.Contains(other.transform))
        {
            objectsInRange.Add(other.transform);

            if (_playerManager.UltimateOn)
                slowInterface.MovementAndRotation(false);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<m_interface>(out m_interface slowInterface)
            && !objectsInRange.Contains(other.transform))
        {
            objectsInRange.Remove(other.transform);
            slowInterface.MovementAndRotation(true);


        }

    }
}
