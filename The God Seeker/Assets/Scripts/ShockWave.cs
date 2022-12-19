using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    float alpha;
    private void FixedUpdate()
    {
        gameObject.transform.localScale += Vector3.one * 15f * Time.deltaTime;
        alpha += Time.deltaTime;
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
