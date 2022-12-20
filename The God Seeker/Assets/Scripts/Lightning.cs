using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] GameObject lightning, warning;
    float animSpace;
    int fire = 0;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        Physics.IgnoreCollision(lightning.GetComponent<BoxCollider>(), player.GetComponent<SphereCollider>());
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
        if (animSpace < 1)
        {
            if (fire == 0)
            {
                fire = 1;
                CameraController.instance.shake = 0.25f;
            }
            warning.SetActive(false);
            lightning.SetActive(true);
            return;
        }
        Destroy(gameObject);
    }
}
