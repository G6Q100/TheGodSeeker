using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemylaser : MonoBehaviour
{
    [SerializeField]int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HP>().Damaged(damage, 0);
        }
    }
}
