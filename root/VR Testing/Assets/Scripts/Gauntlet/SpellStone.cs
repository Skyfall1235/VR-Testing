using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellStone : MonoBehaviour
{
    [SerializeField] private SpellStone_SO _SO;

    //has a chartge up event
    //activates X ability
    //is base class
    //should have ready to go slots for FX
    public virtual void PlayPFX() {}
    //has an impact on play and impact on terrain methods
    public virtual void OnTerrainImpact() {}
    public virtual void OnPlayerImpact() {}

    private void OnCollisionEnter(Collision collision)
    {
        //determine impact method based on tag
        //confirm its not the owner colliding
        if(collision.gameObject.GetInstanceID() == _SO.ownerID) { return; }

        //assume terrain
        if (collision.gameObject.layer != _SO.playerMask)
        {
            OnTerrainImpact();
            return;
        }
        OnPlayerImpact();
    }
}
