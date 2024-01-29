using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZiplineLink : MonoBehaviour
{
    //write code first

    //you cane change to private or whatever after
    public LineRenderer tetherLine;
    public GameObject tetherLink;
    //public bool isEndOfLine = false;  not needed yet
    public Color lineColor;
    public Color lineColorEnd;
    public float lineWidth;

    private void Start()
    {
        LinkTethers();
        SetLineWidth(lineWidth);
    }

    private void OnValidate()
    {
        tetherLine.startWidth = lineWidth;
        tetherLine.endWidth = lineWidth;
        LinkTethers();
    }

    //draw the line from tether to tether link with tether line
    private void LinkTethers()
    {
        tetherLine.positionCount = 2;
        tetherLine.SetPosition(0, transform.position);
        tetherLine.SetPosition(1, tetherLink.transform.position);
        tetherLine.startColor = lineColor;
        tetherLine.endColor = lineColorEnd;
    }

    private void SetLineWidth(float width)
    {
        tetherLine.startWidth = width;
        tetherLine.endWidth = width;
    }

    public void CommandAnchor()
    {
        XRSocketInteractor socketInteractor = GetComponent<XRSocketInteractor>();
        //XRBaseInteractable selectedInteractable = select
    }
}
