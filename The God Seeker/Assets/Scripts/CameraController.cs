using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + new Vector3(0, 50, -48);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, 0.2f);

        transform.position = smoothPosition;
    }
}
