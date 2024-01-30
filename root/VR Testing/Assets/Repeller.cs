using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Repeller : MonoBehaviour
{
    [SerializeField] private GameObject playerPos;

    [SerializeField] private float maxDecendSpeed;
    public bool isLockedIn = false;

    [SerializeField] private Vector3 floorLocation;
    private XRBaseInteractable interactable { get { return GetComponent<XRBaseInteractable>(); } }

    public InputActionProperty triggerAction;
    //if it is currently selected, and then activated, move straight down.

    //If activated, lock player to a position, as a child

    private void Start()
    {
        Debug.Log($"floor found at {floorLocation}");
        //floorLocation = FindFloor();
    }
    void Update()
    {
        if (isLockedIn)
        {
            //find the value of the trigger and convert it into a speed for the rappel device to lerp to
            float triggerValue = triggerAction.action.ReadValue<float>();
            Debug.Log($"trigger value is {triggerValue}");
            float speedofRappel = DetermineSpeedOFRappelDevice(triggerValue);

            //lock the player to the rappeller

            //now, determine how far down the line the user should lerp to, from the current position to
            Vector3 currentPosition = transform.position;
            Vector3 lerpedPosition = Vector3.Lerp(currentPosition, floorLocation, Time.deltaTime * speedofRappel * speedofRappel);

            // Apply the lerped position to the object's transform
            transform.position = lerpedPosition;
        }
        
        
    }

    public void SetPlayerPosition()
    {
        //get the interactors root object and anchor them to the playerPos
        GameObject rootObject;
        //get the interactor currently selecting this entity
        IXRSelectInteractor interactor = CurrentInteractionProvider.FindSelectInteractorInIndex(interactable, 0);
        if(interactor != null)
        {
            //get the gameobject of the interactor
            //xr origin is usually 2 steps up
            rootObject = interactor.transform.parent.parent.gameObject;
            //finally, set the position
            rootObject.transform.position = playerPos.transform.position;
            isLockedIn = true;
            rootObject.transform.parent = this.transform;
        }
    }

    public void SetLock(bool position)
    {
        isLockedIn = position;
    }

    private float DetermineSpeedOFRappelDevice(float percent)
    {
        float currentspeed = (percent / 100f) * maxDecendSpeed;
        return currentspeed;
    }

    private Vector3 FindFloor()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                // Get the position where the ray hits the floor in world space
                Vector3 floorPosition = hit.point;
                return floorPosition;
            }
            else return Vector3.zero;
        }
        else return Vector3.zero;
    }

}
