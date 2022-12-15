using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 pos;

    void FixedUpdate()
    {
        pos = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);

        pos = player.transform.position + new Vector3(0, 10, -9);

        transform.position = pos;
    }
}
