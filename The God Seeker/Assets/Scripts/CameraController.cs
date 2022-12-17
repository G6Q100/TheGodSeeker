using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + new Vector3(0, 50, -48);
        desiredPosition = new Vector3(desiredPosition.x, desiredPosition.y, Mathf.Clamp(desiredPosition.z, -48, 100));
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, 0.2f);

        transform.position = smoothPosition;
    }
}
