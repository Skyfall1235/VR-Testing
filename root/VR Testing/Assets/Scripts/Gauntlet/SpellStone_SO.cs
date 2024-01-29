using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpellStone", menuName = "SpellStone")]
public class SpellStone_SO : ScriptableObject
{
    public enum SpellStoneType
    {
        fire,
        water,
        earth
    }
    public SpellStoneType type;

    public bool isCharging = false;
    public int ownerID;
    public LayerMask playerMask;

    public void InitOwner(string ownerID)
    { this.ownerID = GetInstanceID(); }
}

