using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GroundCheck : MonoBehaviour
{

    [SerializeField] bool grounded;
    [SerializeField] List<GameObject> hitObjects = new List<GameObject>();
    [SerializeField] int layerNumber;
    [SerializeField] CS_AnimatorController animController;
    public bool Grounded { get => grounded;}

    private void Start()
    {
        animController = GetComponentInParent<CS_AnimatorController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Add the object to the list if it's not already in it
        if (other.gameObject.layer == layerNumber &&!hitObjects.Contains(other.gameObject))
        {
            hitObjects.Add(other.gameObject);
        }
        CheckHitObjects();
    }

    void OnTriggerExit(Collider other)
    {
        // Remove the object from the list if it's in it
        if (hitObjects.Contains(other.gameObject))
        {
            hitObjects.Remove(other.gameObject);
        }
        CheckHitObjects();

    }

    private void CheckHitObjects()
    {
        grounded = hitObjects.Count > 0 ? true : false;
        animController.SetGrounded(grounded);

    }
}
