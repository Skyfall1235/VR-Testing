using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Sighthandler : MonoBehaviour
{
    //ref to the sight objects
    [SerializeField] Rifle connectedRifle;
    [SerializeField] bool chamberedBool = false;
    [SerializeField] TextMeshProUGUI chamberedText;
    [SerializeField] Image chamberedColor;
    [SerializeField] TextMeshProUGUI ammoDisplay;
    //display the current Ammo, whether or not the

    private void Update()
    {
        ToggleChamberedIcon(chamberedBool);
    }

    public void ToggleChamberedIcon(bool enabled)
    {
        chamberedBool = enabled;
        switch(chamberedBool)
        {
            case true:
                chamberedText.text = "CHAMBERED";
                chamberedColor.color = Color.green;
                break;
            case false:
                chamberedText.text = "EMPTY";
                chamberedColor.color = Color.red;
                break;
        }
        //toggle the color between green and Red, and change the text to Chambered or Empty
    }
    public void SetAmmoCounter(int currentAmmo, int maxAmmo)
    {
        ammoDisplay.text = $"{currentAmmo}/{maxAmmo}";
        //display the current ammo, only when the method is called. basicially just reference the rifle
    }
}
