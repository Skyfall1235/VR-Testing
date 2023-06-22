using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Magazine : MonoBehaviour
{
    public MagazineData MagazineData = new MagazineData(gunType.rifle, GunPart.Magazine, 20, 20);
    [SerializeField] XRGrabInteractable grabInteractable;

    [SerializeField] InteractionLayerMask toggleLayerMask;
    InteractionLayerMask originalLayerMask;

    public void TestInteractionLayer()
    {
        //basscially, switch the interaction layer, as a coroutine
        StartCoroutine("SwitchLayer");
    }
    private IEnumerator SwitchLayer()
    {
        Debug.Log($"saving the layer mask as {originalLayerMask}");
        //save the old one
        originalLayerMask = grabInteractable.interactionLayers;
        //change to new
        grabInteractable.interactionLayers = toggleLayerMask;
        Debug.Log($"updating the layer mask as {toggleLayerMask}");
        //wait, then change back and end
        yield return new WaitForSeconds(1f);
        grabInteractable.interactionLayers = originalLayerMask;
        Debug.Log($"switching back to {originalLayerMask}");
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with something");
        if(collision.gameObject.GetComponent<Rifle>() != null)
        {
            Debug.Log("collided with a rifle");
            collision.gameObject.GetComponent<Rifle>().GetMagazineInfo(this.gameObject);
            collision.gameObject.GetComponent<Rifle>().GetMagazineInfo(null);
        }
    }

}
[System.Serializable]
public struct MagazineData 
{
    [SerializeField] public gunType associatedGun;
    [SerializeField] public GunPart part;
    [SerializeField] public int maxAmmoAmount;
    [SerializeField] public int currentAmmo;

    public MagazineData(gunType associatedGun, GunPart part, int maxAmmoAmount, int currentAmmo)
    {
        this.associatedGun = associatedGun;
        this.part = part;
        this.maxAmmoAmount = maxAmmoAmount;
        this.currentAmmo = currentAmmo;
    }
}

