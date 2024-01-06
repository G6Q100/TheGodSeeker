using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public static CameraController instance = null;

    public float shake;
    public int maxZ = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + new Vector3(0, 50, -48);
        desiredPosition = new Vector3(Mathf.Clamp(desiredPosition.x, -30, 30), desiredPosition.y, Mathf.Clamp(desiredPosition.z, -48, maxZ));
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, 0.05f);

        if (shake > 0)
            shake -= Time.deltaTime;
        else
            shake = 0;

        transform.position = smoothPosition + Vector3.one * shake * Random.Range(1f,-1f);

    }
}
