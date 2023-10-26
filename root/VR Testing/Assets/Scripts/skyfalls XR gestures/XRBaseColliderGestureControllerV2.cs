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

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using Unity.Collections;
using System;

[RequireComponent(typeof(BoxCollider))]
public class XRBaseColliderGestureControllerV2 : MonoBehaviour
{
    [SerializeField] private bool m_gestureRecognitionEnabled;

    [SerializeField] private float m_gestureTimeoutThreshold = 0f;
    public float GestureTimeoutThreshold { get { return m_gestureTimeoutThreshold; } }//in seconds

    public float m_shoulderWidth = 0.3f;
    [SerializeField] private BoxCollider m_activationBoxCollider;
    [SerializeField] private List<XRGesture> m_XRGestures = new(); //the list of each custom gesture
    [SerializeField] public List<XRGestureObject> m_trackedGestureObjects = new(); // what objects are to be tracked for which gesture
    public AdvanceColliderIndexDelegate advanceColliderIndexDelegate = null;



    #region Base Mononbehavior Methods
    private void Awake()
    {
        
    }
    private void Start()
    {
        SetUpDetectionObjects();
        SubscribeToDelegate(advanceColliderIndexDelegate);
        
    }
    private void Update()
    {
        //SetUpDetectionObjects();
    }
    private void FixedUpdate()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        UnsubscribeToDelegate(advanceColliderIndexDelegate);
    }
    #endregion






    /// <summary>
    /// Closes out function calls related to the object.
    /// </summary>
    /// <remarks>
    /// This method should be called when the object it is attached to is destroyed or disabled. It is responsible for cleaning up any function calls or subscriptions associated with the object.
    /// 
    /// It specifically unsubscribes from the <see cref="AdvanceColliderIndexDelegate"/> to ensure proper cleanup of event subscriptions.
    /// 
    /// Note: It is important to call this method to prevent any memory leaks or unwanted behavior when the object is destroyed or disabled.
    /// </remarks>
    protected virtual void CloseOutFunctionCalls() //MUST CALL THIS ON DESTROY OR DISABLE
    {
        UnsubscribeToDelegate(advanceColliderIndexDelegate);
    }

    #region Setup Methods



    /// <summary>
    /// Sets up the detection objects for XRGesture system.
    /// </summary>
    private void SetUpDetectionObjects()
    {
        // Set up the gesture references.
        for (int i = 0; i < m_XRGestures.Count; i++)
        {
            XRGesture gesture = m_XRGestures[i];

            for (int j = 0; j < gesture.DetectionColliderData.Count; j++)
            {
                detectionColliderData data = gesture.DetectionColliderData[j];
                //GameObject modifiedObject = DetectionColliderSetup(data.DetectionShapeInfo, gesture, j);

                // Store the modified object and related data
                //data.colliderGameobject = modifiedObject;
                data.seriesNumber = j;
                //data.colliderObjectTransform = modifiedObject.GetComponent<Transform>();
                //data.objectCollider = modifiedObject.GetComponent<Collider>();
                //data.DetectionShapeInfo = data.DetectionShapeInfo;

                // Save the updated data back into the gesture
                gesture.DetectionColliderData[j] = data;

                // Set up the index for the gesture
                gesture.gestureInProgress = false;
                gesture.CurrentIndexLocation = 0;

                // Set up the positions
                //modifiedObject.transform.localPosition = data.colliderPlacementPosition;
            }
        }
    }


    /// <summary>
    /// Destroys the old detection colliders and then uses the collection data to rebuild the object collection
    /// </summary>
    private void RegenerateDetectionObjects()
    {
        //clear all the children, and then reset
        DestroyChildrenOfController();
        SetUpDetectionObjects();
    }


    //set up the collider objects when they get spawn in
    /// <summary>
    /// Sets up a detection collider object based on the provided settings data.
    /// </summary>
    /// <param name="settingsData">The detection shape settings data.</param>
    /// <param name="gesture">The XRGesture object associated with the detection collider.</param>
    /// <param name="indexNumber">The index number of the detection collider within the XRGesture system.</param>
    /// <returns>The modified and scaled detection collider object.</returns>
    private GameObject DetectionColliderSetup(DetectionShapeSettings settingsData, XRGesture gesture, int indexNumber)
    {
        // Create a primitive GameObject based on the provided shape
        GameObject primitiveObject = GameObject.CreatePrimitive(settingsData.PrimitiveDetectionShape);

        // Remove the unnecessary mesh renderer and mesh filter components
        Destroy(primitiveObject.GetComponent<MeshRenderer>());
        Destroy(primitiveObject.GetComponent<MeshFilter>());
        Debug.Log("Removed mesh components");

        // Set the collider to trigger mode to avoid collision effects
        primitiveObject.GetComponent<Collider>().isTrigger = true;

        // Set up the collision reporter on the collider for proper reporting
        GameObject reportingObject = SetupCollisionReporter(primitiveObject, indexNumber, gesture.RelevantGestureObjects, gesture);

        // Scale the collider to the specified size if required
        GameObject scaledObject = SetupDetectionColliderScaling(reportingObject, gesture, indexNumber);

        scaledObject.transform.SetParent(transform); // Make it a child of the gesture
        scaledObject.transform.position = transform.position; // Set its position to match the gesture's position
        Debug.Log("Created new modified object");

        return scaledObject;
    }


    /// <summary>
    /// Sets up the scaling for a detection collider object.
    /// </summary>
    /// <param name="primitiveObject">The primitive object to be scaled.</param>
    /// <param name="gesture">The XRGesture object associated with the detection collider.</param>
    /// <param name="itemIndex">The index of the detection collider item within the XRGesture system.</param>
    /// <returns>The scaled detection collider object.</returns>
    private GameObject SetupDetectionColliderScaling(GameObject primitiveObject, XRGesture gesture, int itemIndex)
    {
        // Get the relevant detection shape settings
        //DetectionShapeSettings relevantSettingData = gesture.DetectionColliderData[itemIndex].DetectionShapeInfo;

        // Create a scaled object based on the primitive object
        GameObject scaledObject = primitiveObject;

        // Set up the size
        //if (!relevantSettingData.UseScaledSize)
        //{
        //    // Set the scale using the primitive shape size
        //    scaledObject.transform.localScale = new Vector3(relevantSettingData.PrimitiveDetectionShapeSize, relevantSettingData.PrimitiveDetectionShapeSize, relevantSettingData.PrimitiveDetectionShapeSize);
        //}
        //else
        //{
        //    // Set the local scale using the custom scaled size
        //    scaledObject.transform.localScale = relevantSettingData.CustomScaledSize;
        //}

        // Return the scaled object
        return scaledObject;
    }


    //add the collision reporter, and set it up.
    /// <summary>
    /// Sets up a collision reporter for a detector object in the XRGesture system.
    /// </summary>
    /// <param name="detectorObject">The detector object to set up the collision reporter for.</param>
    /// <param name="indexLocation">The index location of the detector object within the XRGesture system.</param>
    /// <param name="chosenTrackedGestureObjects">The list of chosen tracked gesture objects.</param>
    /// <param name="gesture">The XRGesture object associated with the collision reporter.</param>
    /// <returns>The detector object with the collision reporter set up.</returns>
    private GameObject SetupCollisionReporter(GameObject detectorObject, int indexLocation, List<XRGestureObject> chosenTrackedGestureObjects, XRGesture gesture)
    {
        // Initializes the collision reporter and saves it for reference
        XRGestureCollisionReporter reporter = detectorObject.AddComponent<XRGestureCollisionReporter>();

        // If it's the first object, it will be the observation object
        if (indexLocation == 0)
        {
            reporter.IsObservationObject = true;
            CreateInstanceOfTimer(detectorObject, this, gesture);
            //gesture.ObservationObject = detectorObject;
        }

        // Fills the reporter's variables with the relevant information
        reporter.AssociatedController = this;
        reporter.TrackedGestureObjects = chosenTrackedGestureObjects;
        reporter.AssociatedGesture = gesture;

        //finally, lock the fields by chasnging thier setter using a nice lil bool
        reporter.LockFields = true;

        return detectorObject;
    }

    /// <summary>
    /// Creates a new instance of XRGestureTimeControl and initializes it with the provided associatedController and associatedGesture.
    /// </summary>
    /// <param name="associatedController">The XRBaseColliderGestureControllerV2 associated with the timer control.</param>
    /// <param name="associatedGesture">The XRGesture associated with the timer control.</param>
    /// <returns>Returns the created XRGestureTimeControl instance.</returns>
    public static void CreateInstanceOfTimer(GameObject ObservationGameobject, XRBaseColliderGestureControllerV2 associatedController, XRGesture associatedGesture)
    {
        // Add the XRGestureTimeControl component to the GameObject.
        XRGestureTimeControl timeControlScript = ObservationGameobject.AddComponent<XRGestureTimeControl>();
        // Initialize the XRGestureTimeControl component with the associatedController and associatedGesture.
        timeControlScript.AssociatedController = associatedController;
        timeControlScript.AssociatedGesture = associatedGesture;
        // Return the created XRGestureTimeControl instance.
    }


    private void DestroyChildrenOfController()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    #endregion


    //now, make subscription events that this controller watches for using delegates on the reporter, and when the flag is flipped, it iterates to the next one
    /// <summary>
    /// Advances the collider index for the selected XRGesture based on the triggered collider.
    /// </summary>
    /// <param name="selectedGesture">The selected XRGesture object.</param>
    /// <param name="triggeredCollider">The collider that triggered the advancement.</param>
     private void AdvanceColliderIndex(XRGesture selectedGesture, Collider triggeredCollider, int currentIndexLocation)
    {
        //if (selectedGesture.DetectionColliderData[currentIndexLocation].objectCollider == triggeredCollider)
        //{
        //    selectedGesture.CurrentIndexLocation++; // Bump up to the next value so we can check the next collider
        //}
        //else
        //{
        //    return;
        //}
        //these automatically check if the gesture is in the middle of its sequence or at the end
        PerformCustomActionAtIndexValue(selectedGesture);

        PerformCustomActionAtEndOfSequence(selectedGesture);
    }


    #region Extendable Methods


    /// <summary>
    /// Performs a custom action based on the provided XRGesture at a specific index value.
    /// </summary>
    /// <param name="gesture">The XRGesture object representing the gesture.</param>
    /// <remarks>
    /// This method is called to handle a specific index value of the XRGesture. It checks if the current index location
    /// is within the valid range of the detection collider data. If so, it performs the custom action associated with the collider at that index.
    /// </remarks>
    protected virtual void PerformCustomActionAtIndexValue(XRGesture gesture)
    {
        if (gesture.CurrentIndexLocation >= 0 && gesture.CurrentIndexLocation < gesture.DetectionColliderData.Count)
        {
            // Code to handle the collider in the expected order
            gesture.gestureInProgress = true;
        }
    }


    /// <summary>
    /// Performs a custom action at the end of a gesture sequence.
    /// </summary>
    /// <param name="gesture">The XRGesture object representing the gesture sequence.</param>
    /// <remarks>
    /// This method is called when the gesture sequence has reached its end. Currently, this method is empty and can be overridden in derived classes to provide custom actions to be performed at the end of the sequence.
    /// </remarks>
    protected virtual void PerformCustomActionAtEndOfSequence(XRGesture gesture)
    {
        //confirm
        if (gesture.CurrentIndexLocation >= gesture.DetectionColliderData.Count)
        {
            gesture.CurrentIndexLocation = 0; // Reset the index if it overflows
            gesture.gestureInProgress = false;
        }
        // Empty as of right now
    }


    /// <summary>
    /// Subscribes to the specified delegate for advancing the collider index.
    /// </summary>
    /// <param name="delegateReference">The delegate reference to subscribe to.</param>
    /// <remarks>
    /// This method subscribes to the provided delegate for advancing the collider index. It adds the <see cref="AdvanceColliderIndex"/> method to the delegate's invocation list.
    /// 
    /// To extend this functionality, you can override this method in derived classes to add additional logic or subscribe to different delegates for advancing the collider index.
    /// </remarks>
    protected virtual void SubscribeToDelegate(AdvanceColliderIndexDelegate delegateReference)
    {
        delegateReference += AdvanceColliderIndex;
    }


    /// <summary>
    /// Unsubscribes from the specified delegate for advancing the collider index.
    /// </summary>
    /// <param name="delegateReference">The delegate reference to unsubscribe from.</param>
    /// <remarks>
    /// This method unsubscribes from the provided delegate for advancing the collider index. It removes the <see cref="AdvanceColliderIndex"/> method from the delegate's invocation list.
    /// 
    /// To extend this functionality, you can override this method in derived classes to add additional logic or unsubscribe from different delegates for advancing the collider index.
    /// </remarks>
    protected virtual void UnsubscribeToDelegate(AdvanceColliderIndexDelegate delegateReference)
    {
        delegateReference -= AdvanceColliderIndex;
    }

    #endregion



    public void StartGesture(XRGesture gesture)
    {
        //create the timer and wait for the first connect on the player to the collider
    }





    /// <summary>
    /// Interrupts the timer associated with the provided TimerCoroutine and starts a new timer for the associated XRGesture.
    /// </summary>
    /// <param name="timerCoroutine">The TimerCoroutine for which to interrupt the timer.</param>


    private void KillTimerInstance(XRGesture gestureTimerToKill)
    {


    }

}


