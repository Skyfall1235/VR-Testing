/*--------------------------------------------------------------
 * File: XRGestureCollisionReporter
 * Version: 2.0
 * Author: Wyatt Murray
 * Created: 6/29/23
 * 
 *  Project Description: The XRGesture Toolkit is a virtual reality gesture
 *  controller designed to enhance user interactions
 *  and experiences in Unity applications. It provides
 *  developers with a set of tools and features to
 *  easily incorporate hand gestures and interactions
 *  into their virtual reality projects.
 * --------------------------------------------------------------
 *  This script is the Collision reporter for the XR gesture Toolkit, 
 *  and has method built in to determine what objects collide with it 
 *  and how to operate on that collision data. this script is built to be 
 *  instantiated on a predefined object, with its data defined therein. 
 *  this code is built to be thread safe, and can be extended for 
 *  additional functionality.
 * --------------------------------------------------------------
 * Ownership and Usage:
 * - This code is part of the XR Gesture Toolkit.
 * - The XR Gesture Toolkit is created and owned by Wyatt Murray.
 * - You are granted a non-exclusive, perpetual license to use and
 *   modify this code for personal and commercial purposes.
 * - Distribution of this code, in part or in whole, is not allowed.
 * - Attribution is required for any commercial products that
 *   incorporate this code.

 * --------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.InputSystem;

public class XRGestureCollisionReporterV2 : MonoBehaviour
{
    public XRGestureCollisionReporterV2(List<XRGestureObject> trackedObjects, UnityEvent triggerEvent)
    {
        TrackedGestureObjects = trackedObjects;
        OnTriggerEvent = triggerEvent;
    }

    [SerializeField] bool m_retryLinkToGestureOnValidate = false;

    /// <summary>
    /// The tracked gesture objects.
    /// </summary>
    [SerializeField] private List<XRGestureObject> m_trackedGestureObjects;

    /// <summary>
    /// Gets or sets the tracked gesture objects.
    /// </summary>
    public List<XRGestureObject> TrackedGestureObjects
    {
        get { return m_trackedGestureObjects; }
        set { m_trackedGestureObjects = value; }
    }

    public UnityEvent OnTriggerEvent;


    private void OnValidate()
    {
        if (m_retryLinkToGestureOnValidate)
        {
            RetryLinkWithGesture(gameObject.transform.parent.gameObject);
        }
    }


    [BurstCompile]
    private bool DetermineObjectCollision(Collider otherObject)
    {
        List<XRGestureObject> trackedObjects = m_trackedGestureObjects;

        //for this method, we only need to determine if ANY items are on the gesture object list, not a specific one.
        foreach(XRGestureObject obj in trackedObjects)
        {
            //if the objects ID and compare it against the list of tracked objects
            if(otherObject.GetInstanceID() == obj.GestureObject.GetInstanceID()) {return true;}
        }
        return false;
    }

    //if we want to specific object, we use the method aboves cached value to retrive the speific objec,t which can then be compared against.

    [BurstCompile]
    private XRGestureObject DetermineSpecificObjectCollision(Collider otherObject) 
    {
        List<XRGestureObject> trackedObjects = m_trackedGestureObjects;

        //we find the first gesture that successfully collides. this does mean that there can be stacked collisions,
        //but if we evaluate it for every collsion, we should be able to determine which objects are in what sequence.   
        foreach (XRGestureObject obj in trackedObjects)
        {
            //if the objects ID and compare it against the list of tracked objects
            if (otherObject.GetInstanceID() == obj.GestureObject.GetInstanceID()) {return obj;}
        }
        //return the most null we can make it, and have the method caller confirm its not an invalid object.
        InputActionProperty nullAction = new();
        XRGestureObject emptyObject = new XRGestureObject("", new GameObject(), nullAction);
        return emptyObject;
    }

    

    public void RetryLinkWithGesture(GameObject XRGestureGameObject)
    {
        if (XRGestureGameObject.GetComponent<XRGesture>() != null)
        {
            // Assign and cache the components we wish to add and link
            XRGesture parentXRG = XRGestureGameObject.GetComponent<XRGesture>();;

            // Set the tracked object to the reporter
            m_trackedGestureObjects = parentXRG.RelevantGestureObjects;

            //Create a new unity event, store it in the Gesture, and assign it to the OnTriggerEvent in the reporter
            parentXRG.OnGestureCollideEvent.Add(new UnityEvent());
            UnityEvent newEvent = parentXRG.OnGestureCollideEvent.LastOrDefault();
            OnTriggerEvent = newEvent;
            m_retryLinkToGestureOnValidate = false;
            Debug.Log($"Collision Reporter ID:{gameObject.GetInstanceID()} successfully linked");
        }
        else
        {
            Debug.LogWarning($" Could not assign tracked object list to collider ID:{XRGestureGameObject.GetInstanceID()} \n Please manually assign in the inspector");
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        // if the reporter is on, its able to be interacted with.
        
        // check the collision to make sure its the tracked object.
        // if it isnt the tracked object we wish to use, return
        if (!DetermineObjectCollision(other)) { return; }

        // invoke its unity event
          //.Invoke();

        // if it is, iterate the gesture, turn itself off


    }
}
