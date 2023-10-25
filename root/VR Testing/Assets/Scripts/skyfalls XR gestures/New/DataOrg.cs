
#region Data Structures 
#region struct hell

using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
/// <summary>
/// Represents a gesture in XR.
/// </summary>
[System.Serializable]
public struct XRGesture
{
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
    [SerializeField] private bool _gestureEnabled;
    /// <summary>
    /// Gets or sets a value indicating whether the gesture is enabled or not.
    /// </summary>
    public bool gestureEnabled
    {
        get { return _gestureEnabled; }
        set { _gestureEnabled = value; }
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


    public bool gestureInProgress;



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


    /// <summary>
    /// The observation object used for detection.
    /// </summary>
    [Tooltip("the gestures start position.")]
    [SerializeField] private GameObject m_observationObject;
    /// <summary>
    /// Gets or sets the observation object.
    /// </summary>
    /// <value>The observation object of the gesture.</value>
    public GameObject ObservationObject
    {
        get { return m_observationObject; }
        set { m_observationObject = value; }
    }


    /// <summary>
    /// The current index location.
    /// </summary>
    [Tooltip("The current index value.")]
    [SerializeField] private int currentIndexLocation;
    /// <summary>
    /// Gets or sets the current index location.
    /// </summary>
    /// <value>The current index location.</value>
    public int CurrentIndexLocation
    {
        get { return currentIndexLocation; }
        set { currentIndexLocation = value; }
    }


    /// <summary>
    /// The input trigger type for the gesture.
    /// </summary>
    [Tooltip("The input trigger type for the gesture.")]
    [SerializeField] private GestureInputTriggerType m_GestureInputType;
    /// <summary>
    /// Gets or sets the input trigger type for the gesture.
    /// </summary>
    public GestureInputTriggerType GestureInputType
    {
        get { return m_GestureInputType; }
        set { m_GestureInputType = value; }
    }


    //ya know waht, we can add rotational gestures in later as an extension. i give up :/

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
}


/// <summary>
/// Represents the initialization information for detection colliders.
/// </summary>
[System.Serializable]
public struct detectionColliderData
{
    /// <summary>
    /// The game object of the collider.
    /// </summary>
    public GameObject colliderGameobject;


    /// <summary>
    /// The number in the series of this particluar gesture.
    /// </summary>
    public int seriesNumber;

    /// <summary>
    /// Indicates whether the collider has reached the detection point.
    /// </summary>
    public bool reachedDetectionPoint;

    /// <summary>
    /// The detection shape settings for the gesture.
    /// </summary>
    [SerializeField] private DetectionShapeSettings m_detectionShapeInfo;
    /// <summary>
    /// Gets or sets the detection shape settings.
    /// </summary>
    [Tooltip("The detection shape settings for the gesture.")]
    public DetectionShapeSettings DetectionShapeInfo
    {
        get { return m_detectionShapeInfo; }
        set { m_detectionShapeInfo = value; }
    }
}


/// <summary>
/// Represents the data about an object tracked for an XR gesture
/// </summary>
[System.Serializable]
public struct XRGestureObject
{
    /// <summary>
    /// The name of the gesture object.
    /// </summary>
    [SerializeField] private string m_name;

    /// <summary>
    /// Gets or sets the name of the gesture object.
    /// </summary>
    /// <value>The name of the gesture object.</value>
    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    /// <summary>
    /// The game object of the gesture object.
    /// </summary>
    [SerializeField] private GameObject m_trackedObject;

    /// <summary>
    /// Gets or sets the game object of the gesture object.
    /// </summary>
    /// <value>The game object of the gesture object.</value>
    public GameObject GestureObject
    {
        get { return m_trackedObject; }
        set { m_trackedObject = value; }
    }

    /// <summary>
    /// The start action of the gesture object.
    /// </summary>
    [SerializeField] private InputActionProperty m_startAction;

    /// <summary>
    /// Gets or sets the start action of the gesture object.
    /// </summary>
    /// <value>The start action of the gesture object.</value>
    public InputActionProperty StartAction
    {
        get { return m_startAction; }
        set { m_startAction = value; }
    }
}


/// <summary>
/// Represents the detection shape settings for a gesture.
/// </summary>
[System.Serializable]
public struct DetectionShapeSettings
{
    /// <summary>
    /// The primitive type for the detection shape.
    /// </summary>
    [SerializeField] private PrimitiveType m_primitiveDetectionShape;

    /// <summary>
    /// Gets or sets the primitive type for the detection shape.
    /// </summary>
    public PrimitiveType PrimitiveDetectionShape
    {
        get { return m_primitiveDetectionShape; }
        set { m_primitiveDetectionShape = value; }
    }

    /// <summary>
    /// The size of the detection shape.
    /// </summary>
    [SerializeField] private float m_primitiveDetectionShapeSize;

    /// <summary>
    /// Gets or sets the size of the detection shape.
    /// </summary>
    public float PrimitiveDetectionShapeSize
    {
        get { return m_primitiveDetectionShapeSize; }
        set { m_primitiveDetectionShapeSize = value; }
    }

    /// <summary>
    /// Specifies whether to use a custom scaled size for the detection shape.
    /// </summary>
    [SerializeField] private bool m_useScaledSize;

    /// <summary>
    /// Gets or sets a value indicating whether to use a custom scaled size for the detection shape.
    /// </summary>
    public bool UseScaledSize
    {
        get { return m_useScaledSize; }
        set { m_useScaledSize = value; }
    }

    /// <summary>
    /// The custom scaled size for the detection shape.
    /// </summary>
    [SerializeField] private Vector3 m_customScaledSize;

    /// <summary>
    /// Gets or sets the custom scaled size for the detection shape.
    /// </summary>
    public Vector3 CustomScaledSize
    {
        get { return m_customScaledSize; }
        set { m_customScaledSize = value; }
    }
}


#endregion


#region enums


/// <summary>
/// Represents the input trigger types for a gesture.
/// </summary>
[System.Serializable]
public enum GestureInputTriggerType
{
    /// <summary>
    /// The gesture is triggered by a trigger input.
    /// </summary>
    Trigger,

    /// <summary>
    /// The gesture is triggered by a grip input.
    /// </summary>
    Grip,

    /// <summary>
    /// The gesture is triggered by a button input.
    /// </summary>
    Button,

    /// <summary>
    /// The gesture is triggered by a position input.
    /// </summary>
    Position,

    /// <summary>
    /// The gesture is triggered by a rotation input.
    /// </summary>
    Rotation,

    /// <summary>
    /// The gesture is triggered by other input types.
    /// </summary>
    Other
}


#endregion
#endregion

