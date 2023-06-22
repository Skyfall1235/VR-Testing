using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowardsObjects : MonoBehaviour
{
    public Transform targetObject; // The object to check if it's pointing towards
    public GameObject targetGameObject; //the gameobject we wish to toggle
    public float maximumOffset = 20f; // The maximum allowed offset in degrees

    private bool IsPointingAtTarget()
    {
        // Calculate the vector from this object to the target object
        Vector3 targetDirection = targetObject.position - transform.position;
        targetDirection.Normalize();

        // Calculate the angle between the forward direction of this object and the target direction
        float angle = Vector3.Angle(transform.forward, targetDirection);

        // Check if the angle is within the allowed maximum offset
        if (angle <= maximumOffset)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Example usage
    private void Update()
    {
        if (IsPointingAtTarget())
        {
            targetGameObject.SetActive(true);
            Debug.Log("Object is pointing at the target!");
        }
        else
        {
            targetGameObject.SetActive(false);
            //Debug.Log("Object is not pointing at the target!");
        }
    }
}
