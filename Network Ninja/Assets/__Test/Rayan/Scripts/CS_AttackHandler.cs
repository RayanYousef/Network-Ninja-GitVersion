using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CS_AttackHandler : MonoBehaviour
{
    [SerializeField] List<Collider> hitObjects = new List<Collider>();
    [SerializeField] Transform weapon, player;
    [SerializeField] float speed;




    public void SetAttackRotation() 
    {
        transform.rotation=weapon.rotation;
        transform.position= player.GetComponent<Collider>().bounds.center;
        GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        GetComponentInChildren<Rigidbody>().AddForce(player.forward * speed, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    private void OnEnable()
    {
        hitObjects.Clear();

    }

    private void OnDisable()
    {
        hitObjects.Clear();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!hitObjects.Contains(other))
        {
            Debug.Log(other.name);
        }
    }
}
