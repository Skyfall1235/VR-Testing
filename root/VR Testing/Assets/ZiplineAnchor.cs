using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZiplineAnchor : MonoBehaviour
{
    public bool isHeld;
    public bool isAttached;
    public bool startMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHeld && startMovement)
        {
            AttemptMove();
        }
    }
    
    private void AttemptMove()
    {
        
    }

    private void StartMovement()
    {

    }
}
