using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    public LayerMask FloorLayer;

    private void FixedUpdate()
    {
        Vector3 vectorToCenter = new Vector3(0, 0, transform.position.z) - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, vectorToCenter) * transform.rotation, 0.6f);
    }
}
