/*--------------------------------------------------------------
 * File: XRGestureCollisionReporter
 * Version: 1.6
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
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class XRGestureCollisionReporter : MonoBehaviour
{
    #region Preloaded Data
    /// <summary>
    /// The associated controller for the gesture.
    /// </summary>
    [SerializeField]
    private XRBaseColliderGestureControllerV2 m_associatedController;

    /// <summary>
    /// Gets or sets the associated controller for the gesture.
    /// </summary>
    public XRBaseColliderGestureControllerV2 AssociatedController
    {
        get { return m_associatedController; }
        set 
        { 
            if(!LockFields)
            {
                m_associatedController = value;
            }
        }
    }

    /// <summary>
    /// The associated gesture for the gesture.
    /// </summary>
    [SerializeField]
    private XRGesture m_associatedGesture;

    /// <summary>
    /// Gets the associated gesture for the gesture.
    /// </summary>
    public XRGesture AssociatedGesture
    {
        get { return m_associatedGesture; }
        set
        {
            if (!LockFields)
            {
                m_associatedGesture = value;
            }
        }
    }

    /// <summary>
    /// Indicates whether the object is an observation object.
    /// </summary>
    [SerializeField]
    private bool m_isObservationObject;

    /// <summary>
    /// Gets or sets a value indicating whether the object is an observation object.
    /// </summary>
    public bool IsObservationObject
    {
        get { return m_isObservationObject; }
        set
        {
            if (!LockFields)
            {
                m_isObservationObject = value;
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

    /// <summary>
    /// Indicates whether placement is required for the gesture.
    /// </summary>
    [SerializeField]
    private bool m_requirePlacement;

    /// <summary>
    /// Gets or sets a value indicating whether placement is required for the gesture.
    /// </summary>
    public bool RequirePlacement
    {
        get { return m_requirePlacement; }
        set
        {
            if (!LockFields)
            {
                m_requirePlacement = value;
            }
        }
    }

    /// <summary>
    /// The associated placement for the gesture.
    /// </summary>
    [SerializeField]
    private GesturePlacement m_associatedPlacement;

    /// <summary>
    /// Gets or sets the associated placement for the gesture.
    /// </summary>
    public GesturePlacement AssociatedPlacement
    {
        get { return m_associatedPlacement; }
        set
        {
            if (LockFields)
            {
                m_associatedPlacement = value;
            }
        }
    }
    #endregion

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
            if ( ReferenceEquals(value, m_associatedController))
            {
                m_lockFields = value;
            }
        }
    }


    private void ResetFlags()
    {

    }

    //we need a method that will check the inputs of the tracked objects to determine when to start the
    private void CheckInputs()
    {
        //this should only be called if the timer has worn down/reset and the object is the observation object
    }






    protected virtual void DetermineDelegateCall(XRGestureObject trackedObject)//we dont need to get the collider, its already attached
    {
        if(!IsGameObjectInTrackedList(trackedObject))
        {
            return;
        }
        //this function determines if the delegate gets called. what determines this call is if the tracked object is colliding with this objects collider.
        if (!RequirePlacement)
        {
            //nump the index
            CallDelegate(m_associatedController);
        }
        if (m_associatedGesture.RequirePlacement == true)
        {
            //does it match placement?
            bool match = DoesObjectMatchPlacement(trackedObject);
            if (match)
            {
                //bumb up the index
                CallDelegate(m_associatedController);
            }
        }
    }

    //we call the delegate here but the delegate is ON the other object
    private void CallDelegate(XRBaseColliderGestureControllerV2 controller)
    {
        controller.advanceColliderIndexDelegate(m_associatedGesture, gameObject.GetComponent<Collider>(), m_associatedGesture.CurrentIndexLocation);
    }


    #region extra math
    /// <summary>
    /// Checks if the given XRGestureObject is present in the tracked gesture objects list.
    /// </summary>
    /// <param name="targetObject">The XRGestureObject to check.</param>
    /// <returns>True if the XRGestureObject is found in the list, otherwise false.</returns>
    private bool IsGameObjectInTrackedList(XRGestureObject targetObject)
    {
        bool isObjectInList = m_trackedGestureObjects.Contains(targetObject);
        return isObjectInList;
    }


    /// <summary>
    /// Checks if the given XRGestureObject matches the associated placement.
    /// </summary>
    /// <param name="trackedObject">The XRGestureObject to check.</param>
    /// <returns>True if the XRGestureObject's placement matches the associated placement, otherwise false.</returns>
    private bool DoesObjectMatchPlacement(XRGestureObject trackedObject)
    {
        // Save the variable so we aren't potentially referring to a changing variable
        GesturePlacement trackedObjectsPlacement = trackedObject.Placement;
        if (trackedObjectsPlacement == m_associatedPlacement) { return true; }
        else
        {
            // If gesture doesn't match
            Debug.Log($"Gesture placement '{m_associatedGesture.GesturePlacement}' does not match {trackedObject}'s placement {trackedObject.Placement}");
            return false;
        }
    }


    /// <summary>
    /// Finds the XRGestureObject associated with the given GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject to search for.</param>
    /// <param name="foundObject">The found XRGestureObject, if any.</param>
    /// <returns>True if the XRGestureObject is found, otherwise false.</returns>
    private bool RetrieveGestureObjectInfo(GameObject gameObject, out XRGestureObject foundObject)
    {
        foreach (XRGestureObject gestureObject in m_trackedGestureObjects)
        {
            if (gestureObject.GestureObject == gameObject)
            {
                foundObject = gestureObject;
                return true;
            }
        }
        // No XRGestureObject found for the given GameObject
        foundObject = new XRGestureObject();
        return false;
    }

    #endregion
}
