using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] GameObject lightning, warning;
    float animSpace;
    [SerializeField] float shake = 0.35f;
    int fire = 0;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        Physics.IgnoreCollision(lightning.GetComponent<BoxCollider>(), player.GetComponent<CapsuleCollider>());
    }

    private void Update()
    {
        animSpace += Time.deltaTime;
        if (animSpace < 0.6f)
        {
            warning.SetActive(true);
            lightning.SetActive(false);
            return;
        }
        if (animSpace < 0.9f)
        {
            if (fire == 0)
            {
                fire = 1;
                CameraController.instance.shake = shake;
            }
            warning.SetActive(false);
            lightning.SetActive(true);
            return;
        }
        Destroy(gameObject);
    }
}