/// <summary>
/// Delegate for advancing the collider index in response to a triggered collider.
/// </summary>
/// <param name="selectedGesture">The selected XRGesture object.</param>
/// <param name="triggeredCollider">The collider that triggered the advancement.</param>
/// <param name="currentIndexLocation">The current index location of the XRGesture within the detection collider data.</param>
/// <remarks>
/// This delegate is used to handle the advancement of the collider index when a specific collider is triggered. The delegate can be assigned to methods that handle the collider advancement logic.
/// 
/// The parameters of the delegate provide the necessary information for identifying the selected gesture, the collider that triggered the advancement, and the current index location within the detection collider data.
/// 
/// To assign a method to this delegate, use the following syntax: <c>delegateInstance += MethodName;</c>
/// To unsubscribe a method from this delegate, use the following syntax: <c>delegateInstance -= MethodName;</c>
/// 
/// Example usage:
/// <code>
/// AdvanceColliderIndexDelegate advanceColliderIndexDelegate;
/// 
/// void Start()
/// {
///     advanceColliderIndexDelegate += MyAdvanceColliderIndexMethod;
/// }
/// 
/// void OnDestroy()
/// {
///     advanceColliderIndexDelegate -= MyAdvanceColliderIndexMethod;
/// }
/// 
/// void MyAdvanceColliderIndexMethod(XRGesture selectedGesture, Collider triggeredCollider, int currentIndexLocation)
/// {
///     // Custom collider index advancement logic
///     // Implement your logic here
/// }
/// </code>
/// </remarks>
public delegate void AdvanceColliderIndexDelegate(XRGesture selectedGesture, Collider triggeredCollider, int currentIndexLocation);


