using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float speed = 10f;
    public Transform tip;

    Rigidbody body;
    bool inAir = false;
    Vector3 lastPosition = Vector3.zero;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //add action to release

        Stop();
    }

    private void OnDestroy()
    {
        //remove the action
    }

    void Release(float value)
    {
        if(Physics.Linecast(lastPosition, tip.position, out RaycastHit hitInfo))
        {
            if(hitInfo.transform.TryGetComponent(out Rigidbody body))
            {
                body.interpolation = RigidbodyInterpolation.None;
                transform.parent = hitInfo.transform;
                body.AddForce(body.velocity,ForceMode.Impulse);
            }
            Stop();
        }
    }

    private void Stop()
    {
        inAir = false;
        SetPhysics(false);
    }

    void SetPhysics(bool usePhysics)
    {
        body.useGravity = usePhysics;
        body.isKinematic = !usePhysics;
    }

}
