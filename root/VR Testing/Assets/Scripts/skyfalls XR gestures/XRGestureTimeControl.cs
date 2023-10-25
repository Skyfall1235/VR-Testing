using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRGestureTimeControl : MonoBehaviour
{
    #region Member Variables
    [SerializeField]
    private XRBaseColliderGestureControllerV2 m_associatedController;
    /// <summary>
    /// The XRBaseColliderGestureControllerV2 associated with this XRGestureTimeControl.
    /// </summary>
    public XRBaseColliderGestureControllerV2 AssociatedController
    {
        set { m_associatedController = value; }
    }


    [SerializeField]
    private XRGesture m_associatedGesture;
    /// <summary>
    /// The XRGesture associated with this XRGestureTimeControl.
    /// </summary>
    public XRGesture AssociatedGesture
    {
        set { m_associatedGesture = value; }
    }


    /// <summary>
    /// The global timeout value for gestures associated with the XRGestureTimeControl's controller.
    /// </summary>
    [SerializeField]
    public float m_globalTimeOut
    {
        get { return m_associatedController.GestureTimeoutThreshold; }
    }

    /// <summary>
    /// The timeout value for the specific XRGesture associated with the XRGestureTimeControl.
    /// </summary>
    [SerializeField]
    public float m_associatedGestureTimeOut
    {
        get { return m_associatedGesture.GestureTimeout; }
    }
    #endregion

    float CustomTimerSeconds;


    private void Awake()
    {
        // Calculate the timer duration (CustomTimerSeconds) as the minimum value between m_associatedGestureTimeOut and m_globalTimeOut.
        CustomTimerSeconds = Mathf.Min(m_associatedGestureTimeOut, m_globalTimeOut);
    }




    //gesture timer stuff
    #region Timer methods


    /// <summary>
    /// Starts a timer coroutine associated with the specified XRGesture.
    /// </summary>
    /// <param name="selectedGesture">The XRGesture for which to start the timer.</param>
    public void StartTimer(XRGesture selectedGesture)
    {
        // Check if a timer is already running for the selected gesture.
        if (ContainsGestureTimer(selectedGesture))
        {
            Debug.Log($"A timer is already running for gesture {selectedGesture.GestureName}");
            return;
        }
        // Start a new timer coroutine for the selected gesture.
        Coroutine newTimerCoroutine = StartCoroutine(CustomTimer(CustomTimerSeconds, selectedGesture));
        // Create a new TimerCoroutine struct to hold information about the started timer.


        // Add the new TimerCoroutine struct to the list of timer coroutines associated with the controller.

    }


    /// <summary>
    /// Interrupts and reloads the specified timer coroutine with a new timer for the associated XRGesture.
    /// </summary>
    /// <param name="timerCoroutine">The TimerCoroutine to interrupt and reload.</param>
    /// <param name="cancelationToken">The boolean answer to cancel the reload and instead kill the coroutine</param>




    /// <summary>
    /// A custom timer coroutine for handling XRGesture timeouts.
    /// </summary>
    /// <param name="delaySeconds">The delay in seconds before the timer executes.</param>
    /// <param name="gesture">The XRGesture associated with the timer.</param>
    /// <returns>An IEnumerator representing the timer coroutine.</returns>
    IEnumerator CustomTimer(float delaySeconds, XRGesture gesture)
    {
        // Wait for the specified delay before executing the rest of the coroutine.
        yield return new WaitForSeconds(delaySeconds);

        // Log a message indicating that the timer has completed.
        Debug.Log($"Timer instance {this} failed");

        // Handle the timeout for the associated XRGesture.
        // For example, cancel the gesture and reset the index to 0.
        gesture.CurrentIndexLocation = 0;
        gesture.gestureInProgress = false;
    }

    #endregion


    /// <summary>
    /// Checks if a specific XRGesture is present in the list of timer coroutines associated with a controller.
    /// </summary>
    /// <param name="gesture">The XRGesture to search for.</param>
    /// <returns>Returns true if the specified XRGesture is found, otherwise false.</returns>
    public bool ContainsGestureTimer(XRGesture gesture)
    {

        return false; // The specified gesture is not found in the list of timer coroutines.
    }
}