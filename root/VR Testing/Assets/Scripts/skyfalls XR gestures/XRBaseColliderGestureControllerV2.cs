using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class XRBaseColliderGestureControllerV2 : MonoBehaviour
{
    [SerializeField] private bool m_gestureRecognitionEnabled;
    [SerializeField] private float m_gestureTimeoutThreshold;//in seconds
    [SerializeField] private BoxCollider m_activationBoxCollider;
    [SerializeField] private List<XRGesture> m_XRGestures = new(); //the list of each custom gesture
    [SerializeField] private List<XRGestureObject> m_trackedGestureObjects = new(); // what objects are to be tracked for which gesture
    //inputs



    //set up the collider objects
    private GameObject DetectionColliderSetup(DetectionShapeSettings settingsData)
    {
        // Create a primitive GameObject
        GameObject primitiveObject = GameObject.CreatePrimitive(settingsData.PrimitiveDetectionShape);

        // Remove the mesh renderer and mesh filter components
        Destroy(primitiveObject.GetComponent<MeshRenderer>());
        Destroy(primitiveObject.GetComponent<MeshFilter>());
        return primitiveObject;
    }
    private void SetupDetectionCollider(GameObject primativeObject, XRGesture gesture)
    {
        //get the list, add a new index item and save this info to it
    }
}




[System.Serializable]
public struct XRGesture
{
    [SerializeField] string m_gestureName;
    [SerializeField] float m_gestureTimeout;

    //constants
    const float detectionshapeBaseSize = 1f;
    const int SinglePointGestureColliderAmount = 1;


    [SerializeField] private List<XRGestureObject> relevantGestureObjects;//will be initialised with the objects needed
    [SerializeField] private GameObject m_observationObject; //observation object is the first collider that is spawned, and determines  hwere the next collider should be

    public GameObject ObservationObject
    {
        get { return m_observationObject; }
        set { m_observationObject = value; }
    }

    [SerializeField] private DetectionShapeSettings m_detectionShapeInfo;
    public DetectionShapeSettings DetectionShapeInfo
    {
        get { return m_detectionShapeInfo; }
        set { m_detectionShapeInfo = value; }
    }

    [SerializeField] private GestureInputTriggerType m_GestureInputType;
    public GestureInputTriggerType GestureInputType
    {
        get { return m_GestureInputType; }
        set { m_GestureInputType = value; }
    }
    //ya know waht, we can add rotational gestures in later as an extension. i give up :/

    private List<detectionColliderInitializationInfo> m_detectionColliderInitializationInfos;

}

[System.Serializable]
public struct detectionColliderInitializationInfo
{
    public GameObject colliderGameobject;
    //number in series
    public int seriesNumber;
    //if its been collided with the tracked object yet
    public bool reachedDetectionPoint;
    //its trasnform
    public Transform colliderObjectTransform;
    //its collider
    public Collider objectCollider;


}

[System.Serializable]
public struct XRGestureObject
{
    public string m_name;
    public GameObject m_gestureObject;
    public GesturePlacement m_placement;
    public InputActionProperty m_startAction;
}
[System.Serializable]
public struct DetectionShapeSettings
    {
        /// <summary>
        /// The primitive type for the detection shape.
        /// </summary>
        [SerializeField] private PrimitiveType m_primitiveDetectionShape;
        public PrimitiveType PrimitiveDetectionShape
        {
            get { return m_primitiveDetectionShape; }
            set { m_primitiveDetectionShape = value; }
        }

        /// <summary>
        /// The size of the detection shape.
        /// </summary>
        [SerializeField] private float m_primitiveDetectionShapeSize;
        public float PrimitiveDetectionShapeSize
        {
            get { return m_primitiveDetectionShapeSize; }
            set { m_primitiveDetectionShapeSize = value; }
        }

        /// <summary>
        /// Specifies whether to use a custom scaled size for the detection shape.
        /// </summary>
        [SerializeField] private bool m_useScaledSize;
        public bool UseScaledSize
        {
            get { return m_useScaledSize; }
            set { m_useScaledSize = value; }
        }

        /// <summary>
        /// The custom scaled size for the detection shape.
        /// </summary>
        [SerializeField] private Vector3 m_customScaledSize;
        public Vector3 CustomScaledSize
        {
            get { return m_customScaledSize; }
            set { m_customScaledSize = value; }
        }
    }

#region enum hellscape
[System.Serializable]
public enum GesturePlacement
{
    LeftHand,
    RightHand,
    Head
}
[System.Serializable]
public enum GestureInputTriggerType
{
    Trigger,
    Grip,
    button,
    Position,
    Rotation,
    Other
}
#endregion