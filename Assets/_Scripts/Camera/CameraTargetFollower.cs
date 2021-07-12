using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform secondaryTarget;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.gameObject.activeInHierarchy)

            transform.position = new Vector3(target.position.x, 0f, target.position.z);

        else if (secondaryTarget)
            if (secondaryTarget.gameObject.activeInHierarchy)

                transform.position = new Vector3(secondaryTarget.position.x, 0f, secondaryTarget.position.z);
    }
}
