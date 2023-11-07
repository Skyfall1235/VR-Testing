using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Events;
using System.Linq;

public class XRCreationMenuItems
{
    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture", false, 10)]
    static void CreateCustomGesture(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = XRGestureSetup(menuCommand);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture Manager", false, 10)]
    static void CreateXRGestureManager(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = XRManagerSetup(menuCommand);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture Collider", false, 10)]
    static void CreateXRGestureCollider(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = XRColliderSetup(menuCommand);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


    #region Constructors

    private static GameObject XRManagerSetup(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture Manager");
        go.AddComponent<XRBaseColliderGestureControllerV3>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Set the gameobject to the XR Gesture Layer
        go.layer = 12;
        return go;
    }


    private static GameObject XRGestureSetup(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture");

        // Add and link the Gesture and time controller scripts
        XRGesture XRG = go.AddComponent<XRGesture>();
        XRGestureTimeControl XRGTC = go.AddComponent<XRGestureTimeControl>();

        //link gesture and time controller
        XRGTC.AssociatedGesture = XRG;

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // Set the gameobject to the XR Gesture Layer
        go.layer = 12;
        return go;
    }


    private static GameObject XRColliderSetup(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture Collider");

        // Add the collision reporter to the game object
        go.AddComponent<BoxCollider>();
        go.AddComponent<XRGestureCollisionReporterV2>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // retrieve the objects that are to be tracked from the parent and save them in the reporter
        if (go.transform.parent.GetComponent<XRGesture>() != null)
        {
            // Assign and cache the components we wish to add and link
            XRGesture parentXRG = go.transform.parent.GetComponent<XRGesture>();
            XRGestureCollisionReporterV2 reporter = go.GetComponent<XRGestureCollisionReporterV2>();

            // Set the tracked object to the reporter
            reporter.TrackedGestureObjects = parentXRG.RelevantGestureObjects;

            //Create a new unity event, store it in the Gesture, and assign it to the OnTriggerEvent in the reporter
            parentXRG.OnGestureCollideEvent.Add(new UnityEvent());
            UnityEvent newEvent = parentXRG.OnGestureCollideEvent.LastOrDefault();
            reporter.OnTriggerEvent = newEvent;
        }
        else
        {
            Debug.LogWarning($" Could not assign tracked object list to collider ID:{go.GetInstanceID()} \n Please assign in the inspector");
        }

        // Set the gameobject to the XR Gesture Layer
        go.layer = 12;
        return go;
    }

    #endregion
}