//#region Data Structures 
//#region struct hell

///// <summary>
///// Represents a gesture in XR.
///// </summary>
//[System.Serializable]
//public struct XRGesture
//{
//    /// <summary>
//    /// The name of the gesture.
//    /// </summary>
//    [Tooltip("The name of the gesture.")]
//    [SerializeField] string m_gestureName;
//    public string GestureName
//    {
//        get { return m_gestureName; }
//    }


//    /// <summary>
//    /// Determines whether the gesture is enabled or not.
//    /// </summary>
//    [Tooltip("Determines whether the gesture is enabled or not.")]
//    [SerializeField] private bool _gestureEnabled;
//    /// <summary>
//    /// Gets or sets a value indicating whether the gesture is enabled or not.
//    /// </summary>
//    public bool gestureEnabled
//    {
//        get { return _gestureEnabled; }
//        set { _gestureEnabled = value; }
//    }


//    /// <summary>
//    /// Private backing field for the RequirePlacement property.
//    /// </summary>
//    [SerializeField] private bool m_requirePlacement;
//    /// <summary>
//    /// Gets or sets a value indicating whether placement is required.
//    /// </summary>
//    /// <remarks>
//    /// Set this to true if placement is required; otherwise, set it to false.
//    /// </remarks>
//    public bool RequirePlacement
//    {
//        get { return m_requirePlacement; }
//        set { m_requirePlacement = value; }
//    }


