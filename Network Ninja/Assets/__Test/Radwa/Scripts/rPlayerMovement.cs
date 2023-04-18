using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rPlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool shoot = Input.GetKeyDown(KeyCode.Space);

        Vector3 mov = rb.position + (new Vector3(h, 0, v) * movementSpeed * Time.deltaTime);

        rb.MovePosition(mov);
    }
}
