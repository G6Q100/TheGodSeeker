using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemylaser : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
            return;
        if (other.gameObject.tag == "Enemy")
            return;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HP>().Damaged(1, 0);
        }

        gameObject.SetActive(false);
    }
}