//    //## i would hide this if the bool above was false
//    /// <summary>
//    /// The gesture placement for the object.
//    /// </summary>
//    /// <remarks>
//    /// This property determines the placement of the gesture on the object.
//    /// </remarks>
//    [SerializeField] private GesturePlacement m_gesturePlacement;
//    /// <summary>
//    /// Gets or sets the gesture placement for the object.
//    /// </summary>
//    /// <value>The gesture placement.</value>
//    /// <remarks>
//    /// Use this property to get or set the placement of the gesture on the object.
//    /// </remarks>
//    public GesturePlacement GesturePlacement
//    {
//        get { return m_gesturePlacement; }
//        set { m_gesturePlacement = value; }
//    }


//    /// <summary>
//    /// The timeout value for the gesture.
//    /// </summary>
//    [Tooltip("The timeout value for the gesture.")]
//    [SerializeField] private float m_gestureTimeout;
//    public float GestureTimeout
//    {
//        get { return m_gestureTimeout; }
//        set { m_gestureTimeout = value; }
//    }


//    public bool gestureInProgress;



//    /// <summary>
//    /// The list of relevant gesture objects for the gesture.
//    /// </summary>
//    [Tooltip("The list of relevant objects that are to be tracked for the gesture.")]
//    [SerializeField] private List<XRGestureObject> m_relevantGestureObjects;
//    public List<XRGestureObject> RelevantGestureObjects
//    {
//        get { return m_relevantGestureObjects; }
//        set { m_relevantGestureObjects = value; }
//    }


