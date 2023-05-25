using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    public enum ColorType
    {
        Red,
        Green,
        Blue,
        BaseColor
    }

    [SerializeField] private Color baseColor;
    [SerializeField] private ColorType colorType;
    [SerializeField] private XRBaseInteractable interactable;

    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        interactable = GetComponent<XRBaseInteractable>();
        Debug.Log(interactable);
        interactable.hoverEntered.AddListener(ChangeColor);
    }
    public void Update()
    {
        
    }

    public void ChangeColor( BaseInteractionEventArgs hover)
    {
        switch (colorType)
        {
            case ColorType.Red:
                material.color = Color.red;
                break;

            case ColorType.Green:
                material.color = Color.green;
                break;

            case ColorType.Blue:
                material.color = Color.blue;
                break;

            case ColorType.BaseColor:
                material.color = baseColor;
                break;

            default:
                Debug.LogWarning($"Unknown color type: {colorType}");
                break;
        }
    }
}
