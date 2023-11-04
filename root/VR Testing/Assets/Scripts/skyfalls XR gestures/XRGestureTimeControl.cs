using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class XRGestureTimeControl : MonoBehaviour
{
    #region Member Variables

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
    /// The timeout value for the specific XRGesture associated with the XRGestureTimeControl.
    /// </summary>
    [SerializeField]
    public float m_associatedGestureTimeOut
    {
        get { return m_associatedGesture.GestureTimeout; }
    }
    #endregion
    bool gestureTimerIsRunning = false;
    bool indexerTimerIsRunning = false;
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
    public void StartGestureTimer()
    {
        // Check if a timer is already running for the selected gesture.
        if (gestureTimerIsRunning)
        {
            Debug.Log($"A timer is already running for gesture {m_associatedGesture.GestureName}");
        }
        // Start a new timer coroutine for the index timer
        Coroutine indexerCoroutine = StartCoroutine(CustomTimer(CustomTimerSeconds));
        //start a new timer for the Gesture as a whole
        Coroutine GestureCoroutine = StartCoroutine(CustomTimer(m_associatedGesture.GestureTimeout));

    }

    //timer for between gestures

    //timer for gesture as a whole


    
    public void StopIterationTimer() 
    {
        if (indexerTimerIsRunning)
        {

        }
    }
    public void StopGestureTimer()
    {
        if (gestureTimerIsRunning)
        {
            StopCoroutine
        }
    }




    /// <summary>
    /// A custom timer coroutine for handling XRGesture timeouts.
    /// </summary>
    /// <param name="delaySeconds">The delay in seconds before the timer executes.</param>
    /// <param name="gesture">The XRGesture associated with the timer.</param>
    /// <returns>An IEnumerator representing the timer coroutine.</returns>
    IEnumerator CustomTimer(float delaySeconds)
    {
        // Wait for the specified delay before executing the rest of the coroutine.
        yield return new WaitForSeconds(delaySeconds);

        // Log a message indicating that the timer has completed.
        Debug.Log($"Timer instance {this} failed");

        // Handle the timeout for the associated XRGesture.
        // For example, cancel the gesture and reset the index to 0.
        m_associatedGesture.CurrentIndexLocation = 0;
        m_associatedGesture.gestureInProgress = false;
    }

    #endregion


}