//    /// <summary>
//    /// The observation object used for detection.
//    /// </summary>
//    [Tooltip("the gestures start position.")]
//    [SerializeField] private GameObject m_observationObject;
//    /// <summary>
//    /// Gets or sets the observation object.
//    /// </summary>
//    /// <value>The observation object of the gesture.</value>
//    public GameObject ObservationObject
//    {
//        get { return m_observationObject; }
//        set { m_observationObject = value; }
//    }


//    /// <summary>
//    /// The current index location.
//    /// </summary>
//    [Tooltip("The current index value.")]
//    [SerializeField] private int currentIndexLocation;
//    /// <summary>
//    /// Gets or sets the current index location.
//    /// </summary>
//    /// <value>The current index location.</value>
//    public int CurrentIndexLocation
//    {
//        get { return currentIndexLocation; }
//        set { currentIndexLocation = value; }
//    }


//    /// <summary>
//    /// The input trigger type for the gesture.
//    /// </summary>
//    [Tooltip("The input trigger type for the gesture.")]
//    [SerializeField] private GestureInputTriggerType m_GestureInputType;
//    /// <summary>
//    /// Gets or sets the input trigger type for the gesture.
//    /// </summary>
//    public GestureInputTriggerType GestureInputType
//    {
//        get { return m_GestureInputType; }
//        set { m_GestureInputType = value; }
//    }


