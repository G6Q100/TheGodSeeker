using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    [SerializeField]
    Transform lookAt;
    [SerializeField]
    Vector3 offset;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(lookAt.position + offset);

        if (transform.position != pos)
            transform.position = pos;
    }
}
