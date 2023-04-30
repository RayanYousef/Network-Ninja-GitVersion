using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class looakAtPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    void Start()
    {
        playerTransform = GameObjectsManager.Instance.Player.transform;
    }

    void Update()
    {
        transform.LookAt(playerTransform);
    }
}