//    //ya know waht, we can add rotational gestures in later as an extension. i give up :/

//    /// <summary>
//    /// the ordered collection of the detection flags used for a gesture.
//    /// </summary>
//    [Tooltip("the ordered collection of the detection flags.")]
//    [SerializeField] private List<detectionColliderData> m_detectionColliderData;
//    public List<detectionColliderData> DetectionColliderData
//    {
//        get { return m_detectionColliderData; }
//        set { m_detectionColliderData = value; }
//    }
//}


///// <summary>
///// Represents the initialization information for detection colliders.
///// </summary>
//[System.Serializable]
//public struct detectionColliderData
//{
//    /// <summary>
//    /// The game object of the collider.
//    /// </summary>
//    public GameObject colliderGameobject;


//    /// <summary>
//    /// The number in the series of this particluar gesture.
//    /// </summary>
//    public int seriesNumber;

//    /// <summary>
//    /// Indicates whether the collider has reached the detection point.
//    /// </summary>
//    public bool reachedDetectionPoint;

//    /// <summary>
//    /// The transform of the collider object.
//    /// </summary>
//    public Transform colliderObjectTransform;

//    /// <summary>
//    /// The position the collider should be placed
//    /// </summary>
//    public Vector3 colliderPlacementPosition;

//    /// <summary>
//    /// The collider component of the object.
//    /// </summary>
//    public Collider objectCollider;

//    /// <summary>
//    /// The detection shape settings for the gesture.
//    /// </summary>
//    [SerializeField] private DetectionShapeSettings m_detectionShapeInfo;
//    /// <summary>
//    /// Gets or sets the detection shape settings.
//    /// </summary>
//    [Tooltip("The detection shape settings for the gesture.")]
//    public DetectionShapeSettings DetectionShapeInfo
//    {
//        get { return m_detectionShapeInfo; }
//        set { m_detectionShapeInfo = value; }
//    }
//}


///// <summary>
///// Represents the data about an object tracked for an XR gesture
///// </summary>
//[System.Serializable]
//public struct XRGestureObject
//{
//    /// <summary>
//    /// The name of the gesture object.
//    /// </summary>
//    [SerializeField] private string m_name;

//    /// <summary>
//    /// Gets or sets the name of the gesture object.
//    /// </summary>
//    /// <value>The name of the gesture object.</value>
//    public string Name
//    {
//        get { return m_name; }
//        set { m_name = value; }
//    }

//    /// <summary>
//    /// The game object of the gesture object.
//    /// </summary>
//    [SerializeField] private GameObject m_gestureObject;

//    /// <summary>
//    /// Gets or sets the game object of the gesture object.
//    /// </summary>
//    /// <value>The game object of the gesture object.</value>
//    public GameObject GestureObject
//    {
//        get { return m_gestureObject; }
//        set { m_gestureObject = value; }
//    }

//    /// <summary>
//    /// The placement of the gesture object.
//    /// </summary>
//    [SerializeField] private GesturePlacement m_placement;

//    /// <summary>
//    /// Gets or sets the placement of the gesture object.
//    /// </summary>
//    /// <value>The placement of the gesture object.</value>
//    public GesturePlacement Placement
//    {
//        get { return m_placement; }
//        set { m_placement = value; }
//    }

//    /// <summary>
//    /// The start action of the gesture object.
//    /// </summary>
//    [SerializeField] private InputActionProperty m_startAction;

//    /// <summary>
//    /// Gets or sets the start action of the gesture object.
//    /// </summary>
//    /// <value>The start action of the gesture object.</value>
//    public InputActionProperty StartAction
//    {
//        get { return m_startAction; }
//        set { m_startAction = value; }
//    }
//}


