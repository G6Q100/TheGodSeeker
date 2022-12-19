using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Rigidbody rb;

    private void FixedUpdate()
    {
        gameObject.transform.localScale += Vector3.one * (5 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
            return;
        if (other.gameObject.tag == "Enemy")
            return;
        if (other.gameObject.tag == "Segment")
            return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HP>().Damaged(1, 0);
        }
    }
}
