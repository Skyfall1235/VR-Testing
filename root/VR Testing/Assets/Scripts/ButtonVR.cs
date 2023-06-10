using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textToModify;
    [SerializeField] private string displaytext = "you made a button!";

    public void DisplayTextToUser()
    {
        if (textToModify != null)
        {
            textToModify.text = displaytext;
        }
    }


}
