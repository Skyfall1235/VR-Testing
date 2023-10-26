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
    public int seriesNumber;

    /// <summary>
    /// The associated controller for the gesture.
    /// </summary>
    [SerializeField]
    private XRBaseColliderGestureControllerV2 m_associatedController;

    /// <summary>
    /// This bool is responsible for locking the other variables after they have been set.
    /// </summary>
    /// <remarks>
    /// these variables are reference variables and should only ever refer to thieir predesigned components. if they need to be changed, their can change from the controller or a sub class of it.
    /// If the values above need to be changed, this value has to be set to False first.
    /// </remarks>
    [HideInInspector] bool m_lockFields = false;
    public bool LockFields
    {
        get { return m_lockFields; }
        set
        {
            // Only allow modification from within the XRGestureController script
            if (ReferenceEquals(value, m_associatedController))
            {
                m_lockFields = value;
            }
        }
    }

    

    /// <summary>
    /// The tracked gesture objects.
    /// </summary>
    [SerializeField]
    private List<XRGestureObject> m_trackedGestureObjects;

    /// <summary>
    /// Gets or sets the tracked gesture objects.
    /// </summary>
    public List<XRGestureObject> TrackedGestureObjects
    {
        get { return m_trackedGestureObjects; }
        set
        {
            if (!LockFields)
            {
                m_trackedGestureObjects = value;
            }
        }
    }

    public Collider reporterCollider;

    [SerializeField] private UnityEvent OnTriggerEvent = new UnityEvent();


    public void Setup()
    {

    }

    public virtual void TriggerEvent()
    {
        OnTriggerEvent.Invoke();
    }

    private void CheckCurrentSeriesNumber()
    {

    }

    private bool DetermineObjectCollision(Collider otherObject, out XRGestureObject gestureData)
    {
        List<XRGestureObject> trackedObjects = m_trackedGestureObjects;

        foreach(XRGestureObject obj in trackedObjects)
        {
            if(otherObject.Equals(obj))
            {
                gestureData = obj;
                return true;
            }
        }
        gestureData = new XRGestureObject();
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        
        //first determine if its in series, then if its good, check the object. if thats also good, report back to the manager.
        //run unity job only after youve confirmed the object is needed.

        //job consists of starting the manager to shift the next item over, starting the managers toggle to show the current and next colliders.
    }
}
