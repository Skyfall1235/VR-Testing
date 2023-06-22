using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class keepthesesynced : MonoBehaviour
{
    public Transform playerHead; // Reference to the player's head transform
    public float rotationSpeed = 1.0f;

    private void Awake()
    {
        //transform.position = new Vector3(playerHead.position.x, transform.position.y, playerHead.position.z);
    }
    void Update()
    {
        // Set the VR origin position to match the player's head position
        //transform.position = new Vector3(playerHead.position.x, transform.position.y, playerHead.position.z);
        transform.RotateAround(new Vector3(playerHead.position.x, playerHead.position.y, 0f), playerHead.transform.forward, rotationSpeed * Time.deltaTime);

        // this fdoes what i need it to do, but we need to roate the entire part to a lerped position for left and right, and slide.
        //also, VINGETTE PLEASE DEAR GOD THIS TRIGGERED THE MOTION SICKNESS IN A MAN WHO HAS 2000 HOURS OF HAVING IT RTEMOVED

    }

    //we want to keep the head at the top of the collider of the XR orirgin, scaling the collider as neccisiary. (i think it already does this, but only compared to the floor)

    //we want to sync up the x&y posistion of the 2 objects instantatniously




}
