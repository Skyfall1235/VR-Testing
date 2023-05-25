using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed;
    public float followAngleThres = 45;

    private bool freeze = false;

    [SerializeField] private Vector3 initialLocalPos;

    private Vector3 offset;
    [SerializeField] private Transform pokeAttachTransform;

    [SerializeField] private XRBaseInteractable interactable;
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;

        interactable = GetComponent<XRBaseInteractable>();
        Debug.Log(interactable);
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(ResetButton);
        interactable.selectEntered.AddListener(Freeze);
        Debug.Log("added all listeners");
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        Debug.Log("Hover entered: " + hover);
        if (hover.interactorObject is XRPokeInteractor)
        {
            
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;

            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));

            if (pokeAngle > followAngleThres)
            {
                isFollowing = false;
                freeze = true;
            }
        }
        
    }

    public void ResetButton(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isHoverEntered = interactable.hoverEntered.GetPersistentEventCount() > 0;
        Debug.Log(isHoverEntered);
        Debug.Log(interactable.hoverEntered.GetPersistentEventCount());
        if (freeze)
            return;
        if (isFollowing)
        {
            Debug.Log("pbutton is trying to follow players hand");
            Vector3 localTrargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTragetPosition = Vector3.Project(localTrargetPosition, localAxis);


            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTragetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }
}
