using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentRotation : MonoBehaviour
{
    public float speed;

    public Vector3 direction;
    public Transform target;

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speed * Time.deltaTime);
    }
}
