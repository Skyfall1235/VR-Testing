using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magazine : MonoBehaviour
{
    public MagazineData MagazineData = new MagazineData(gunType.rifle, GunPart.Magazine, 20, 20);

    
}
public struct MagazineData 
{
    public gunType associatedGun;
    public GunPart part;
    public int maxAmmoAmount;
    public int currentAmmo;

    public MagazineData(gunType associatedGun, GunPart part, int maxAmmoAmount, int currentAmmo)
    {
        this.associatedGun = associatedGun;
        this.part = part;
        this.maxAmmoAmount = maxAmmoAmount;
        this.currentAmmo = currentAmmo;
    }
}

