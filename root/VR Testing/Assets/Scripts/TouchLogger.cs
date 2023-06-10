using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchLogger : MonoBehaviour
{
    public TextMeshProUGUI logText;

    private void Awake()
    {
        // Make sure you have a reference to the TextMeshProUGUI component
        if (logText == null)
        {
            Debug.LogError("Log Text component is not assigned!");
        }
    }

    public void DisplayLog(string logMessage)
    {
        // Append the new log message to the existing text with a new line
        logText.text += logMessage + "\n";
    }
}

public class XRSelectionLog : XRBaseInteractable
{
    public TouchLogger logDisplay;

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);

        // Log and display the selection
        string selectionLog = "Selected: " + gameObject.name;
        logDisplay.DisplayLog(selectionLog);
    }
}
