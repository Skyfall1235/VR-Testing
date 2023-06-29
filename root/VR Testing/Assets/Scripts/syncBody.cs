using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncBody : MonoBehaviour
{
    [SerializeField] GameObject xrOrigin;
    [SerializeField] GameObject cameraDirection;
    private CharacterController xrController;
    [SerializeField] private float bodyOffset = 2f;
    // Start is called before the first frame update
    void Start()
    {
        xrController =xrOrigin.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
            
        transform.localPosition = new Vector3(xrController.center.x, xrController.height - bodyOffset, xrController.center.z);
        //Quaternion cameriaAdjusted = new Vector3(bodyOffset, 0, 0);
        //transform.rotation = Quaternion.Euler(0, cameraDirection.transform.forward.y, 0);
        //transform.rotation = Quaternion.Euler(0, cameraDirection.transform.forward.y, 0);
        Vector3 cameraForward = cameraDirection.transform.forward;
        float cameraYRotation = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);

        DebugRay();
    }

    private void DebugRay()
    {
        //Debug.Log("sending Ray");
        Vector3 position = transform.position;
        Vector3 forwardDirection = new Vector3(0, cameraDirection.transform.rotation.y, 0);

        // Set the length of the debug line
        float lineLength = 5f;

        // Calculate the end position of the line
        Vector3 endPosition = position + forwardDirection * lineLength;

        // Draw the debug line from the object's position to the end position
        Debug.DrawLine(position, endPosition, Color.red);
    }


    //to expand on this you could have a smoothing motion or a thershold or rotation hit before turning with the head, and then a set delay before turning fulling towards the players direction, or not at all

}
