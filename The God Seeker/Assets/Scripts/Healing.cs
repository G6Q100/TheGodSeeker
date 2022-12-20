using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<HP>().healthPoint < 10)
                other.gameObject.GetComponent<HP>().healthPoint++;
            Destroy(gameObject);
        }
    }
}
