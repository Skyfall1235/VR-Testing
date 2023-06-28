using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class XRBaseColliderGestureController : MonoBehaviour
{
    //we will need the input action proterties, as well as allow this method to be overridded to allow for addtional inputs
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    //somewhere we will need to require a greater collider for the detection of when the actions can even be allowed to occuur.


    //set a runtime for the object to created when a series of possible triggers are selected (probably determined by enum
    public enum GestureInputTriggerType 
    {
        Trigger,
        Grip,
        button,
        Position,
        Rotation,
        Other
    }

    [SerializeField] protected GestureInputTriggerType m_GestureInputType;
    public GestureInputTriggerType gestureInputType
    {
        get { return m_GestureInputType; }
        set { m_GestureInputType = value; }

    }


    //we need to protect the gameobject that is spawned, buty it also needed to be an inhereited memeber for future scripts
    //observation object for the trigger
    [SerializeField] protected  GameObject m_observationObject;
    public GameObject ObservationObject
    {
        get { return m_observationObject; }
        set { m_observationObject = value; }
    }


    //for the primitve shape we plan to use
    
    /// <summary>
    /// Represents the settings for detection shape.
    /// </summary>
    [Serializable]
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
    /// <summary>
    /// The detection shape information.
    /// </summary>
    [SerializeField] protected DetectionShapeSettings m_detectionShapeInfo = new DetectionShapeSettings();
    public DetectionShapeSettings DetectionShapeInfo
    {
        get { return m_detectionShapeInfo; }
        set { m_detectionShapeInfo = value; }
    }




    protected GameObject CreateColliderObject( DetectionShapeSettings shapeSettings)
    {
        GameObject colliderObject = GameObject.CreatePrimitive(shapeSettings.PrimitiveDetectionShape);

        Collider collider = colliderObject.GetComponent<Collider>();
        
        if (shapeSettings.UseScaledSize)
        {
            // Set custom scaled size if specified
            colliderObject.transform.localScale = shapeSettings.CustomScaledSize;
        }
        else
        {
            // Set scaled size based on primitive detection shape size
            colliderObject.transform.localScale = new Vector3(1f, 1f, 1f) * shapeSettings.PrimitiveDetectionShapeSize;
        }

        return colliderObject;
    }

    /// <summary>
    /// Scales a vector by the specified size value to create a new scaled vector.
    /// </summary>
    /// <param name="sizeToScaleTo">The size value to scale the vector by.</param>
    /// <returns>The new scaled vector.</returns>
    private Vector3 ScaleObjectbyPrimitive(float sizeToScaleTo)
    {
        Vector3 primitiveScaledVector = new Vector3(1,1,1) * sizeToScaleTo;
        return primitiveScaledVector;
    }

    //wee need to create the gamebojest with the collider


    //method to protectively summon tyhe object to a given position and rotation in local space but also allowing an easy method to get its real space location

}


