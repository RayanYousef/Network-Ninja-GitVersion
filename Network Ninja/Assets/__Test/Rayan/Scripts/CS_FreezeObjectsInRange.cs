using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FreezeObjectsInRange : MonoBehaviour
{
    [SerializeField] CS_PlayerManager _playerManager;
    [SerializeField] List<Transform> objectsInRange;


    public void ObjectsMovementEnabled(bool value)
    {
        foreach(var enemy in objectsInRange)
        {
            enemy.TryGetComponent<IStopObject>(out IStopObject enemySlowInterface);
            enemySlowInterface.ObjectMovementEnabled(value);
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<IStopObject>(out IStopObject slowInterface)
            && !objectsInRange.Contains(other.transform))
        {
            objectsInRange.Add(other.transform);

            if (_playerManager.UltimateOn)
                slowInterface.ObjectMovementEnabled(false);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IStopObject>(out IStopObject slowInterface)
            && !objectsInRange.Contains(other.transform))
        {
            objectsInRange.Remove(other.transform);
            slowInterface.ObjectMovementEnabled(true);


        }

    }
}
