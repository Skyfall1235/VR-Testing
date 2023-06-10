using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//im just gonna make this simple and not a class. it can become a class when i need it to
public class Rifle : MonoBehaviour
{
    //basic values for gun
    [SerializeField] GameObject projectile;
    [SerializeField] Transform instatiationPoint;
    [SerializeField] XRSocketInteractor magazineSocket;

    //needed to select and select exit the boject
    [SerializeField] GameObject currentMagazineObject;
    [SerializeField] Magazine currentEquipedMagazine;
    

    private int currentAmmo = 0;
    private bool roundChambered = false;


    //get these from magazine



    //methods to fire projectile

    public void HandleShoot()
    {
        //check if have ammo.
        //if has ammo, deduct 1 and call the shoot method

    }

    public void shoot()
    {
        if (roundChambered)
        {
            //shoot the projectile
            roundChambered = false;
        }
        //if ammo is left, chamber
        
        RackRound();
    }

    public void RackRound()
    {
        if(currentAmmo < 0)
        {
            roundChambered = true;
            //eject a casing
        }
        else
        {
            roundChambered = true;
            //eject last casing
        }
    }

    //magazineControl
    //should only run on an attach or detach
    public void GetMagazineInfo()
    {
        //get the magazine fromthe socket and save it locally
        IXRSelectInteractable magazineInteractable = magazineSocket.GetOldestInteractableSelected();
        GameObject magazineObject = magazineInteractable.transform.gameObject;
        Magazine magazineData = magazineObject.GetComponent<Magazine>();

        //now, set the current magazine to the
    }   
    
    private void AssignMagazineInfo(Magazine magazine)
    {
        XRBaseInteractor socketInteractor = GetComponent<XRBaseInteractor>();
        //may need to run everytime shoot is called, and on the enter of the magazine
        XRSocketInteractor interactable = GetComponent<XRSocketInteractor>();
        //yourSocket.interactionManager.SelectExit(yourSocket, itemSocketed)
        socketInteractor.ForceDeselect();
    }

    //run on detach
    public void PurgeMagazineInfo()
    {

    }

    //method to handle Gun UI

    private void ShowUI()
    {

    }
}
