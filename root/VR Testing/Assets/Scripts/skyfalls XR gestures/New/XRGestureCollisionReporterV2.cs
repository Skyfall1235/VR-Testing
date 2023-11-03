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

public class XRGestureCollisionReporterV2 : MonoBehaviour
{
    public XRGestureCollisionReporterV2(List<XRGestureObject> trackedObjects, UnityEvent triggerEvent)
    {
        TrackedGestureObjects = trackedObjects;
        OnTriggerEvent = triggerEvent;
    }

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

    public UnityEvent OnTriggerEvent = new UnityEvent();

    public virtual void TriggerEvent()
    {
        OnTriggerEvent.Invoke();
    }

    [BurstCompile]
    private bool DetermineObjectCollision(Collider otherObject)
    {
        List<XRGestureObject> trackedObjects = m_trackedGestureObjects;

        foreach(XRGestureObject obj in trackedObjects)
        {
            if(otherObject.Equals(obj))
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if its on, its able to be interacted with.
        
        // check the collision to make sure its the tracked object.

        //if it is, iterate the gesture, turn itself off,
        //run unity job only after youve confirmed the object is needed.

        //job consists of starting the manager to shift the next item over, starting the managers toggle to show the current and next colliders.
    }
}
