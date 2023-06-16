using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CS_RotateTowardTarget : MonoBehaviour
{

    [SerializeField] List<Transform> listOfTargets;
    [SerializeField] Transform objectToRotate, objectToRotateTowards;

    public Transform ObjectToRotateTowards { get => objectToRotateTowards;}
    public List<Transform> ListOfTargets { get => listOfTargets;}

    public void RotateTowardsClosestEnemy()
    {
        CheckInActiveObjectsAndRemoveIt();
        if (listOfTargets.Count < 1) return;

        objectToRotateTowards = GetClosestTransform(listOfTargets);
        Vector3 direction = objectToRotateTowards.position - objectToRotate.position;
        direction.y = 0; direction.Normalize();
        if (direction != Vector3.zero && listOfTargets.Count==1)
            objectToRotate.rotation = Quaternion.LookRotation(direction);
    }

    public void RotateTowardsClosestEnemy(Transform target)
    {
        objectToRotateTowards = target;
        Vector3 direction = objectToRotateTowards.position - objectToRotate.position;
        direction.y = 0; direction.Normalize();
        if (direction != Vector3.zero)
            objectToRotate.rotation = Quaternion.LookRotation(direction);
    }

    public void RotateTowardsClosestEnemy(List<Transform> listOfTargets)
    {
        if (listOfTargets.Count < 1) return;

            objectToRotateTowards = GetClosestTransform(listOfTargets);
            Vector3 direction = objectToRotateTowards.position - objectToRotate.position;
            direction.y = 0; direction.Normalize();
            if (direction != Vector3.zero)
                objectToRotate.rotation = Quaternion.LookRotation(direction);
    }

    public void CheckInActiveObjectsAndRemoveIt()
    {

        for (int i = listOfTargets.Count - 1; i >= 0; i--)
            if (listOfTargets[i].gameObject.activeInHierarchy == false)
                listOfTargets.Remove(listOfTargets[i]);
    }

    public Transform GetClosestTransform(List<Transform> listOfTargets)
    {

        Transform closestTransform = null;
        float closestSqrDistance = float.MaxValue;

        // Iterate through the list of Transforms
        foreach (Transform transform in listOfTargets)
        {
            // Calculate the squared distance between the current Transform and the target position
            Vector3 direction = transform.position - objectToRotate.position;
            float sqrDistance = direction.sqrMagnitude;

            // Check if the squared distance is smaller than the closest squared distance found so far
            if (sqrDistance < closestSqrDistance)
            {
                closestTransform = transform;
                closestSqrDistance = sqrDistance;
            }
        }

        return closestTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy) 
            && !listOfTargets.Contains(other.transform)
            && enemy.Targetable==true)
            listOfTargets.Add(other.transform);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<StatsManager>(out StatsManager enemy)
            && listOfTargets.Contains(other.transform)
            && enemy.Targetable == true) listOfTargets.Remove(other.transform);

    }
}
