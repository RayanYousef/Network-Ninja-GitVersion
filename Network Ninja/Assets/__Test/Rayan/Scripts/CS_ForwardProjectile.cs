using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ForwardProjectile : MonoBehaviour
{
    [SerializeField] float velocity, disableCoolDown;
    [SerializeField] Transform directionObject;
    float timer;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb= GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        transform.position = directionObject.position;
        transform.rotation = directionObject.rotation;

        rb.velocity =directionObject.forward* velocity;
        timer = 0;
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer>disableCoolDown)
            gameObject.SetActive(false);
    }
}
