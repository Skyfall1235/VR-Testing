using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class XRCreationMenuItems
{

    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture", false, 10)]
    static void CreateCustomGesture(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture");
        go.AddComponent<XRGesture>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture Manager", false, 10)]
    static void CreateXRGestureManager(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture Manager");
        go.AddComponent<XRBaseColliderGestureControllerV3>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }


    [MenuItem("GameObject/XR/XR Gesture/Custom Gesture Collider", false, 10)]
    static void CreateXRGestureCollider(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("XR Gesture Collider");
        go.AddComponent<XRGestureCollisionReporterV2>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
