using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FreezeObjectsInRange : MonoBehaviour
{
    [SerializeField] CS_PlayerManager _playerManager;
    [SerializeField] List<Transform> objectsInRange;


    public void ObjectsMovementEnabled(bool value, float animationSpeed)
    {
        foreach(var enemy in objectsInRange)
        {
            if(enemy.TryGetComponent<IStopObject>(out IStopObject enemySlowInterface))
            {
                enemySlowInterface.ObjectMovementEnabled(value);

            }
            if (enemy.TryGetComponent<Animator>(out Animator animator))
                animator.speed = animationSpeed;            
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
            && objectsInRange.Contains(other.transform))
        {
            objectsInRange.Remove(other.transform);
            slowInterface.ObjectMovementEnabled(true);
        }

    }
}
