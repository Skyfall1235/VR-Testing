using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRJoyStick : XRGrabInteractable
{
    public UnityEvent OnUseStarted = new();
    public UnityEvent OnUseStopped = new();

    [Header("Joystick")]
    [SerializeField] Transform _handle;

    [SerializeField] float _maxXAngle = 45;

    [SerializeField] float _maxZAngle = 45;

    [SerializeField] [Range(0, 1)]
    float _vibrationStrength = .1f;

    [SerializeField] [Range(0, 1)]
    float _vibrationInterval = .05f;

    public float XPercentage;
    public float ZPercentage;

    bool _grabbing;

    // Update is called once per frame
    void Update()
    {
        var angleX = _handle.localRotation.eulerAngles.x;
        if (angleX > 180)
            angleX -= 360;

        var angleZ = _handle.localRotation.eulerAngles.z;
        if (angleZ > 180)
            angleZ -= 360;

        XPercentage = angleX / _maxXAngle;
        ZPercentage = angleZ / _maxZAngle;
    }

    public void OnGrab()
    {
        //allows the grab to begin and track the maximum angular travel for the joystick
        _grabbing = true;
        StartCoroutine(DetectJoystickMovement());
    }
    public void OnGrabRelease()
    {
        _grabbing = false;
    }

    IEnumerator DetectJoystickMovement()
    {
        var currentXPercentage = XPercentage;
        var currentZPercentage = ZPercentage;
        while (_grabbing)
        {
            if (Mathf.Abs(currentXPercentage - XPercentage) > _vibrationInterval)
            {
                //haptic event
                currentXPercentage = XPercentage;
            }
            else if (Mathf.Abs(currentZPercentage - ZPercentage) > _vibrationInterval)
            {
                //haptic event
                currentZPercentage = ZPercentage;
            }
            //lets go to the next frame
            yield return null;
        }
    }
}