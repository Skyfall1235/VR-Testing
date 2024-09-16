using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable bow;
    bool arrowNotched = false;
    GameObject currentArrow = null;

    private void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        //PullInteraction.PullAction += NotchEmpty;
    }

    private void OnDestroy()
    {
        //PullInteraction.PullAction -= NotchEmpty;
    }

    private void Update()
    {
        if(bow.isSelected && arrowNotched == false)
        {
            arrowNotched = true;
            StartCoroutine(DelayedSpawn());
        }
        if(!bow.isSelected && currentArrow != null)
        {
            Destroy(currentArrow);
            NotchEmpty(1f);
        }
    }

    void NotchEmpty(float value)
    {
        arrowNotched = false;
        currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1);
        currentArrow = Instantiate(arrow, notch.transform);
    }
}
