using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class C_Firearm : MonoBehaviour
{
    //weight, bullet type, pettlet type, falloff distance,

    [SerializeField] private float falloffDistance;

    [SerializeField] private FiringMode firingMode;
    [SerializeField] private gunType gunType;
    [SerializeField] private GunPart gunPart;

    protected float weight;
    protected int magazineSizeMax;
    protected int currentAmmoInMagazine = 0;
    protected int currentAmmo;

    protected float accuracy; //0 is random up to 45degrees, 1 is dead center every time
    protected float fireRate;

    //should be in derivative classes
    [SerializeField] private bool pellets;
    [SerializeField] private float chamberedRound;
    [SerializeField] private bool rackedRound;


    //effects
    [SerializeField] protected AudioClip dropMagazine;
    [SerializeField] protected AudioClip loadMagazine;
    [SerializeField] protected AudioClip[] fireSound;
    [SerializeField] protected ParticleSystem muzzleFlash;

    //protected GameObject GetMagazineInfo()
    //{
    //    //get the interactor
    //}

    //protected void GetAmmoInfoFromMagazine(out int maxAmmo, out int currentAmmo)
    //{
    //    //get the magazine script from the magazine that connects
    //}

}





public enum FiringMode
{ 
    single,
    threeRoundBurst,
    auto,
    safe
}
public enum gunType
{
    handgun,//only alows the handgun magazine
    rifle, //only alows the rifle magazines
    Launcher //only alows the launcher magazine
}
public enum GunPart
{
    Gun,
    Magazine
}


