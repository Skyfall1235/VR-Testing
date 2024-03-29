using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEditor;
using Unity.Jobs;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

//lOCATE UNFINISHED WORK BY LOOKING THESE
//NOT FINISHED
//TEMP
/// <summary>
/// Represents a gesture in XR.
/// </summary>
public class XRGesture : MonoBehaviour
{
    #region Gesture Variables
    [Header("Gesture Properties")]
    /// <summary>
    /// The name of the gesture.
    /// </summary>
    [Tooltip("The name of the gesture.")]
    [SerializeField] string m_gestureName;
    public string GestureName
    {
        get { return m_gestureName; }
    }

    /// <summary>
    /// Determines whether the gesture is enabled or not.
    /// </summary>
    [Tooltip("Determines whether the gesture is enabled or not.")]
    [SerializeField] private bool m_gestureEnabled;
    /// <summary>
    /// Gets or sets a value indicating whether the gesture is enabled or not.
    /// </summary>
    public bool gestureEnabled
    {
        get { return m_gestureEnabled; }
        set { m_gestureEnabled = value; }
    }

    /// <summary>
    /// The timeout value for the gesture.
    /// </summary>
    [Tooltip("The timeout value for the gesture.")]
    [SerializeField] private float m_gestureTimeout;
    public float GestureTimeout
    {
        get { return m_gestureTimeout; }
        set { m_gestureTimeout = value; }
    }

    public bool gestureInProgress;// will need to communicate this back to the manager
    public XRGestureTimeControl timeController;//TEMP


    [Space(12)]
    [Header("Tracked Gesture Objects")]

    /// <summary>
    /// The input trigger type for the gesture.
    /// </summary>
    [Tooltip("The input trigger type for the gesture.")]
    [SerializeField] private GestureInputTriggerType m_gestureInputType;
    /// <summary>
    /// Gets or sets the input trigger type for the gesture.
    /// </summary>
    public GestureInputTriggerType GestureInputType
    {
        get { return m_gestureInputType; }
        set { m_gestureInputType = value; }
    }

    /// <summary>
    /// The list of relevant gesture objects for the gesture.
    /// </summary>
    [Tooltip("The list of relevant objects that are to be tracked for the gesture.")]
    [SerializeField] private List<XRGestureObject> m_relevantGestureObjects;
    public List<XRGestureObject> RelevantGestureObjects
    {
        get { return m_relevantGestureObjects; }
        set { m_relevantGestureObjects = value; }
    }   


    [Space(12)]
    [Header("Gesture Colliders")]
    [Tooltip("Have the gestures colliders be compiled in advance?")]
    [SerializeField] private bool m_compileGestures = false; //NOT FINISHED

    /// <summary>
    /// The current index location.
    /// </summary>
    [Tooltip("The current index value.")]
    [SerializeField] private int m_currentIndexLocation;
    /// <summary>
    /// Gets or sets the current index location.
    /// </summary>
    /// <value>The current index location.</value>
    public int CurrentIndexLocation
    {
        get { return m_currentIndexLocation; }
        set { m_currentIndexLocation = value; }
    }

    /// <summary>
    /// the ordered collection of the detection flags used for a gesture.
    /// </summary>
    [Tooltip("the ordered collection of the detection flags.")]
    [SerializeField] private List<detectionColliderData> m_detectionColliderData;
    public List<detectionColliderData> DetectionColliderData
    {
        get { return m_detectionColliderData; }
        set { m_detectionColliderData = value; }
    }


    [Space(12)]
    [Header("Gesture Events")]
    public UnityEvent OnGestureStartEvent = new();
    public List<UnityEvent> OnGestureCollideEvent;
    public UnityEvent OnGestureEndEvent = new();
    public UnityEvent OnGestureCancelEvent = new();


    #endregion

    private void OnValidate()
    {
        if(m_compileGestures)
        {
            CompileChildrenColliders();
        }
    }


    


    //we need to run this at runtime, or at least have it precalculated.
    [BurstCompile]
    private void CompileChildrenColliders()
    {
        //empty the list of colliders and the unity event list, and recompile
        // Get an array of all children of the parent GameObject.
        GameObject[] childrenGameObjects = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            //check for invalid gameobjects, and if there is one, notify the user and end.
            if(gameObject.transform.GetChild(i).gameObject.GetComponent<XRGestureCollisionReporterV2>() == null)
            {
                Debug.LogWarning($"GameObject {gameObject.transform.GetChild(i).gameObject.name} should not be under GameObject {gameObject.name}. Please move it to a different parent object");
                return;
            }
            childrenGameObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }
        Debug.Log($"children list size is -> {childrenGameObjects.Length}");

        //if there are child game objects, we attempt to compile their scripts and references
        if(childrenGameObjects.Length == 0)
        {
            Debug.LogWarning($"Gesture {m_gestureName} contains no child objects, therefore will not compile collider events");
            return;
        }
        CompileColliderEvents(childrenGameObjects);
    }

    [BurstCompile]
    private void CompileColliderEvents(GameObject[] colliderReporters)
    {
        //in the case of it being null, create it, else just clear the list.
        if (OnGestureCollideEvent == null) { OnGestureCollideEvent = new(); } else { OnGestureCollideEvent.Clear(); }

        //for each item in the collider list, go through its components and find the Collision Reporter. Grab its unity event, add it to the event list.
        List<UnityEvent> collisionReporterEventReferences = new List <UnityEvent>();



        //iterate through each object to confirm that it has the reporter, and if it does, to grab the reference to its unity event
        foreach(GameObject go in colliderReporters)
        {
            XRGestureCollisionReporterV2 reporter = go.GetComponent<XRGestureCollisionReporterV2>();
            if (reporter != null)
            {
                //add it to the end of the list
                collisionReporterEventReferences.Add(reporter.OnTriggerEvent);
            }
            else
            {
                //object shouldnt be here or is missing its collision reporting script
                Debug.LogError($"Collider Event Compiliation has found object {go.name} missing its collision reporter. Please Attach the CollisionReporter script to this gameobject or remove it.");
            }
        }

        //save the new compiled events to the event list
        OnGestureCollideEvent = collisionReporterEventReferences;
    }

    

}

