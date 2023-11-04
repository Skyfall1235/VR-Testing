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
    Coroutine gestureTimer;
    Coroutine indexerTimer;
    float CustomTimerSeconds;


    private void Awake()
    {
        // Calculate the timer duration (CustomTimerSeconds) as the minimum value between m_associatedGestureTimeOut and m_globalTimeOut.
    }




    //gesture timer stuff
    #region Timer methods


    /// <summary>
    /// 
    /// </summary>
    public void StartGestureTimer()
    {
        // Check if a timer is already running for the selected gesture.
        if (gestureTimer != null)
        {
            Debug.Log($"A timer is already running for gesture {m_associatedGesture.GestureName}");
        }
        // Start a new timer coroutine for the index timer, using the individuals custom time amount
        indexerTimer = StartCoroutine(IterationTimer(CustomTimerSeconds));
        //start a new timer for the Gesture using the gestures timeout, which can be local or global
        gestureTimer = StartCoroutine(GestureTimer(m_associatedGesture.GestureTimeout));
    }


    //timer for between gestures

    public void IterateColliderTimer()
    {
        StopCoroutine(indexerTimer);
        //call back to the
    }

    //timer for gesture as a whole


    

    public void StopGestureTimer()
    {
        if (gestureTimer != null)
        {
            StopCoroutine(gestureTimer);
            StopCoroutine(indexerTimer);
            m_associatedGesture.CurrentIndexLocation = 0;
            m_associatedGesture.gestureInProgress = false;
            //log as a fail
        }
    }




    /// <summary>
    /// A custom timer coroutine for handling XRGesture timeouts.
    /// </summary>
    /// <param name="delaySeconds">The delay in seconds before the timer executes.</param>
    /// <returns>An IEnumerator representing the timer coroutine.</returns>
    IEnumerator GestureTimer(float delaySeconds)
    {
        // Wait for the specified delay before executing the rest of the coroutine.
        yield return new WaitForSeconds(delaySeconds);

        // Log a message indicating that the timer has completed.
        Debug.Log($"Gesture timer {this} failed");

        // Handle the timeout for the associated XRGesture.
        // For example, cancel the gesture and reset the index to 0.
        m_associatedGesture.CurrentIndexLocation = 0;
        m_associatedGesture.gestureInProgress = false;
        if(indexerTimer != null)
        {
            StopCoroutine(indexerTimer);
        }
    }

    IEnumerator IterationTimer(float delaySeconds)
    {
        yield return GestureTimer(delaySeconds);
        // Log a message indicating that the timer has completed.
        Debug.Log($"Iterator timer {this} failed");

    }


    #endregion


}