///// <summary>
///// Represents the detection shape settings for a gesture.
///// </summary>
//[System.Serializable]
//public struct DetectionShapeSettings
//{
//    /// <summary>
//    /// The primitive type for the detection shape.
//    /// </summary>
//    [SerializeField] private PrimitiveType m_primitiveDetectionShape;

//    /// <summary>
//    /// Gets or sets the primitive type for the detection shape.
//    /// </summary>
//    public PrimitiveType PrimitiveDetectionShape
//    {
//        get { return m_primitiveDetectionShape; }
//        set { m_primitiveDetectionShape = value; }
//    }

//    /// <summary>
//    /// The size of the detection shape.
//    /// </summary>
//    [SerializeField] private float m_primitiveDetectionShapeSize;

//    /// <summary>
//    /// Gets or sets the size of the detection shape.
//    /// </summary>
//    public float PrimitiveDetectionShapeSize
//    {
//        get { return m_primitiveDetectionShapeSize; }
//        set { m_primitiveDetectionShapeSize = value; }
//    }

//    /// <summary>
//    /// Specifies whether to use a custom scaled size for the detection shape.
//    /// </summary>
//    [SerializeField] private bool m_useScaledSize;

//    /// <summary>
//    /// Gets or sets a value indicating whether to use a custom scaled size for the detection shape.
//    /// </summary>
//    public bool UseScaledSize
//    {
//        get { return m_useScaledSize; }
//        set { m_useScaledSize = value; }
//    }

//    /// <summary>
//    /// The custom scaled size for the detection shape.
//    /// </summary>
//    [SerializeField] private Vector3 m_customScaledSize;

//    /// <summary>
//    /// Gets or sets the custom scaled size for the detection shape.
//    /// </summary>
//    public Vector3 CustomScaledSize
//    {
//        get { return m_customScaledSize; }
//        set { m_customScaledSize = value; }
//    }
//}


///// <summary>
///// Represents a timer coroutine associated with an XRGesture.
///// </summary>
//public struct TimerCoroutine
//{
//    /// <summary>
//    /// The XRGesture associated with this timer coroutine.
//    /// </summary>
//    public XRGesture coroutinesGesture;

//    /// <summary>
//    /// The actual coroutine that represents the timer.
//    /// </summary>
//    public Coroutine timerCoroutine;

//    ///<summary>
//    /// The gameobject associated with this timer coroutine.
//    ///</summary>
//    public GameObject coroutineGameobject;
//}

//#endregion


//#region enum hellscape
///// <summary>
///// Represents the placement options for a gesture.
///// </summary>
//[System.Serializable]
//public enum GesturePlacement
//{
//    /// <summary>
//    /// The gesture is associated with the left hand.
//    /// </summary>
//    LeftHand,

//    /// <summary>
//    /// The gesture is associated with the right hand.
//    /// </summary>
//    RightHand,

//    /// <summary>
//    /// The gesture is associated with the head.
//    /// </summary>
//    Head,

//    /// <summary>
//    /// The gesture is not associated one of the predefined sides.
//    /// </summary>
//    Other,

//    /// <summary>
//    /// the gesture is not associated with any side.
//    /// </summary>
//    None
//}


///// <summary>
///// Represents the input trigger types for a gesture.
///// </summary>
//[System.Serializable]
//public enum GestureInputTriggerType
//{
//    /// <summary>
//    /// The gesture is triggered by a trigger input.
//    /// </summary>
//    Trigger,

//    /// <summary>
//    /// The gesture is triggered by a grip input.
//    /// </summary>
//    Grip,

//    /// <summary>
//    /// The gesture is triggered by a button input.
//    /// </summary>
//    Button,

//    /// <summary>
//    /// The gesture is triggered by a position input.
//    /// </summary>
//    Position,

//    /// <summary>
//    /// The gesture is triggered by a rotation input.
//    /// </summary>
//    Rotation,

//    /// <summary>
//    /// The gesture is triggered by other input types.
//    /// </summary>
//    Other
//}


//#endregion
//#endregion


#region Notes
//Notes
#endregion