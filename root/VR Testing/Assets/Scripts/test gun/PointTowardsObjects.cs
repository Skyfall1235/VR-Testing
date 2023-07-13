using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PointTowardsObjects;

public class PointTowardsObjects : MonoBehaviour
{
    public Transform targetObject; // The object to check if it's pointing towards
    public bool useOptionalBaseReference;
    public Transform optionalBaseReference;
    public GameObject objectToToggle; //the gameobject we wish to toggle
    public float maximumOffset = 20f; // The maximum allowed offset in degrees
    public TransformType transformSelection;
    public enum TransformType
    { 
        X,
        Y,
        Z,
        negX,
        negY,
        negZ
    }


    private void Start()
    {
        //objectToToggle = gameObject;
    }

    // Example usage
    private void Update()
    {
        if (IsPointingAtTarget())
        {
            objectToToggle.SetActive(true);
            Debug.Log("Object is pointing at the target!");
        }
        else
        {
            objectToToggle.SetActive(false);
            //Debug.Log("Object is not pointing at the target!");
        }
    }
    private bool IsPointingAtTarget()
    {
        // Calculate the vector from this object to the target object
        Vector3 targetDirection = targetObject.position - DetermineOptionalBaseReference();
        targetDirection.Normalize();



        // Calculate the local transform direction based on the selected TransformType
        Vector3 localTransformDirection = PerformTransform(transformSelection);

        // Transform the local transform direction into the global direction
        Vector3 globalDirection = transform.TransformDirection(localTransformDirection);

        // Calculate the angle between the global direction and the target direction
        float angle = Vector3.Angle(globalDirection, targetDirection);

        //Debug.Log(PerformTransform(transformSelection));


        Debug.DrawRay(DetermineOptionalBaseReference(), globalDirection, Color.red);
        Debug.DrawRay(DetermineOptionalBaseReference(), targetDirection, Color.green);


        // Check if the angle is within the allowed maximum offset
        if (angle <= maximumOffset)
        {
            Debug.Log(angle);
            Debug.Log(true);
            return true;
        }
        else
        {
            //Debug.Log(angle);
            return false;
        }
    }

    private Vector3 DetermineOptionalBaseReference()
    {
        if (optionalBaseReference != null && useOptionalBaseReference) 
        {
            return optionalBaseReference.transform.position;
        }
        else
        {
            return transform.position;
        }
    }

    private Vector3 PerformTransform(TransformType type)
    {
        switch (type)
        {
            case TransformType.X:
                return Vector3.right;

            case TransformType.Y:
                return Vector3.up;

            case TransformType.Z:
                return Vector3.forward;

            case TransformType.negX:
                return Vector3.left;

            case TransformType.negY:
                return Vector3.down;

            case TransformType.negZ:
                return Vector3.back;

            default:
                Debug.LogWarning("Invalid transform type!");
                return Vector3.zero;
        }
    }

}
