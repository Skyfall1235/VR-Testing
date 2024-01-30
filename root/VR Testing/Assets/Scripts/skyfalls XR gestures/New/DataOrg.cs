using UnityEngine.InputSystem;
using UnityEngine;

#region Data Structures 

#region struct hell

/// <summary>
/// Represents the initialization information for detection colliders.
/// </summary>
[System.Serializable]
public struct detectionColliderData
{
    /// <summary>
    /// The number in the series of this particluar gesture.
    /// </summary>
    public int seriesNumber;

    /// <summary>
    /// The game object of the collider.
    /// </summary>
    public GameObject colliderGameobject;

    /// <summary>
    /// Indicates whether the collider has reached the detection point.
    /// </summary>
    public bool reachedDetectionPoint;


    //TEMP : if not using custom collider, use basic collider
    public bool useCustomCollider;
    public Collider customCollider;

    public DetectionShapeSettings basicCollider;

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

    public XRGestureObject(string name, GameObject trackedGameObject, InputActionProperty startAction)
    {
        this.m_name = name;
        this.m_trackedObject = trackedGameObject;
        this.m_startAction = startAction;
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

