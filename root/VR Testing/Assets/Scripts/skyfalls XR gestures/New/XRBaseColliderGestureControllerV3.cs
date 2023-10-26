/*--------------------------------------------------------------
 * File: XRBaseColliderGestureController
 * Version: 1.8
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
 *  This script is the primary controller for the XR gesture, and 
 *  has methods built in to evaluate gesture data through its collision 
 *  reporters. This script is written to be thread safe for 
 *  multithreading, and can be extended for addtional functionality.
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
using UnityEngine.InputSystem;


[RequireComponent(typeof(Collider))] // for the generatio aspect
public class XRBaseColliderGestureControllerV3 : MonoBehaviour
{
    #region Inspector Variables
    /// <summary>
    /// Determines whether gesture recognition is enabled.
    /// </summary>
    [Tooltip("Enable this to allow users to interact with objects and perform actions using gestures.")]
    [SerializeField] private bool m_gestureRecognitionEnabled;

    /// <summary>
    /// The gesture settings that are used by the gesture recognition system.
    /// </summary>
    [Tooltip("These settings control the sensitivity and accuracy of the gesture recognition system.")]
    [SerializeField] private XRGestureSettings m_gestureSettings;
    public XRGestureSettings GlobalGestureSettings
    {
        set 
        { 
            if(m_gestureSettings == null)
            {
                m_gestureSettings = value;
            }
        }
    }

    /// <summary>
    ///  The maximum amount of time that the gesture recognition system will wait for a user to complete a gesture.
    /// </summary>
    [Tooltip("Increase this value to give users more time to complete gestures, or decrease it to make the gesture recognition system more responsive.")]
    [SerializeField] private float m_gestureTimeoutThreshold = 0f;

    /// <summary>
    /// A list of all custom gestures that are supported by the gesture recognition system.
    /// </summary>
    [Tooltip("Add new gestures to this list to allow users to interact with objects and perform actions using custom gestures.")]
    [SerializeField] private List<XRGesture> m_XRGestures = new(); //the list of each custom gesture

    /// <summary>
    /// A list of all objects that are being tracked for gestures.
    /// </summary>
    [Tooltip("Add objects to this list to allow users to interact with them using gestures.")]
    [SerializeField] public List<XRGestureObject> m_trackedGestureObjects = new(); // what objects are to be tracked for which gesture
    #endregion

    public XRGesture currentGesture;

    public void StartGesture(XRGesture gesture)
    {

    }

}

//will need a way to turn off the colliders that are not in use. so if a whole gesture is off, turn every object off inside of it.
//if someone is in a gesture, no other gesture should be active
//only the current and next collider should be active. this toggle process can be a unity job.

