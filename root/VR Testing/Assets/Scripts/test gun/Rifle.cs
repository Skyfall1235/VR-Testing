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
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] Transform instatiationPoint;
    [SerializeField] XRSocketInteractor magazineSocket;

    //needed to select and select exit the boject
    [SerializeField] GameObject currentMagazineObject;
    [SerializeField] MagazineData currentEquipedMagazine;

    gunType rifleType = gunType.rifle;
    GunPart riflePart = GunPart.Gun;

    private int currentAmmo = 0;
    private bool roundChambered = false;

    [SerializeField] private Sighthandler sighthandler;

    //the guns check of all the magzine data



    //methods to fire projectile

    public void HandleShoot()
    {
        //check if have ammo.
        //if has ammo, deduct 1 and call the shoot method
        GameObject projectile = Instantiate(projectilePrefab, instatiationPoint.position, instatiationPoint.rotation);
        //give it force at the location relative to the direction of the user
        //projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, projectileLaunchVelocity));

    }

    public void shoot()
    {
        if (roundChambered && currentMagazineObject != null )
        {
            currentEquipedMagazine.currentAmmo--;
            AssignMagazineInfo(currentEquipedMagazine);
            //shoot the projectile
            roundChambered = false;
        }
        //if ammo is left, chamber
        
        RackRound();
    }
    private void AssignMagazineInfo(MagazineData magazineData)
    {
        currentAmmo = magazineData.currentAmmo;

    }
    private void RetrieveAmmoInformation()
    {

    }

    public void RackRound()
    {
        if(currentAmmo < 0)
        {
            roundChambered = true;
            sighthandler.ToggleChamberedIcon(roundChambered);
            //eject a casing
        }
        else
        {
            roundChambered = false;
            sighthandler.ToggleChamberedIcon(roundChambered);
            //eject last casing
        }
    }

    //magazineControl
    //should only run on an attach or detach
    public void GetMagazineInfo(GameObject magazine)
    {
        //get the magazine fromthe socket and save it locally
        //IXRSelectInteractable magazineInteractable = magazineSocket.GetOldestInteractableSelected();
        //Debug.Log(magazineInteractable);
        currentMagazineObject = magazine;
        currentEquipedMagazine = currentMagazineObject.GetComponent<Magazine>().MagazineData;
        Debug.Log(currentMagazineObject.GetComponent<Magazine>());

        //if the magazine is the approrpiate magazine, do this
        if (currentEquipedMagazine.associatedGun == rifleType && currentEquipedMagazine.part == riflePart)
        {
            AssignMagazineInfo(currentEquipedMagazine);
            Debug.Log("storing the data from the magazine");
        }
        
        //else, purge he magazine info and turn off the
    }   
    


    //run on detach
    public void PurgeMagazineInfo()
    {

    }

    //method to handle Gun UI

    private void UpdateUI()
    {
        sighthandler.SetAmmoCounter(currentEquipedMagazine.currentAmmo, currentEquipedMagazine.maxAmmoAmount);
    }
}


