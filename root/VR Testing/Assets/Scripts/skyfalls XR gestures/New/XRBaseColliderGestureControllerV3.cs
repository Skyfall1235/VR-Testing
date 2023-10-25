/*--------------------------------------------------------------
 * File: XRBaseColliderGestureController
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
    [SerializeField] private bool m_gestureRecognitionEnabled;

    [SerializeField] private float m_gestureTimeoutThreshold = 0f;
    public float GlobalGestureTimeoutThreshold { get { return m_gestureTimeoutThreshold; } }//in seconds
    [SerializeField] private List<XRGesture> m_XRGestures = new(); //the list of each custom gesture
    [SerializeField] public List<XRGestureObject> m_trackedGestureObjects = new(); // what objects are to be tracked for which gesture

    public ScriptableObject m_gameObject;
}

[System.Serializable]
public class ScriptObject : ScriptableObject
{
    public string SOname = "name";

}

