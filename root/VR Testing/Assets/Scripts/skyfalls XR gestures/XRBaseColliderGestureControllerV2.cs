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
    [SerializeField] private List<XRGesture> m_XRGestures = new();
    [SerializeField] private List<XRGestureObject> m_trackedGestureObjects = new();

    //inputs

}




[System.Serializable]
public struct XRGesture
{
    string m_gestureName;
    float m_gestureTimeout;


    private List<XRGestureObject> relevantGestureObjects;//will be initialised with the objects needed
    [SerializeField] private GameObject m_observationObject;
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

    [SerializeField] private GestureStyle m_selectedGestureStyle;
    public GestureStyle SelectedGestureStyle
    {
        get { return m_selectedGestureStyle; }
        set { m_selectedGestureStyle = value; }
    }




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
public enum GestureStyle
{
    SinglePoint,
    MultiPoint,
    RotationSingle,
    RotationMultiDirectional,
    FreeRotation
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




